using System;
using System.Collections.Generic;

namespace Chitataunga.Algorithms.Searching
{
    public class BinarySearchTree<TKeyType, TValueType> // : IEnumerable<TValueType>

        where TKeyType : IComparable<TKeyType>
    {
        private Node root;

        /// <summary>
        /// Comparison time = 1 + Depth of the node
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKeyType key, TValueType value)
        {
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
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>

        public TValueType Get(TKeyType key)
        {
            Node x = root;
            while (x is not null)
            {
                int comp = key.CompareTo(x.Key);
                if (comp < 0) x = x.Left;
                else if (comp > 0) x = x.Right;
                else return x.Value;
            }
            return default;
        }

        #region Floor, Ceiling and Rank of a key

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

        private int Size(Node x) => x?.Count ?? 0;

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
        public void Delete(TKeyType key) => root = Delete(root, key);
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

        private Node MinNode(Node x)
        {
            if (x.Left is null) return x;
            return MinNode(x.Left);
        }
        #endregion

        private class Node
        {
            public TKeyType Key;
            public TValueType Value;
            public Node Left;
            public Node Right;
            public int Count;

            public Node(TKeyType key, TValueType value)
            {
                Key = key;
                Value = value;
            }

        }
    }
}
