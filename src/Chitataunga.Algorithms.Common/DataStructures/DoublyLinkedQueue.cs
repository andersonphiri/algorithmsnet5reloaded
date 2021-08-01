using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    /// <summary>
    /// Queue implementation using doubly linked lists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DoublyLinkedQueue<T> :  IDoublyLinkedQueue<T>
    {
        private DoublyLinkedList<T> _list;
        public DoublyLinkedQueue()
        {
            _list = new DoublyLinkedList<T>();
        }

        public void Enqueue(T item) => _list.InsertAtBack(item);
        public T Dequeue() => _list.DeleteAtFront();

        public T Peek() => _list.PeekFront();
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
