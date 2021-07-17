using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Common.DataStructures
{
    public class SinglyLinkedList<TType> : IEnumerable<TType>
    {

        private Node first;
        private Node last;
        private int N = 0;

        public SinglyLinkedList()
        {
            first = new();
            last = new Node() {Next = null};
            first.Next = last;

        }

        public bool IsEmpty() => N == 0;
        public TType DeleteAtStart()
        {
            Node oldLast = first;
            first = first.Next;
            N--;
            return oldLast.Data;
        }
        public void InsertAtBeginning(TType data)
        {
            Node oldfirst = first;
            first = new Node(data) {Next = oldfirst};
            N++;
            //check();
        }
        public void InsertAtEnd(TType data)
        {
            Node oldLast = last;
            last = new(data) {Next = null};
            if (IsEmpty())
            {
                first = last;
            }
            else
            {
                oldLast.Next = last;
            }
           

            N++;
            //check();

        }

        private class  Node
        {
            public Node()
            {
                
            }
            public Node(TType data)
            {
                Data = data;
            }
            public TType Data { get; set; }
            public Node Next { get; set; }
        }

        private class SinglyLinkedListEnumerator : IEnumerator<TType>
        {
            private readonly SinglyLinkedList<TType> _list;
            private bool firstCall = true;
            public SinglyLinkedListEnumerator(SinglyLinkedList<TType> list)
            {
                _list = list;
            }

            private Node current = null;
            public bool MoveNext()
            {
                if (firstCall)
                {
                    current = _list.first;
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

            public TType Current
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
        private bool check()
        {

            // check a few properties of instance variable 'first'
            if (N < 0)
            {
                return false;
            }
            if (N == 0)
            {
                if (first != null) return false;
            }
            else if (N == 1)
            {
                if (first == null) return false;
                if (first.Next != null) return false;
            }
            else
            {
                if (first == null) return false;
                if (first.Next == null) return false;
            }

            // check internal consistency of instance variable N
            int numberOfNodes = 0;
            for (Node x = first; x != null && numberOfNodes <= N; x = x.Next)
            {
                numberOfNodes++;
            }
            if (numberOfNodes != N) return false;

            return true;
        }

        public IEnumerator<TType> GetEnumerator()
        {
            return new SinglyLinkedListEnumerator( this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
