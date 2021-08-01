using System.Collections.Generic;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public interface IDoublyLinkedList<T> :  IEnumerable<T>
    {
        uint Count { get; }
        ulong LongCount { get; }
        void Clear();
        T DeleteAtBack();
        T DeleteAtFront();
        void InsertAtBack(T item);
        void InsertAtFront(T item);
        bool IsEmpty();
        T PeekBack();
        T PeekFront();
        bool TryDeleteAtBack(out T result);
        bool TryDeleteAtFront(out T result);
        bool TryPeekBack(out T result);
        bool TryPeekFront(out T result);
    }
}