using System;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace AQWEmulator.Network
{
    internal sealed class SocketAsyncEventArgsPool
    {
        private readonly ConcurrentStack<SocketAsyncEventArgs> _pool;

        public SocketAsyncEventArgsPool(int capacity)
        {
            _pool = new ConcurrentStack<SocketAsyncEventArgs>();
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