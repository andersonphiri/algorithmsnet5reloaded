using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chitataunga.Algorithms.Searching
{
    /// <summary>
    /// Search - worst case O(Log(N))
    /// Search - average case (search hit) O(Log(N))
    /// Insert - O(N)
    /// Insert - average case - O(N /2)
    /// Efficiently Support Ordered ops: YES
    /// </summary>
    /// <typeparam name="TKeyType"></typeparam>
    /// <typeparam name="TValueType"></typeparam>
    public class OrderedArrayBinarySearchST<TKeyType, TValueType>
         where TKeyType : IComparable<TKeyType>
    {
        private List<TKeyType> _keys;
        private List<TValueType> _values;
        private ulong N;
        public OrderedArrayBinarySearchST(ulong capacity)
        {
            _keys = new List<TKeyType>((int)capacity);
            _values = new List<TValueType>((int)capacity);
            N = 0;
        }
        #region Helper methods
        public int Count() => (int)N;
        public ulong LongCount() => N;
        public bool IsEmpty() => N == 0;
        public ulong Rank(TKeyType key)
        {
            ulong lo = 0;
            ulong hi = N - 1;
            while (lo <= hi)
            {
                ulong mid = lo + (hi - lo) / 2;
                int comp = key.CompareTo(_keys[(int)mid]);
                if (comp < 0) { hi = mid - 1; }
                if (comp > 0) { lo = mid + 1; }
                else return mid;
            }
            return lo;
        }
        public ulong RecursiveRank(TKeyType key, ulong lo, ulong hi)
        {
            if (hi < lo) return lo;
            ulong mid = lo + (hi - lo) / 2;
            int comp = key.CompareTo(_keys[(int)mid]);
            if (comp < 0) return RecursiveRank(key, lo, mid - 1);
            else if (comp > 0) return RecursiveRank(key, mid + 1, hi);
            else return mid;

        }
        #endregion

        #region Get Value from key
        public TValueType Get(TKeyType key)
        {
            if (IsEmpty()) return default;
            int rank = (int)Rank(key);
            if ((ulong)rank < N && key.CompareTo(_keys[rank]) == 0) return _values[rank];
            return default;
        }
        #endregion
        #region Insertion
        /// <summary>
        /// search for key, if found, update table else grow table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Put(TKeyType key, TValueType @value)
        {
            int rank = (int)Rank(key);
            if ((ulong)rank < N && key.CompareTo(_keys[rank]) == 0)
            {
                // key found, so update
                _values[rank] = value;
                return;
            }
            for (int j = (int)N; j > rank; j--)
            {
                _keys[j] = _keys[j - 1];
                _values[j] = _values[j - 1];
            }
            _keys[rank] = key;
            _values[rank] = value;
            N++;
        }
        #endregion


        #region deletion
        public void Delete(TKeyType key)
        {
            int rank = (int)Rank(key);
            if ((ulong)rank < N && key.CompareTo(_keys[rank]) == 0)
            {
                // key found, so remove it
                for (int j = (int)N; j > rank; j--)
                {
                    _keys[j - 1] = _keys[j];
                    _values[j - 1] = _values[j];
                }
                N--;
            }
        }
        #endregion
    }
}
