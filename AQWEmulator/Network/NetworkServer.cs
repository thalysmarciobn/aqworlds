using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AQWEmulator.Network.Packet;
using AQWEmulator.Network.Pool;
using AQWEmulator.Network.Sessions;
using AQWEmulator.Settings;
using AQWEmulator.Utils.Log;
using AQWEmulator.World;
using AQWEmulator.World.Users;

namespace AQWEmulator.Network
{
    public class NetworkServer
    {
        private readonly NetworkSettings _settings;
        
        private SemaphoreSlim _maxConnectionsEnforcer;
        private SemaphoreSlim _maxSaeaSendEnforcer;
        private SemaphoreSlim _maxAcceptOpsEnforcer;
        
        private Socket _listenSocket;
        
        private NetworkAcceptPool _poolOfAcceptEventArgs;
        private NetworkReceivePool _poolOfRecEventArgs;
        private NetworkSendPool _poolOfSendEventArgs;

        public NetworkServer(NetworkSettings settings)
        {
            _settings = settings;
        }

        public NetworkServer Init()
        {
            _poolOfAcceptEventArgs = new NetworkAcceptPool(_settings.MaxSimultaneousAcceptOps, AcceptEventArg_Completed);
            _poolOfRecEventArgs = new NetworkReceivePool(_settings.NumOfSaeaForRec, IO_ReceiveCompleted);
            _poolOfRecEventArgs = new NetworkReceivePool(_settings.NumOfSaeaForRec, IO_ReceiveCompleted);
            _poolOfSendEventArgs = new NetworkSendPool(_settings.NumOfSaeaForSend, IO_SendCompleted);

            _maxConnectionsEnforcer = new SemaphoreSlim(_settings.MaxConnections, _settings.MaxConnections);
            _maxSaeaSendEnforcer = new SemaphoreSlim(_settings.NumOfSaeaForSend, _settings.NumOfSaeaForSend);
            _maxAcceptOpsEnforcer = new SemaphoreSlim(_settings.MaxSimultaneousAcceptOps, _settings.MaxSimultaneousAcceptOps);
            
            _listenSocket = new Socket(_settings.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            return this;
        }

        public void Bind()
        {
            _listenSocket.Bind(_settings.Endpoint);
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
            _maxSaeaSendEnforcer.Wait();
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
            _maxSaeaSendEnforcer.Release();
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
        }
    }
}