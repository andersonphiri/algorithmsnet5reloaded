using Chitataunga.Algorithms.Common.Constants;
using Chitataunga.Algorithms.Common.DataStructures;
using Chitataunga.Algorithms.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chitataunga.Algorithms.Searching
{
    /// <summary>
    /// Summary Time complexities:
    /// Search - worst case: O(N)
    /// Search Hit - average case: 1.39Log(N)
    /// Insert - worst case: O(N)
    /// Insert - average case (after N random insert): 1.39Log(N)
    /// Efficiently Support Ordered ops: YES
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class BinarySearchTree<TKeyType, TValueType> // : IEnumerable<TValueType>

        where TKeyType : IComparable<TKeyType>
    {
        private Node root;

        /// <summary>
        /// Comparison time = 1 + Depth of the node
        /// running time O(N)
        /// average case (after N random insert): 1.39Log(N)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKeyType key, TValueType value)
        {
            if (key is null) throw new CollectionException(ExceptionConstants.KeyIsNull, "key cannot be null");
            if (value is null) throw new CollectionException(ExceptionConstants.ValueIsNull, "value cannot be null");

            root = PutUtil(root, key, value);
        }

        private Node PutUtil(Node x, TKeyType key, TValueType value)
        {
            if (x is null) return new Node(key, value);
            int comp = key.CompareTo(x.Key);
            if (comp < 0) x.Left = PutUtil(x.Left, key, value); // go left
            else if (comp > 0) PutUtil(x.Right, key, value); // go right
            else x.Value = value; // update
            x.Count = 1 + Size(x.Left) + Size(x.Right);
            return x;
        }

        /// <summary>
        /// Running time = 1 + Depth of the node
        /// Search - worst case: O(N)
        /// Search Hit - average case: 1.39Log(N)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public TValueType Get(TKeyType key)
        {
            if (key is null) throw new CollectionException(ExceptionConstants.KeyIsNull, "key cannot be null");

            Node x = root;
            while (x is not null)
            {
                int comp = key.CompareTo(x.Key);
                if (comp < 0) x = x.Left;
                else if (comp > 0) x = x.Right;
                else return x.Value;
            }
            return default ;
        }
        /// <summary>
        /// Running time = 1 + Depth of the node
        /// Search - worst case: O(N)
        /// Search Hit - average case: 1.39Log(N)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValueType GetAlt(TKeyType key)
        {
            if (key is null) throw new CollectionException(ExceptionConstants.KeyIsNull, "key cannot be null");

            return GetAltUtil(root, key);
        }

        private TValueType GetAltUtil(Node node, TKeyType key)
        {
            if (node is null) return default;
            int comp = key.CompareTo(node.Key);
            if (comp < 0) return GetAltUtil(node.Left, key);
            else if (comp > 0) return GetAltUtil(node.Right, key);
            else return node.Value;
        }

        #region Floor, Ceiling, Select and Rank of a key
        /// <summary>
        /// return the key of rank k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public TKeyType Select(int k)
        {
            return Select(root, k).Key;
        }
        /// <summary>
        /// returns key of node containing rank k
        /// pp 422
        /// </summary>
        /// <param name="x"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private Node Select(Node x, int k)
        {
            if (x is null) return null;
            int t = Size(x.Left);
            if (t > k) return Select(x.Left, k);
            else if (t < k) return Select(x.Right, k - t - 1);
            else return x;
        }
        public TKeyType Floor(TKeyType key)
        {
            Node x = Floor(root, key);
            if (x is null) return default;
            return x.Key;
        }
        private Node Floor(Node x, TKeyType key)
        {
            if (x is null) return null;
            int comp = key.CompareTo(x.Key);
            if (comp == 0) return x;
            if (comp < 0) return Floor(x.Left, key);
            // then search to the right in the left side
            Node t = Floor(x.Right, key);
            if (t != null) return t;
            return x;
        }

        /// <summary>
        /// The key just greater than key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TKeyType Ceiling(TKeyType key)
        {
            if (key == null) throw new ArgumentNullException("argument to Ceiling() is null"); 
            if (IsEmpty()) throw new InvalidOperationException("called Ceiling() with empty symbol table");
            Node x = Ceiling(root, key);
            if (x == null) throw new KeyNotFoundException("ceiling key does not exist");
            else return x.Key;
        }

        private Node Ceiling(Node x, TKeyType key)
        {
            if (x == null) return null;
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0) return x;
            if (cmp < 0)
            {
                Node t = Ceiling(x.Left, key);
                if (t != null) return t;
                return x;
            }
            return Ceiling(x.Right, key);
        }
        /// <summary>
        /// How many keys < k
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int Rank(TKeyType key)
        {
            return Rank(key, root);
        }
        private int Rank(TKeyType key, Node x)
        {
            if (x is null) return 0;
            int comp = key.CompareTo(x.Key);
            if (comp < 0) return Rank(key, x.Left);
            else if (comp > 0) return 1 + Size(x.Left) + Rank(key, x.Right);
            else
            {
                return Size(x.Left);
            }
        }

        #endregion

        #region Size

        public int Count
        {
            get =>  Size(root);
        }

        private int Size(Node x) => x is null ? 0 : x.Count;

        #endregion

        #region Deleting

        public void DeleteMinimum()
        {
            root = DeleteMinimum(root);
        }
        public void DeleteMaximum()
        {
            root = DeleteMaximum(root);
        }
        /// <summary>
        /// uses hibbard implementation
        /// </summary>
        /// <param name="key"></param>
        public void Delete(TKeyType key) {
            if (key is null) throw new CollectionException(ExceptionConstants.KeyIsNull, "key cannot be null");
            root = Delete(root, key);
        }
        
        private Node DeleteMinimum(Node x)
        {
            if (x.Left is null) return x.Right;
            x.Left = DeleteMinimum(x.Left);
            x.Count = 1 + Size(x.Left) + Size(x.Right);
            return x;
        }
        private Node DeleteMaximum(Node x)
        {
            if (x.Right is null) return x.Left;
            x.Left = DeleteMaximum(x.Left);
            x.Count = 1 + Size(x.Left) + Size(x.Right);
            return x;
        }
        /// <summary>
        /// Uses Hibbard implementation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private Node Delete(Node x, TKeyType key)
        {
            if (x is null) return null;
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0) x.Left = Delete(x.Left, key);
            else if (cmp > 0) x.Right = Delete(x.Right, key);
            else
            {
                if (x.Right is null) return x.Left;
                if (x.Left is null) return x.Right;
                Node t = x;
                x = MinNode(t.Right);
                x.Right = DeleteMinimum(t.Right);
                x.Left = t.Left;
            }

            x.Count = Size(x.Left) + Size(x.Right) + 1;
            return x;
        }



        public bool IsEmpty() => Count == 0;

        public TKeyType MinKey
        {
            get
            {
                if (IsEmpty()) throw new InvalidOperationException("Tree is empty");
                return MinNode(root).Key;
            }
        }
        public TKeyType MaxKey
        {
            get
            {
                if (IsEmpty()) throw new InvalidOperationException("Tree is empty");
                return MaxNode(root).Key;
            }
        }
        public TKeyType Max()
        {
            if (IsEmpty()) throw new InvalidOperationException("Tree is empty");
            return MaxNode(root).Key;
        }
        public TKeyType Min()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode,  "Tree is empty");
            return MinNode(root).Key;
        }

        private Node MinNode(Node x)
        {
            if (x.Left is null) return x;
            return MinNode(x.Left);
        }
        private Node MaxNode(Node x)
        {
            if (x.Right is null) return x;
            return MinNode(x.Right);
        }
        #endregion

        #region Range Queries, Traversal and Printing
        public IEnumerable<TKeyType> Keys(TKeyType lo, TKeyType hi, Func<IDoublyLinkedQueue<TKeyType>> queueFunc = null)
        {
            if (lo is null) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "lo cannot be null");
            if (hi is null) throw new EmptyCollectionException(ExceptionConstants.EmptyCollectionStatusCode, "hi cannot be null");
            if (IsEmpty()) return Enumerable.Empty<TKeyType>();
            IDoublyLinkedQueue<TKeyType> queue = queueFunc is null ? new DoublyLinkedQueue<TKeyType>() : queueFunc();
            Keys(root, queue, lo, hi);
            return queue;
        }
        public IEnumerable<TKeyType> Keys(Func<IDoublyLinkedQueue<TKeyType>> queueFunc = null)
        {
            if (IsEmpty()) return Enumerable.Empty<TKeyType>();
            IDoublyLinkedQueue<TKeyType> queue = queueFunc is null ? new DoublyLinkedQueue<TKeyType>() : queueFunc();
            Keys(root, queue, MinKey, MaxKey);
            return queue;
        }
        private void Keys(Node x, IDoublyLinkedQueue<TKeyType> queue, TKeyType lo, TKeyType hi)
        {
            if (x is null) return;
            int cmplo = lo.CompareTo(x.Key);
            int cmphi = hi.CompareTo(x.Key);
            if (cmplo < 0) Keys(x.Left, queue, lo, hi);
            if (cmplo <= 0 && cmphi >= 0) queue.Enqueue(x.Key);
            if (cmphi > 0) Keys(x.Right, queue, lo, hi);
        }
        private void Print(Node x)
        {
            if (x is null) return;
            Print(x.Left);
            System.Console.Write($"{x.Value} \t");
            Print(x.Right);
        }
        #endregion

        private class Node
        {
            public TKeyType Key;
            public TValueType Value;
            public Node Left;
            public Node Right;
            /// <summary>
            /// subtree count
            /// </summary>
            public int Count;

            public Node(TKeyType key, TValueType value)
            {
                Key = key;
                Value = value;
                Count = 1;
            }
            public Node(TKeyType key, TValueType value, int count)
            {
                Key = key;
                Value = value;
                Count = count;
            }

        }
    }
}
