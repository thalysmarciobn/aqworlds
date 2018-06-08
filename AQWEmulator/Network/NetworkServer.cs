﻿using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AQWEmulator.Network.Sessions;
using AQWEmulator.Settings;

namespace AQWEmulator.Network
{
    public class NetworkServer
    {
        private NetworkSettings _settings;
        
        private BufferManager _bufferManager;
        
        private SemaphoreSlim _maxSaeaSendEnforcer;
        private SemaphoreSlim _maxAcceptOpsEnforcer;
        
        private Socket _listenSocket;
        
        private SocketAsyncEventArgsPool _poolOfAcceptEventArgs;
        private SocketAsyncEventArgsPool _poolOfRecEventArgs;
        private SocketAsyncEventArgsPool _poolOfSendEventArgs;

        public NetworkServer Build(NetworkSettings settings)
        {
            _settings = settings;
            _bufferManager = new BufferManager((_settings.BufferSize * _settings.NumOfSaeaForRec) + (_settings.BufferSize * _settings.NumOfSaeaForSend), _settings.BufferSize);
            _poolOfAcceptEventArgs = new SocketAsyncEventArgsPool(_settings.MaxSimultaneousAcceptOps);
            _poolOfRecEventArgs = new SocketAsyncEventArgsPool(_settings.NumOfSaeaForRec);
            _poolOfSendEventArgs = new SocketAsyncEventArgsPool(_settings.NumOfSaeaForSend);

            _maxSaeaSendEnforcer = new SemaphoreSlim(_settings.NumOfSaeaForSend, _settings.NumOfSaeaForSend);
            _maxAcceptOpsEnforcer = new SemaphoreSlim(_settings.MaxSimultaneousAcceptOps, _settings.MaxSimultaneousAcceptOps);
            return this;
        }

        public NetworkServer Init()
        {
            _bufferManager.InitBuffer();

            for (var i = 0; i < _settings.MaxSimultaneousAcceptOps; i++)
            {
                var acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed +=
                    AcceptEventArg_Completed;

                _poolOfAcceptEventArgs.Push(acceptEventArg);
            }
            for (var i = 0; i < _settings.NumOfSaeaForRec; i++)
            {
                var eventArgObjectForPool = new SocketAsyncEventArgs();
                if (!_bufferManager.SetBuffer(eventArgObjectForPool)) continue;
                eventArgObjectForPool.Completed +=
                    IO_ReceiveCompleted;
                eventArgObjectForPool.UserToken = new Session(i, this);
                _poolOfRecEventArgs.Push(eventArgObjectForPool);
            }

            // send objects
            for (var i = 0; i < _settings.NumOfSaeaForSend; i++)
            {
                var eventArgObjectForPool = new SocketAsyncEventArgs();
                if (!_bufferManager.SetBuffer(eventArgObjectForPool)) continue;
                eventArgObjectForPool.Completed +=
                    IO_SendCompleted;
                eventArgObjectForPool.UserToken = new SendDataToken();
                _poolOfSendEventArgs.Push(eventArgObjectForPool);
            }

            return this;
        }
        
        public void StartListen()
        {
            _listenSocket = new Socket(_settings.Endpoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
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
                var session = ((Session) recEventArgs.UserToken);
                session.Socket = acceptEventArgs.AcceptSocket;

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
            var token = (Session)receiveEventArgs.UserToken;

            var willRaiseEvent = token.Socket.ReceiveAsync(receiveEventArgs);

            if (!willRaiseEvent)
            {
                ProcessReceive(receiveEventArgs);
            }
        }

        private void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            var token = (Session)receiveEventArgs.UserToken;

            if (receiveEventArgs.BytesTransferred > 0 && receiveEventArgs.SocketError == SocketError.Success)
            {
                var dataReceived = new byte[receiveEventArgs.BytesTransferred];
                Buffer.BlockCopy(receiveEventArgs.Buffer, receiveEventArgs.Offset, dataReceived, 0, receiveEventArgs.BytesTransferred);
                if (!token.Logged)
                {
                    token.Receive(Encoding.UTF8.GetString(dataReceived));
                }
                else
                {
                    token.UserReceive(Encoding.UTF8.GetString(dataReceived));
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

        private static void CloseClientSocket(SocketAsyncEventArgs args)
        {
            var con = (Session)args.UserToken;

            try
            {
                con.Socket.Shutdown(SocketShutdown.Both);
                con.Socket.Close();
            }
            catch (SocketException) { }
            con.Disconnect();
        }

        private void ReturnReceiveSaea(SocketAsyncEventArgs args)
        {
            _poolOfRecEventArgs.Push(args);
        }

        private void ReturnSendSaea(SocketAsyncEventArgs args)
        {
            _poolOfSendEventArgs.Push(args);
            _maxSaeaSendEnforcer.Release();
        }

        private static void HandleBadAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            //_poolOfAcceptEventArgs.Push(acceptEventArgs);
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