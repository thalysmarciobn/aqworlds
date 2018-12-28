using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AQWEmulator.Network.Packet;
using AQWEmulator.Network.Pool;
using AQWEmulator.Network.Sessions;
using AQWEmulator.Utils.Log;
using AQWEmulator.World;
using AQWEmulator.World.Users;
using AQWEmulator.Xml.Game;

namespace AQWEmulator.Network
{
    public class NetworkServer
    {
        public readonly IPEndPoint IpEndPoint;
        private readonly ServerNetwork _settings;
        
        private readonly SemaphoreSlim _maxConnectionsEnforcer;
        private readonly SemaphoreSlim _maxSendEnforcer;
        private readonly SemaphoreSlim _maxAcceptOpsEnforcer;
        
        private readonly Socket _listenSocket;
        
        private readonly NetworkAcceptPool _poolOfAcceptEventArgs;
        private readonly NetworkReceivePool _poolOfRecEventArgs;
        private readonly NetworkSendPool _poolOfSendEventArgs;

        public NetworkServer(ServerNetwork settings)
        {
            _poolOfAcceptEventArgs = new NetworkAcceptPool(settings.MaxSimultaneousAcceptOps, AcceptEventArg_Completed);
            _poolOfRecEventArgs = new NetworkReceivePool(settings.NumOfSaeaForRec, IO_ReceiveCompleted);
            _poolOfSendEventArgs = new NetworkSendPool(settings.NumOfSaeaForSend, IO_SendCompleted);

            _maxConnectionsEnforcer = new SemaphoreSlim(settings.MaxConnections, settings.MaxConnections);
            _maxSendEnforcer = new SemaphoreSlim(settings.NumOfSaeaForSend, settings.NumOfSaeaForSend);
            _maxAcceptOpsEnforcer = new SemaphoreSlim(settings.MaxSimultaneousAcceptOps, settings.MaxSimultaneousAcceptOps);
            
            IpEndPoint =
                new IPEndPoint(
                    settings.Host.Equals("*")
                        ? IPAddress.Any
                        : (IPAddress.TryParse(settings.Host, out var address) ? address : IPAddress.Any),
                    settings.Port);
            _listenSocket = new Socket(IpEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _settings = settings;
        }

        public void Bind()
        {
            _listenSocket.Bind(IpEndPoint);
            _listenSocket.Listen(_settings.Backlog);
            StartAccept();
        }
        
        private void StartAccept()
        {
            _maxAcceptOpsEnforcer.Wait();
            if (!_poolOfAcceptEventArgs.TryPop(out var acceptEventArgs)) return;
            try
            {
                _maxConnectionsEnforcer.Wait();
                var willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArgs);
                }
            }
            catch
            {
                // ignored
            }
        }
        
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            StartAccept();

