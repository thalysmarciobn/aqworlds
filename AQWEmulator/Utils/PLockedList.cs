using System;
using System.Collections.Generic;

namespace AQWEmulator.Utils
{
    public class PLockedList<T> : List<T>
    {
        private readonly object _lock = new object();

        public PLockedList()
        {
        }

        public PLockedList(IEnumerable<T> otherlist)
            : base(otherlist)
        {
        }

        public bool TryAdd(T item)
        {
            lock (_lock)
            {
                if (Contains(item)) return false;
                Add(item);
                return true;
            }
        }

        public new bool Contains(T item)
        {
            lock (_lock)
            {
                return base.Contains(item);
            }
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            lock (_lock)
            {
                base.InsertRange(index, collection);
            }
        }

        public new bool Remove(T item)
        {
            lock (_lock)
            {
                return base.Remove(item);
            }
        }

        public new void Add(T item)
        {
            lock (_lock)
            {
                base.Add(item);
            }
        }

        public new void ForEach(Action<T> action)
        {
            GetCopy().ForEach(action);
        }

        public new void Clear()
        {
            lock (_lock)
            {
                base.Clear();
            }
        }

        /// <summary>
        ///     Get a copy and clear list.
        /// </summary>
        /// <returns></returns>
        public List<T> Empty()
        {
            lock (_lock)
            {
                var copy = GetCopy();
                base.Clear();
                TrimExcess();
                return copy;
            }
        }

        public List<T> GetCopy()
        {
            lock (_lock)
            {
                return new List<T>(this);
            }
        }

        public new T Find(Predicate<T> match)
        {
            lock (_lock)
            {
                return base.Find(match);
            }
        }
    }
}