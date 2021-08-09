using Chitataunga.Algorithms.Common.Constants;
using Chitataunga.Algorithms.Common.DataStructures;
using Chitataunga.Algorithms.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Searching
{
    /// <summary>
    /// null links are classified as black / false
    /// if parent link to child is red, then child will be red
    /// Assume a LLRB - Left Leaning Red Black Tree
    /// https://algs4.cs.princeton.edu/code/edu/princeton/cs/algs4/RedBlackBST.java.html
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class RedBlackTree<TKeyType, TValueType>
        where TKeyType : IComparable<TKeyType>
    {
        public const bool RED = true;
        public const bool BLACK = false;
        private Node root;

        #region Size

        public int Count
        {
            get => Size(root);
        }
        public int Size() => Size(root);

        private static int Size(Node x) => x is null ? 0 : x.Count;

        private bool IsEmpty() => root is null;
        #endregion


        #region Search

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

            return Get(root, key);
        }
        private static TValueType Get(Node x, TKeyType key)
        {
            while (x != null)
            {
                int cmp = key.CompareTo(x.Key);
                if (cmp < 0) x = x.Left;
                else if (cmp > 0) x = x.Right;
                else return x.Value;
            }
            return default;
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

        /// <summary>
        /// Does this symbol table contain the given key?</summary>
        /// <param name="key">key the key</param>
        /// <returns><c>true</c> if this symbol table contains <c>key</c> and
        ///    <c>false</c> otherwise</returns>
        /// <exception cref="ArgumentNullException">if <c>key</c> is <c>null</c></exception>
        ///
        public bool Contains(TKeyType key)
        {
            return Get(key) is not null;
        }

        #endregion



        #region Helper Functions
        /// <summary>
        /// flip color to split a temporary 4-node
        /// </summary>
        /// <param name="h"></param>
        private void FlipColors(Node h)
        {
            h.Color = !h.Color;
            h.Left.Color = !h.Left.Color;
            h.Right.Color = !h.Right.Color;
        }

        /// <summary>
        /// orient a temporarily right leaning node to left
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node RotateLeft(Node h)
        {
            Node x = h.Right;
            h.Right = x.Left;
            x.Left = h;
            x.Color = x.Left.Color;
            x.Left.Color = RED;
            x.Count = h.Count;
            h.Count = Size(h.Left) + Size(h.Right) + 1;
            return x;
        }

        /// <summary>
        /// orient a left leaning node to temporarily lean right
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private Node RotateRight(Node h)
        {
            Node x = h.Left;
            h.Left = x.Right;
            x.Right = h;
            x.Color = h.Color;
            h.Color = RED;
            x.Right.Color = RED;
            x.Count = h.Count;
            h.Count = Size(h.Left) + Size(h.Right) + 1;
            return x;
        }

        /// <summary>
        /// null links are black
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool IsRed(Node node)
        {
            if (node is null) return false;
            return node.Color == RED;
        }

        // restore red-black tree invariant
        private void Balance(ref Node h)
        {
            // check for 4-node and other checks to maintain invariant
            // the node is temp right leaning Red Child
            if (IsRed(h.Right) && !IsRed(h.Left)) { h = RotateLeft(h); }
            // if there are two REDs in a left row / in a row, then do a temporary rotate right
            if (IsRed(h.Left) && IsRed(h.Left.Left)) { h = RotateRight(h); }

            // finally, check if we have both left child and right child in RED, if so, then flip colors
            // this mean we have temporary 4-node
            if (IsRed(h.Left) && IsRed(h.Right)) { FlipColors(h); };
            h.Count = 1 + Size(h.Left) + Size(h.Right);
            return;
        }

        private int Height(Node x)
        {
            if (x is null) return -1;
            return 1 + Math.Max(Height(x.Left), Height(x.Right));
        }
        /// <summary>
        /// Returns the height of the BST (for debugging).
        /// </summary>
        /// <returns>the height of the BST (a 1-node tree has height 0)</returns>
        public int Height() => Height(root);



        // Assuming that h is red and both h.left and h.left.left
        // are black, make h.left or one of its children red.
        private Node MoveRedLeft(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Right.Left))
            {
                h.Right = RotateRight(h.Right);
                h = RotateLeft(h);
                FlipColors(h);
            }
            return h;
        }

        // Assuming that h is red and both h.right and h.right.left
        // are black, make h.right or one of its children red.
        private Node MoveRedRight(Node h)
        {
            FlipColors(h);
            if (IsRed(h.Left.Left))
            {
                h = RotateRight(h);
                FlipColors(h);
            }
            return h;
        }

        /// <summary>
        ///  Returns the smallest key in the symbol table.
        /// </summary>
        /// <returns></returns>
        public TKeyType Min()
        {
            if (IsEmpty()) throw new EmptyCollectionException("calls Min() with empty symbol table");
            return Min(root).Key;
        }

        // the smallest key in subtree rooted at x; null if no such key
        private Node Min(Node x)
        {
            // assert x != null;
            if (x.Left == null) return x;
            else return Min(x.Left);
        }


        /// <summary>
        /// Returns the largest key in the symbol table.
        /// return the largest key in the symbol table
        /// </summary>
        /// <returns></returns>
        public TKeyType Max()
        {
            if (IsEmpty()) throw new EmptyCollectionException("calls Max() with empty symbol table");
            return Max(root).Key;
        }

        // the largest key in the subtree rooted at x; null if no such key
        private Node Max(Node x)
        {
            // assert x != null;
            if (x.Right == null) return x;
            else return Max(x.Right);
        }

        #endregion

        #region Insertion
        /// <summary>
        /// Inserts the specified key-value pair into the symbol table, overwriting the old 
        /// value with the new value if the symbol table already contains the specified key.
        /// Deletes the specified key (and its associated value) from this symbol table
        /// if the specified key is null
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKeyType key, TValueType @value)
        {
            if (key is null) throw new CollectionException(ExceptionConstants.KeyIsNull, "key cannot be null");
            if (value is null) { Delete(key); return; }
            root = Put(root, key, value);
            root.Color = BLACK;
        }
        private Node Put(Node h, TKeyType key, TValueType @value)
        {
            if (h is null) return new Node { Key = key, Value = @value, Color = RED };
            int comp = key.CompareTo(h.Key);
            if (comp < 0) { h.Left = Put(h.Left, key, @value); }
            else if (comp > 0) { h.Right = Put(h.Right, key, @value); }
            else { h.Value = @value; }
            Balance(ref h);
            return h;
        }

        /// <summary>
        /// Indexer wrapping <c>Get</c> and <c>Put</c> for convenient syntax
        /// </summary>
        /// <param name="key">key the key </param>
        /// <returns>value associated with the key</returns>
        /// <exception cref="NullReferenceException">null reference being used for value type</exception>
        public TValueType this[TKeyType key]
        {
            get
            {
                TValueType value = Get(key);
                if (value is null)
                {
                    if (default(TValueType) == null) return value;
                    else throw new NullReferenceException("null reference being used for value type");
                }
                return value;
            }

            set { Put(key, value); }
        }
        #endregion

        #region Delete operations

        public void Delete(TKeyType key)
        {
            if (key is null) throw new CollectionException(ExceptionConstants.TryToDeleteWhenEmpty, "The tree is empty");
            if (!Contains(key)) return;
            // if both children of root are black, set root to red
            if (!IsRed(root.Left) && !IsRed(root.Right))
                root.Color = RED;

            root = Delete(root, key);
            if (!IsEmpty()) root.Color = BLACK;
            // assert check();
        }
        private Node Delete(Node h, TKeyType key)
        {
            if (key.CompareTo(h.Key) < 0)
            {
                if (!IsRed(h.Left) && !IsRed(h.Left.Left))
                    h = MoveRedLeft(h);
                h.Left = Delete(h.Left, key);
            }
            else
            {
                if (IsRed(h.Left))
                    h = RotateRight(h);
                if (key.CompareTo(h.Key) == 0 && (h.Right is null))
                    return null;
                if (!IsRed(h.Right) && !IsRed(h.Right.Left))
                    h = MoveRedRight(h);
                if (key.CompareTo(h.Key) == 0)
                {
                    Node x = Min(h.Right);
                    h.Key = x.Key;
                    h.Value = x.Value;
                    // h.val = get(h.right, min(h.right).key);
                    // h.key = min(h.right).key;
                    h.Right = DeleteMinimum(h.Right);
                }
                else h.Right = Delete(h.Right, key);
            }
            Balance(ref h);
            return h;
        }

        // delete the key-value pair with the minimum key rooted at h
        private Node DeleteMinimum(Node h)
        {
            if (h.Left is null) return null;

            if (!IsRed(h.Left) && !IsRed(h.Left.Left)) { h = MoveRedLeft(h); }

            h.Left = DeleteMinimum(h.Left);
            Balance(ref h);
            return h;
        }
        public void DeleteMinimum()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToDeleteWhenEmpty, "Tree is empty");
            // if both children of root are black, set root to red
            if (!IsRed(root.Left) && !IsRed(root.Right)) { root.Color = RED; }
            root = DeleteMinimum(root);
            if (!IsEmpty()) root.Color = BLACK;
        }

        // delete the key-value pair with the maximum key rooted at h
        private Node DeleteMaximum(Node h)
        {
            if (IsRed(h.Left)) { h = RotateRight(h); }
            if (h.Right == null)
            {
                return null;
            }
            if (!IsRed(h.Right) && !IsRed(h.Right.Left))
            {
                h = MoveRedRight(h);
            }
            h.Right = DeleteMaximum(h.Right);
            Balance(ref h);
            return h;
        }
        public void DeleteMaximum()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToDeleteWhenEmpty, "Tree is empty");

            // if both children of root are black, set root to red
            if (!IsRed(root.Left) && !IsRed(root.Right))
            {
                root.Color = RED;
            }
            root = DeleteMaximum(root);
            if (!IsEmpty()) root.Color = BLACK;
            // assert check();
        }


        #endregion

        #region Min Max Logic
        /// <summary>
        /// return the least key in the symbol table
        /// </summary>
        /// <returns>the smallest key in the symbol table</returns>
        /// <exception cref="EmptyCollectionException">throws exception when empty</exception>
        public TKeyType Minimum()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToReadWhenEmpty, "Tree is empty");
            return Minimum(root).Key;
        }
        private Node Minimum(Node h)
        {
            if (h.Left is null) return h;
            else return Minimum(h.Left);

        }
        private Node Maximum(Node h)
        {
            if (h.Right is null) return h;
            else return Maximum(h.Left);

        }

        /// <summary>
        /// return the largest key in the symbol table
        /// </summary>
        /// <returns>the largest key in the symbol table</returns>
        /// <exception cref="EmptyCollectionException">throws exception when empty</exception>
        public TKeyType Maximum()
        {
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToReadWhenEmpty, "Tree is empty");
            return Maximum(root).Key;

        }
        #endregion

        #region Floor, Ceiling, Rank
        // the largest key in the subtree rooted at x less than or equal to the given key
        private Node Floor(Node x, TKeyType key)
        {
            if (x == null) return null;
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0) return x;
            if (cmp < 0) return Floor(x.Left, key);
            Node t = Floor(x.Right, key);
            if (t != null) return t;
            else return x;
        }
        /// <summary>
        /// Returns the largest key in the symbol table less than or equal to key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the largest key in the symbol table less than or equal to key</returns>
        /// <exception cref="EmptyCollectionException">throws exception when empty</exception>
        /// <exception cref="ArgumentNullException">throws exception when key passed is null </exception>
        /// <exception cref="NoSuchElementException">throws exception when key passed is too small</exception>
        public TKeyType Floor(TKeyType key)
        {
            if (key == null) throw new ArgumentNullException("key argumnent must not be a null value");
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToReadWhenEmpty, "Tree is empty");
            Node x = Floor(root, key);
            if (x == null) throw new NoSuchElementException(ExceptionConstants.NoSuchKey, "key is too small");
            else return x.Key;
        }

        // the smallest key in the subtree rooted at x greater than or equal to the given key
        private Node Ceiling(Node x, TKeyType key)
        {
            if (x is null) return null;
            int cmp = key.CompareTo(x.Key);
            if (cmp == 0) return x;
            if (cmp > 0) return Ceiling(x.Right, key);
            Node t = Ceiling(x.Left, key);
            if (t != null) return t;
            else return x;
        }

        /// <summary>
        /// Returns the smallest key in the symbol table greater than or equal to key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the smallest key in the symbol table greater than or equal to key</returns>
        /// /// <exception cref="EmptyCollectionException">throws exception when empty</exception>
        /// <exception cref="ArgumentNullException">throws exception when key passed is null </exception>
        /// <exception cref="NoSuchElementException">throws exception when key passed is too small</exception>
        public TKeyType Ceiling(TKeyType key)
        {
            if (key is null) throw new ArgumentNullException("key argumnent must not be a null value");
            if (IsEmpty()) throw new EmptyCollectionException(ExceptionConstants.TryToReadWhenEmpty, "Tree is empty");
            Node x = Ceiling(root, key);
            if (x == null) throw new NoSuchElementException(ExceptionConstants.NoSuchKey, "argument passed is too small");
            else return x.Key;
        }

        // Return key in BST rooted at x of given rank.
        // Precondition: rank is in legal range.
        private TKeyType Select(Node x, int rank)
        {
            if (x == null) return default;
            int leftSize = Size(x.Left);
            if (leftSize > rank) return Select(x.Left, rank);
            else if (leftSize < rank) return Select(x.Right, rank - leftSize - 1);
            else return x.Key;
        }

        /// <summary>
        /// Return the key in the symbol table of a given rank.
        /// This key has the property that there are (rank)
        /// keys in
        /// the symbol table that are smaller.In other words, this key is the
        /// (rank + 1)st smallest key in the symbol table.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns>the key in the symbol table of given rank}</returns>

        public TKeyType Select(int rank)
        {
            if (rank < 0 || rank >= Size())
            {
                throw new CollectionException(ExceptionConstants.RankOutOfRange, "argument rank is out of range: " + rank);
            }
            return Select(root, rank);
        }

        // number of keys less than key in the subtree rooted at x
        private int Rank(TKeyType key, Node x)
        {
            if (x == null) return 0;
            int cmp = key.CompareTo(x.Key);
            if (cmp < 0) return Rank(key, x.Left);
            else if (cmp > 0) return 1 + Size(x.Left) + Rank(key, x.Right);
            else return Size(x.Left);
        }

        /// <summary>
        /// Return the number of keys in the symbol table strictly less than key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">if key is null</exception>
        public int Rank(TKeyType key)
        {
            if (key is null) throw new ArgumentNullException("argument key must not be null");
            return Rank(key, root);
        }

        #endregion

        #region Count Range and Search
        // add the keys between lo and hi in the subtree rooted at x
        // to the queue
        private void Keys(Node x, IDoublyLinkedQueue<TKeyType> queue, TKeyType lo, TKeyType hi)
        {
            if (x == null) return;
            int cmplo = lo.CompareTo(x.Key);
            int cmphi = hi.CompareTo(x.Key);
            if (cmplo < 0) Keys(x.Left, queue, lo, hi);
            if (cmplo <= 0 && cmphi >= 0) queue.Enqueue(x.Key);
            if (cmphi > 0) Keys(x.Right, queue, lo, hi);
        }

        /// <summary>
        /// Returns all keys in the given range as an <code>IEnumerable<typeparamref name="TKeyType"/></code>
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns>all keys in the symbol table between <code>lo</code>(inclusive) and <code>hi</code>(inclusive) as an <code>IEnumerable<typeparamref name="TKeyType"/></code></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<TKeyType> Keys(TKeyType lo, TKeyType hi)
        {
            if (lo == null) throw new ArgumentNullException(nameof(lo), "first argument to keys() is null");
            if (hi == null) throw new ArgumentNullException(nameof(hi), "second argument to keys() is null");

            IDoublyLinkedQueue<TKeyType> queue = new DoublyLinkedQueue<TKeyType>();
            // if (isEmpty() || lo.compareTo(hi) > 0) return queue;
            Keys(root, queue, lo, hi);
            return queue;
        }

        /// <summary>
        /// Returns all keys in the symbol table as an an <code>IEnumerable<typeparamref name="TKeyType"/></code>
        /// To iterate over all of the keys in the symbol table named <code>st</code>,
        /// you may use the foreach notation: <code>foreach (TKeyType key in st.Keys()) { DoStuff(key); }</code>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TKeyType> Keys()
        {
            if (IsEmpty()) return Enumerable.Empty<TKeyType>();
            return Keys(Min(), Max());
        }

        /// <summary>
        /// Returns all key : Value pairs in the symbol table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<TKeyType, TValueType>> GetKeyValuePairs()
        {
            if (IsEmpty()) return Enumerable.Empty<KeyValuePair<TKeyType, TValueType>>();
            IDoublyLinkedQueue <KeyValuePair<TKeyType, TValueType>> queue = new DoublyLinkedQueue<KeyValuePair<TKeyType, TValueType>>();
            KeyValues(root, queue, Minimum(), Maximum());
            return queue;
        }

        /// <summary>
        /// Returns all key : Value pairs in the symbol table in the given range
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumerable<KeyValuePair<TKeyType, TValueType>> GetKeyValuePairs(TKeyType lo, TKeyType hi)
        {
            if (IsEmpty()) return Enumerable.Empty<KeyValuePair<TKeyType, TValueType>>();
            if (lo == null) throw new ArgumentNullException(nameof(lo), "argument lo must not be null");
            if (hi == null) throw new ArgumentNullException(nameof(hi), "argument hi must not be null");

            IDoublyLinkedQueue<KeyValuePair<TKeyType, TValueType>> queue = new DoublyLinkedQueue<KeyValuePair<TKeyType, TValueType>>();
            KeyValues(root, queue, lo, hi);
            return queue;
        }
        // add the keys and values between lo and hi in the subtree rooted at x
        // to the queue
        private void KeyValues(Node x, IDoublyLinkedQueue<KeyValuePair<TKeyType, TValueType>> queue, TKeyType lo, TKeyType hi)
        {
            if (x == null) return;
            int cmplo = lo.CompareTo(x.Key);
            int cmphi = hi.CompareTo(x.Key);
            if (cmplo < 0) KeyValues(x.Left, queue, lo, hi);
            if (cmplo <= 0 && cmphi >= 0)
            {
                KeyValuePair<TKeyType, TValueType> item = new(x.Key, x.Value);
                queue.Enqueue(item);
            }
            if (cmphi > 0) KeyValues(x.Right, queue, lo, hi);
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="lo"></param>
        /// <param name="hi"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throw <code>ArgumentNullException</code> if both hi and lo are not non-null</exception>

        public int Size(TKeyType lo, TKeyType hi)
        {
            if (lo is null) throw new ArgumentNullException(nameof(lo), "lo value must not be null");
            if (hi is null) throw new ArgumentNullException(nameof(hi), "hi value must not be null");

            if (lo.CompareTo(hi) > 0) return 0;
            if (Contains(hi)) return Rank(hi) - Rank(lo) + 1;
            else return Rank(hi) - Rank(lo);
        }

        #endregion

        #region Checking Tree ness
        // is the tree rooted at x a BST with all keys strictly between min and max
        // (if min or max is null, treat as empty constraint)
        // Credit: Bob Dondero's elegant solution
        private bool IsBST(Node x, TKeyType min, TKeyType max)
        {
            if (x == null) return true;
            if (min != null && x.Key.CompareTo(min) <= 0) return false;
            if (max != null && x.Key.CompareTo(max) >= 0) return false;
            return IsBST(x.Left, min, x.Key) && IsBST(x.Right, x.Key, max);
        }
        // does this binary tree satisfy symmetric order?
        // Note: this test also ensures that data structure is a binary tree since order is strict
        private bool IsBST()
        {
            return IsBST(root, default, default);
        }
        // are the size fields correct?
        private bool IsSizeConsistent() { return IsSizeConsistent(root); }
        private bool IsSizeConsistent(Node x)
        {
            if (x == null) return true;
            if (x.Count != Size(x.Left) + Size(x.Right) + 1) return false;
            return IsSizeConsistent(x.Left) && IsSizeConsistent(x.Right);
        }

        // check that ranks are consistent
        private bool IsRankConsistent()
        {
            for (int i = 0; i < Size(); i++)
                if (i != Rank(Select(i))) return false;
            foreach (TKeyType key in Keys())
                if (key.CompareTo(Select(Rank(key))) != 0) return false;
            return true;
        }
        // Does the tree have no red right links, and at most one (left)
        // red links in a row on any path?
        private bool Is23() { return Is23(root); }
        private bool Is23(Node x)
        {
            if (x == null) return true;
            if (IsRed(x.Right)) return false;
            if (x != root && IsRed(x) && IsRed(x.Left))
                return false;
            return Is23(x.Left) && Is23(x.Right);
        }

        // do all paths from root to leaf have same number of black edges?
        private bool IsBalanced()
        {
            int black = 0;     // number of black links on path from root to min
            Node x = root;
            while (x != null)
            {
                if (!IsRed(x)) black++;
                x = x.Left;
            }
            return IsBalanced(root, black);
        }

        // does every path from the root to a leaf have the given number of black links?
        private bool IsBalanced(Node x, int black)
        {
            if (x == null) return black == 0;
            if (!IsRed(x)) black--;
            return IsBalanced(x.Left, black) && IsBalanced(x.Right, black);
        }

        #endregion

        private record Node
        {
            public TKeyType Key;
            public TValueType Value;
            public Node Left;
            public Node Right;
            public bool Color;
            public int Count;
            public Node()
            {

            }
            public Node(TKeyType key, TValueType value, bool color)
            {
                Key = key;
                Value = value;
                Color = color;
            }

        }
    }
}
