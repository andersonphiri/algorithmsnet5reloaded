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
    /// Insertion can be either front or back, however, deletion is only done at the front
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SinglyLinkedList<T> : IEnumerable<T>
    {

        private Node _first;
        private Node _last;
        private ulong N;
        internal BooleanThreadSafe IsEnumerating { get; set; }

        public SinglyLinkedList()
        {
            _first = null;
            _last = null;
            N = 0;
            IsEnumerating = new();

        }
        private void CheckBeforeMutate()
        {
            if (IsEnumerating) throw new EmptyCollectionException(ExceptionConstants.DeleteWhileEnumerating, "Cannot delete new item while enumerating");
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "Collection is empty");

        }


        public bool IsEmpty() => N == 0;

        /// <summary>
        /// deletes the element at the front and returns its value
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exceptions if collection is empty</exception>
        public T DeleteAtStart()
        {
            CheckBeforeMutate();
            return DeleteStartUtil();
        }

        private T DeleteStartUtil()
        {
            T data = _first.Data;
            _first = _first.Next;
            if (_first is null) _last = null;
            N--;
            return data;
        }
        /// <summary>
        /// return value at the back / last item in the collection
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exceptions if collection is empty</exception>
        public T PeekBack()
        {
            CheckBeforeMutate();
            return _last.Data;
        }
        /// <summary>
        /// ///Returns the value at the back of the collection
        /// if collection is empty, will result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeekBack(out T result)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = default;
                return false;
            }
            result = _last.Data;
            return true;
        }

        public void Clear()
        {
            //TODO: handle clear properly
            N = 0;
            _first = null;
            _last = null;
        }

        /// <summary>
        /// return value at the front / first items in the collection, without deleting it
        /// </summary>
        /// <returns></returns>
        /// <exception cref="EmptyCollectionException">throws exceptions if collection is empty</exception>
        public T PeekFront()
        {
            CheckBeforeMutate();
            return _first.Data;
        }

        /// <summary>
        /// Returns the value at the front of the collection, without deleting the value
        /// if collection is empty, result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryPeekFront(out T result)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = default;
                return false;
            }
            result = _first.Data;
            return true;
        }
        /// <summary>
        /// ///deletes the value at the front of the collection and returns it
        /// if collection is empty, will result is set to default of T and return false otherwise returns true
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>

        public bool TryDeleteAtStart(out T result)
        {
            if (IsEmpty() || IsEnumerating)
            {
                result = default;
                return false;
            }
            result = DeleteStartUtil();
            return true;
        }
        public void InsertAtBeginning(T data)
        {
            Node newItem = new(data);
            if (_first is null)
            {
                _first = _last = newItem;
                N++;
                return;
            }
            Node oldfirst = _first;
            newItem.Next = oldfirst;
            _first = newItem;
            N++;
        }
        public void InsertAtEnd(T data)
        {
            Node newItem = new(data);
            if (_first is null)
            {
                _first = _last = newItem;
                N++;
                return;
            }

            Node oldLast = _last;
            oldLast.Next = newItem;
            _last = newItem;
            N++;

        }

        private record Node
        {
            public Node()
            {

            }
            public Node(T data)
            {
                Data = data;
            }
            public T Data;
            public Node Next;

        }


        private class SinglyLinkedListEnumerator : IEnumerator<T>
        {
            private readonly SinglyLinkedList<T> _list;
            private bool firstCall = true;
            public SinglyLinkedListEnumerator(SinglyLinkedList<T> list)
            {
                _list = list;
            }

            private Node current = null;
            public bool MoveNext()
            {
                if (firstCall)
                {
                    current = _list._first;
                    firstCall = false;
                    return current != null;
                }

                if (current != null)
                {
                    current = current.Next;
                    return current != null;
                }

                return false;
            }

            public void Reset()
            {
                current = null;
                firstCall = true;
            }

            public T Current
            {
                get
                {
                    if (current is null)
                        throw new InvalidOperationException("This is past the end of the collection");
                    return current.Data;
                }
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {

            }
        }

        // check internal invariants
        private bool Check()
        {

            // check a few properties of instance variable 'first'
            if (N < 0)
            {
                return false;
            }
            if (N == 0)
            {
                if (_first != null) return false;
            }
            else if (N == 1)
            {
                if (_first == null) return false;
                if (_first.Next != null) return false;
            }
            else
            {
                if (_first == null) return false;
                if (_first.Next == null) return false;
            }

            // check internal consistency of instance variable N
            ulong numberOfNodes = 0;
            for (Node x = _first; x != null && numberOfNodes <= N; x = x.Next)
            {
                numberOfNodes++;
            }
            if (numberOfNodes != N) return false;

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SinglyLinkedListEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
