using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public class PushDownStack<T> : IPushDownStack<T>
    {
        private DoublyLinkedList<T> _list;
        public PushDownStack()
        {
            _list = new();
        }
        /// <summary>
        /// inserts an item at the top of stack
        /// </summary>
        /// <param name="item"></param>
        public void Push(T item) => _list.InsertAtFront(item);
        /// <summary>
        /// removes the item from the top of collection and returns the value
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exceptions.EmptyCollectionException">throws exception if empty</exception>
        public T Pop()
        {
            return _list.DeleteAtFront();
        }
        /// <summary>
        /// if delete was successful, returns true and sets result to value at front, otherwise result is set to default(T) and
        /// returns false
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPop(out T result) => _list.TryDeleteAtFront(out result);

        /// <summary>
        /// reads the item from the front / top of stack and returns the value
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exceptions.EmptyCollectionException">throws exception if empty</exception>
        public T Peek() => _list.PeekFront();

        /// <summary>
        /// Returns the value at the front of the stack
        /// if collection is empty, will result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeek(out T result) => _list.TryPeekFront(out result);

        public uint Count
        {
            get => _list.Count;
        }
        public ulong LongCount
        {
            get => _list.LongCount;
        }
        public void Clear() => _list.Clear();
        public async ValueTask ClearAsync() => await Task.Run(() => _list.Clear());
        public bool IsEmpty() => _list.IsEmpty();

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private async ValueTask DisposeAsyncUtil()
        {
            await Task.Run(() => _list.Clear());
            _list = null;

        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncUtil();
        }

        public void Dispose()
        {
            _list.Clear();
            _list = null;
        }
    }
}
