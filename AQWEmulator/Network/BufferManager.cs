using System.Collections.Generic;
using System.Net.Sockets;

namespace AQWEmulator.Network
{
    internal sealed class BufferManager
    {
        private readonly int totalBytesInBufferBlock;

        private byte[] _bufferBlock;
        private readonly Stack<int> _freeIndexPool;
        private int _currentIndex;
        private readonly int _bufferBytesAllocatedForEachSaea;

        public BufferManager(int totalBytes, int totalBufferBytesInEachSaeaObject)
        {
            totalBytesInBufferBlock = totalBytes;
            _currentIndex = 0;
            _bufferBytesAllocatedForEachSaea = totalBufferBytesInEachSaeaObject;
            _freeIndexPool = new Stack<int>();
        }

        public void InitBuffer()
        {
            _bufferBlock = new byte[totalBytesInBufferBlock];
        }

        public bool SetBuffer(SocketAsyncEventArgs args)
        {
            if (_freeIndexPool.Count > 0)
            {
                args.SetBuffer(_bufferBlock, _freeIndexPool.Pop(), _bufferBytesAllocatedForEachSaea);
            }
            else
            {
                if ((totalBytesInBufferBlock - _bufferBytesAllocatedForEachSaea) < _currentIndex)
                {
                    return false;
                }

                args.SetBuffer(_bufferBlock, _currentIndex, _bufferBytesAllocatedForEachSaea);
                _currentIndex += _bufferBytesAllocatedForEachSaea;
            }

            return true;
        }

        public void FreeBuffer(SocketAsyncEventArgs args)
        {
            _freeIndexPool.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}