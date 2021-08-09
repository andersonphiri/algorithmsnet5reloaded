using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Searching
{
    /// <summary>
    /// Uses .equals for comparison
    /// inserting N distinct keys into an initially empty linked-list symbol table 
    /// uses ~N^2/2 compares.
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class SequentialSearchST<TKeyType, TValueType>
    {
        private Node first;

        #region Insertion
        public void Put(TKeyType key, TValueType @value)
        {
            for (Node x = first; x is not null; x = x.Next)
            {
                if (x.Key.Equals(key)) { x.Value = value; };
                return;
            }
            first = new Node(key, value, first);
        }

        #endregion

        #region Reading from collection
        public TValueType Get(TKeyType key)
        {
            for (Node x = first; x is not null; x = x.Next)
            {
                if (x.Key.Equals(key)) { return x.Value; }
            }
            return default;
        }
        #endregion

        private record Node
        {
            public TKeyType Key;
            public TValueType Value;
            public Node Next;
            public Node(TKeyType key, TValueType @value, Node next)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }
    }
}
