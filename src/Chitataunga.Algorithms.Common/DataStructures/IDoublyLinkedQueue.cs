using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public interface IDoublyLinkedQueue<T> : IEnumerable<T>, IDisposable, IAsyncDisposable
    {
        uint Count { get; }
        ulong LongCount { get; }

        void Clear();
        ValueTask ClearAsync();
        T Dequeue();
        void Enqueue(T item);
        bool IsEmpty();
        T Peek();
        bool TryPeek(out T result);
    }
}