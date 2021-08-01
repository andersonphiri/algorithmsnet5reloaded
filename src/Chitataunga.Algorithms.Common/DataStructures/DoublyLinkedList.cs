using Chitataunga.Algorithms.Common.Constants;
using Chitataunga.Algorithms.Common.Exceptions;
using Chitataunga.Algorithms.Common.Locks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Chitataunga.Algorithms.Common.DataStructures
{
    /// <summary>
    /// a data structure that allows you to insert front and back of the queue
    /// uses linkedList and not arrays
    /// </summary>
    public class DoublyLinkedList<T> : IDoublyLinkedList<T>
    {
        private ulong N;
        private Node _first;
        private Node _last;
        internal BooleanThreadSafe IsEnumerating { get; set; }
        internal BooleanThreadSafe IsClearing { get; set; }
        /// <summary>
        /// a data structure that allows you to insert front and back of the queue
        /// uses linkedList and not arrays
        /// </summary>
        public DoublyLinkedList()
        {
            _first = null;
            _last = null;
            N = 0;
            IsEnumerating = new();
            IsClearing = new();
        }

        public bool IsEmpty() => N == 0;
        public uint Count { get => (uint)N; }
        public ulong LongCount { get => N; }
        /// <summary>
        /// clears all elements in the linked list. Time complexity : O(N)
        /// </summary>
        /// <exception cref=""
        public void Clear()
        {
            if (IsEmpty()) return;
            if (IsEnumerating) throw new CollectionException(ExceptionConstants.DeleteWhileEnumerating, "Cannot clear collection while enumerating");
            IsClearing = true;
            Node first = _first;
            _first = _first.Next;
            while (_first is not null)
            {
                first.Prev = null;
                //first.Data = default;
                _first.Prev = null;
                _first = _first.Next;
                first = _first;
            }

            _first = null;
            _last = null;
            N = 0;
            IsEnumerating = false;
            IsClearing = false;
        }

        #region Peek logic
        ///Returns the value at the front of the queue
        ///<exception cref="EmptyCollectionException">throws exception when collection is empty </exception>
        public T PeekFront()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "The collection is empty");
            return _first.Data;
        }
        /// <summary>
        /// Returns the value at the front of the queue
        /// if collection is empty, will result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeekFront(out T result)
        {
            if (IsEmpty())
            {
                result = default;
                return false;
            }
            result = _first.Data;
            return true;
        }

        /// <summary>
        /// returns the last value at the back of the queue / list / collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exception when collection is empty </exception>
        public T PeekBack()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "The collection is empty");
            return _last.Data;
        }

        /// <summary>
        /// Returns the value at the back of the queue ie the last element to be processed
        /// if collection is empty, result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeekBack(out T result)
        {
            if (IsEmpty())
            {
                result = default;
                return false;
            }
            result = _first.Data;
            return true;
        }



        #endregion

        #region Insert logic
        public void InsertAtFront(T item)
        {
            Node newItem = new() { Data = item };
            if (_first is null)
            {
                _first = _last = newItem;
                N++;
                return;
            }
            Node oldFirst = _first;
            newItem.Next = oldFirst;
            oldFirst.Prev = newItem;
            _first = newItem;
            N++;
        }
        public void InsertAtBack(T item)
        {
            Node newItem = new() { Data = item };
            if (_first is null)
            {
                _first = _last = newItem;
                N++;
                return;
            }
            Node oldlast = _last;
            oldlast.Next = newItem;
            newItem.Prev = oldlast;
            _last = newItem;
            N++;
        }

        #endregion

        #region Delete logic
        /// <summary>
        /// Removes the item next in the queue / collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exception when either empty of the collection is being iterated</exception>
        public T DeleteAtFront()
        {
            Check();
            return DeleteFrontUtil();
        }

        private T DeleteFrontUtil()
        {
            T result = _first.Data;
            _first = _first.Next;
            N--;
            if (_first is null) _last = null;
            else _first.Prev = null;
            return result;
        }
        /// <summary>
        /// if delete was successful, returns true and sets result to value at front, otherwise result is set to default(T) and
        /// returns false
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryDeleteAtFront(out T result)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = default;
                return false;
            }
            result = DeleteFrontUtil();
            return true;
        }

        /// <summary>
        /// if delete was successful, returns true and sets result to value at front, otherwise result is set to the
        /// passed value of defaultIfFail and returns false
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool TryDeleteAtFront(out T result, T defaultIfFail)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = defaultIfFail;
                return false;
            }
            result = DeleteFrontUtil();
            return true;
        }

        /// <summary>
        /// Removes the last item in the queue / collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exception when either empty of the collection is being iterated</exception>
        public T DeleteAtBack()
        {
            Check();
            return DeleteAtBackUtil();
        }

        /// <summary>
        /// Removes the last item in the queue / collection and returns true, 
        /// otherwise returns false and result is set default(t)
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryDeleteAtBack(out T result)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = default;
                return false;
            }
            result = DeleteAtBackUtil();
            return true;

        }
        private T DeleteAtBackUtil()
        {
            T result = _last.Data;
            _last = _last.Prev;
            N--;
            if (_last is null) _first = null;
            else _last.Next = null;
            return result;
        }

        private void Check()
        {
            if (IsEnumerating) throw new EmptyCollectionException(ExceptionConstants.DeleteWhileEnumerating, "Cannot delete new item while enumerating");
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "Collection is empty");

        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LinkedListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        private record Node
        {
            public T Data;
            public Node Next;
            public Node Prev;
        }

        #region Enumeration
        private class LinkedListEnumerator : IEnumerator<T>
        {
            public T Current
            {
                get
                {
                    if (current is null)
                        throw new CollectionException(ExceptionConstants.TryToReadWhenEmpty, "Past end of collection");
                    return current.Data;
                }
            }
            private Node current = null;
            private DoublyLinkedList<T> _list;
            private bool firstCall = true;
            public LinkedListEnumerator(DoublyLinkedList<T> list)
            {
                _list = list;
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                current = null;
                _list = null;
            }

            public bool MoveNext()
            {
                if (firstCall)
                {
                    current = _list._first;
                    firstCall = false;
                    return current is not null;
                }
                if (current is not null)
                {
                    current = current.Next;
                    return current is not null;
                }
                return false;
            }

            public void Reset()
            {
                current = null;
                firstCall = true;
            }
        }

        #endregion

    }
}
