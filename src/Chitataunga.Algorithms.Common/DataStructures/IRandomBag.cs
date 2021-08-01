using System;
using System.Collections.Generic;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public interface IRandomBag<T> : IEnumerable<T>, IDisposable, IAsyncDisposable
    {
        void Add(T item);
        bool IsEmpty();
        uint Count { get; }
        ulong LongCount { get; }
    }
}