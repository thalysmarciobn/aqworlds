using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using System.ServiceModel.Channels;
using AQWEmulator.Network.Sessions;

namespace AQWEmulator.Network.Pool
{
    internal sealed class NetworkReceivePool
    {
        private readonly ConcurrentStack<SocketAsyncEventArgs> _pool;
        private readonly BufferManager _bufferManager;

        public NetworkReceivePool(int capacity, EventHandler<SocketAsyncEventArgs> completed, int singleBufferMaxSize = 8 * 1024)
        {
            _pool = new ConcurrentStack<SocketAsyncEventArgs>();
            _bufferManager = BufferManager.CreateBufferManager(singleBufferMaxSize * capacity, singleBufferMaxSize);
            for (var i = 0; i < capacity; i++)
            {
                var buffer = _bufferManager.TakeBuffer(singleBufferMaxSize);
                var acceptEventArg = new SocketAsyncEventArgs
                {
                    UserToken = new Session(i)
                };
                acceptEventArg.Completed += completed;
                acceptEventArg.SetBuffer(buffer, 0, buffer.Length);
                _pool.Push(acceptEventArg);
            }
        }

        public bool TryPop(out SocketAsyncEventArgs args)
        {
            return _pool.TryPop(out args);
        }

        public void Push(SocketAsyncEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }
            lock (this)
            {
                args.AcceptSocket = null;
                args.RemoteEndPoint = null;
                args.DisconnectReuseSocket = true;
            }
            _pool.Push(args);
        }

        public void Dispose()
        {
            while (_pool.Count > 0)
            {
                if (_pool.TryPop(out var eventArgs))
                {
                    eventArgs.Dispose();
                }
            }
        }
    }
}