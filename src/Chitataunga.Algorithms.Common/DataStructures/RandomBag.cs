using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public class RandomBag<T> : IRandomBag<T>
    {
        private PushDownStack<T> _stack;
        public uint Count { get => _stack.Count; }

        public ulong LongCount  { get => _stack.LongCount;  }

        public void Add(T item)
        {
            _stack.Push(item);
        }

        public void Dispose()
        {
            _stack.Dispose();
            _stack = null;
        }

        public async ValueTask DisposeAsync()
        {
            await _stack.DisposeAsync();
            _stack = null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _stack.GetEnumerator();
        }

        public bool IsEmpty()
        {
            return _stack.IsEmpty();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