            if (acceptEventArgs.SocketError != SocketError.Success)
            {
                HandleBadAccept(acceptEventArgs);
                _maxAcceptOpsEnforcer.Release();
                return;
            }
            if (_poolOfRecEventArgs.TryPop(out var recEventArgs))
            {
                var session = new Session(recEventArgs, acceptEventArgs.AcceptSocket, this);
                recEventArgs.UserToken = session;
                
                WriteConsole.Session("New connection", session.Address);
                acceptEventArgs.AcceptSocket = null;
                _poolOfAcceptEventArgs.Push(acceptEventArgs);
                _maxAcceptOpsEnforcer.Release();
                StartReceive(recEventArgs);
            }
            else
            {
                HandleBadAccept(acceptEventArgs);
                Console.WriteLine("Cannot handle this session, there are no more receive objects available for us.");
            }
        }

        private void IO_SendCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Send)
            {
                throw new InvalidOperationException("Tried to pass a send operation but the operation expected was not a send.");
            }

            ProcessSend(e);
        }

        private void IO_ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.LastOperation != SocketAsyncOperation.Receive)
            {
                throw new InvalidOperationException("Tried to pass a receive operation but the operation expected was not a receive.");
            }

            ProcessReceive(e);
        }

        private void StartReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            var willRaiseEvent = true;
            if (receiveEventArgs.UserToken.GetType() == typeof(Session))
            {
                var session = (Session)receiveEventArgs.UserToken;
                willRaiseEvent = session.ReceiveAsync(receiveEventArgs);
            }
            else if (receiveEventArgs.UserToken.GetType() == typeof(User))
            {
                var user = (User)receiveEventArgs.UserToken;
                willRaiseEvent = user.Session.ReceiveAsync(receiveEventArgs);
            }
            if (!willRaiseEvent)
            {
                ProcessReceive(receiveEventArgs);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            if (receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success)
            {
                var dataReceived = new byte[receiveEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveEventArgs.Buffer, receiveEventArgs.Offset, dataReceived, 0, receiveEventArgs.BytesTransferred);
                if (receiveEventArgs.UserToken.GetType() == typeof(Session))
                {
                    var session = (Session) receiveEventArgs.UserToken;
                    session.Receive(Encoding.UTF8.GetString(dataReceived));
                }
                else if (receiveEventArgs.UserToken.GetType() == typeof(User))
                {
                    var user = (User) receiveEventArgs.UserToken;
                    var packet = Encoding.UTF8.GetString(dataReceived);
                    var dataArray = packet.Substring(0, packet.Length - 1).Split(Convert.ToChar(0x0));
                    foreach (var message in dataArray)
                    {
                        var _params = message.Substring(1, message.Length - 2).Split('%');
                        if (_params.Length <= 3) return;
                        var newParams = new string[_params.Length - 4];
                        if (!_params[0].Equals("xt") || !_params[1].Equals("zm")) return;
                        var cmd = _params[2];
                        if (!int.TryParse(_params[3], out var room)) room = 0;
                        Array.Copy(_params, 4, newParams, 0, _params.Length - 4);
                        PacketProcessor.TryHandlePacket(user, cmd, room, newParams);
                        //if (TryGet(cmd, out var packetEvent))
                        //    packetEvent.Dispatch(user, room, newParams);
                        //else if (TryGetCustom(cmd, out var custom))
                        //    custom.Parser(user, room, newParams);
                        //else
                        //    Console.WriteLine($"[{user.Name}] Packet not found: {cmd}");
                    }
                }
                StartReceive(receiveEventArgs);
            }
            else
            {
                CloseClientSocket(receiveEventArgs);
                ReturnReceiveSaea(receiveEventArgs);
            }
        }

        public void SendData(Socket socket, byte[] data)
        {
            _maxSendEnforcer.Wait();
            if (!_poolOfSendEventArgs.TryPop(out var sendEventArgs)) return;
            var token = (SendDataToken) sendEventArgs.UserToken;
            token.DataToSend = data;
            token.SendBytesRemainingCount = data.Length;

            sendEventArgs.AcceptSocket = socket;
            StartSend(sendEventArgs);
        }

        private void StartSend(SocketAsyncEventArgs sendEventArgs)
        {
            var token = (SendDataToken)sendEventArgs.UserToken;

            if (token.SendBytesRemainingCount <= _settings.BufferSize)
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, token.SendBytesRemainingCount);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer, sendEventArgs.Offset, token.SendBytesRemainingCount);
            }
            else
            {
                sendEventArgs.SetBuffer(sendEventArgs.Offset, _settings.BufferSize);
                Buffer.BlockCopy(token.DataToSend, token.BytesSentAlreadyCount, sendEventArgs.Buffer, sendEventArgs.Offset, _settings.BufferSize);
            }

            var willRaiseEvent = sendEventArgs.AcceptSocket.SendAsync(sendEventArgs);

            if (!willRaiseEvent)
            {
                ProcessSend(sendEventArgs);
            }
        }

        private void ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            try
            {
                var token = (SendDataToken) sendEventArgs.UserToken;

                if (sendEventArgs.SocketError == SocketError.Success)
                {
                    token.SendBytesRemainingCount = token.SendBytesRemainingCount - sendEventArgs.BytesTransferred;

                    if (token.SendBytesRemainingCount == 0)
                    {
                        token.Reset();
                        ReturnSendSaea(sendEventArgs);
                    }
                    else
                    {
                        token.BytesSentAlreadyCount += sendEventArgs.BytesTransferred;
                        StartSend(sendEventArgs);
                    }
                }
                else
                {
                    token.Reset();
                    CloseClientSocket(sendEventArgs);
                    ReturnSendSaea(sendEventArgs);
                }
            }
            catch
            {
                
            }
        }

        private void CloseClientSocket(SocketAsyncEventArgs args)
        {
            if (args.UserToken.GetType() == typeof(Session))
            {
                var session = (Session) args.UserToken;
                WriteConsole.Session("Connection lost", session.Address);
                try
                {
                    session.Shutdown();
                }
                catch (SocketException)
                {
                }
                //_connections.Remove(session);
            }
            else if (args.UserToken.GetType() == typeof(User))
            {
                var user = (User) args.UserToken;
                WriteConsole.Session("Connection lost", user.Name);
                UsersManager.Instance.Remove(user);
                user.RoomUser.Remove();
                try {
                    user.Session.Shutdown();
                }
                catch (SocketException)
                {
                }
            }

        }

        private void ReturnReceiveSaea(SocketAsyncEventArgs args)
        {
            _maxConnectionsEnforcer.Release();
            _poolOfRecEventArgs.Push(args);
        }

        private void ReturnSendSaea(SocketAsyncEventArgs args)
        {
            _poolOfSendEventArgs.Push(args);
            _maxSendEnforcer.Release();
        }

        private void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            _poolOfAcceptEventArgs.Push(acceptEventArgs);
            try
            {
                acceptEventArgs.AcceptSocket.Shutdown(SocketShutdown.Both);
                acceptEventArgs.AcceptSocket.Close();
            }
            catch
            {
                
            }
        }

        [Obsolete]
        public void Shutdown()
        {
            _listenSocket.Shutdown(SocketShutdown.Both);
            _listenSocket.Close();

            DisposeAllSaeaObjects();
        }

        private void DisposeAllSaeaObjects()
        {
            _poolOfAcceptEventArgs.Dispose();
            _poolOfSendEventArgs.Dispose();
            _poolOfRecEventArgs.Dispose();
            _maxConnectionsEnforcer.Dispose();
            _maxSendEnforcer.Dispose();
            _maxAcceptOpsEnforcer.Dispose();
        }
    }
}