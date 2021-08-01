using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public interface IPushDownStack<T> : IEnumerable<T>, IAsyncDisposable, IDisposable
    {
        uint Count { get; }
        ulong LongCount { get; }

        void Clear();
        ValueTask ClearAsync();
        bool IsEmpty();
        T Peek();
        T Pop();
        void Push(T item);
        bool TryPeek(out T result);
        bool TryPop(out T result);
    }
}