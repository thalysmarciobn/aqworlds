using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace AQWEmulator.Network.Pool
{
    internal sealed class NetworkAcceptPool
    {
        private readonly ConcurrentStack<SocketAsyncEventArgs> _pool;

        public NetworkAcceptPool(int capacity, EventHandler<SocketAsyncEventArgs> completed)
        {
            _pool = new ConcurrentStack<SocketAsyncEventArgs>();
            for (var i = 0; i < capacity; i++)
            {
                var acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += completed;
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