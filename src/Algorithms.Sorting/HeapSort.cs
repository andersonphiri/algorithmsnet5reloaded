using Algorithms.Common;
using System;
using System.Collections.Generic;

namespace Algorithms.Sorting
{
    /// <summary>
    /// Time complexity: worst case = average case = (2 * NlgN), best case = (NlgN)
    /// </summary>
    public class HeapSortGeneral
    {
        public HeapSortGeneral()
        {

        }


        /// <summary>
        /// Time complexity: worst case = average case = (2 * NlgN), best case = (NlgN)
        /// </summary>
        /// <param name="pq"></param>
        public static void Sort(IComparable[] pq)
        {
            int N = pq.Length;
            for (int k = N / 2; k >= 1; k--)
                Sink(pq, k, N);
            while (N > 1)
            {
                Exchange(pq, 1, N--);
                Sink(pq, 1, N);
            }
        }

        /// <summary>
        /// Time complexity: worst case = average case = (2 * NlgN), best case = (NlgN)
        /// </summary>
        /// <param name="pq"></param>
        public static void Sort(List<IComparable> pq)
        {
            int N = pq.Count;
            for (int k = N / 2; k >= 1; k--)
                Sink(pq, k, N);
            while (N > 1)
            {
                Exchange(pq, 1, N--);
                Sink(pq, 1, N);
            }
        }

        protected static void Sink(IComparable[] pq, int k, int N)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Less(pq, j, j + 1)) j++;
                if (!Less(pq, k, j)) break;
                Exchange(pq, k, j);
                k = j;
            }
        }

        protected static void Sink(List<IComparable> pq, int k, int N)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Less(pq, j, j + 1)) j++;
                if (!Less(pq, k, j)) break;
                Exchange(pq, k, j);
                k = j;
            }
        }

        protected static bool Less(IComparable[] pq, int i, int j)
        {
            return pq[i - 1].CompareTo(pq[j - 1]) < 0;
        }
        protected static bool Less(List<IComparable> pq, int i, int j)
        {
            return pq[i - 1].CompareTo(pq[j - 1]) < 0;
        }
        protected static bool Less(IComparable a, IComparable b)
        {
            return a.CompareTo(b) < 0;
        }

        protected static void Exchange(IComparable[] pq, int i, int j)
        {
            IComparable swap = pq[i - 1];
            pq[i - 1] = pq[j - 1];
            pq[j - 1] = swap;
        }

        protected static void Exchange(List<IComparable> pq, int i, int j)
        {
            IComparable swap = pq[i - 1];
            pq[i - 1] = pq[j - 1];
            pq[j - 1] = swap;
        }

        public static bool IsSorted(IComparable[] pq)
        {
            for (int i = 1; i < pq.Length; i++)
            {
                if (Less(pq[i], pq[i - 1])) return false;
            }

            return true;
        }
        public static bool IsSorted(List<IComparable> pq)
        {
            for (int i = 1; i < pq.Count; i++)
            {
                if (Less(pq[i], pq[i - 1])) return false;
            }

            return true;
        }


    }

    public class HeapSortGeneral<TKey> : HeapSortGeneral where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Time complexity: worst case = average case = (2 * NlgN), best case = (NlgN)
        /// </summary>
        /// <param name="pq"></param>
        public static void Sort(List<TKey> pq)
        {
            int N = pq.Count;
            for (int k = N / 2; k >= 1; k--)
                Sink(pq, k, N);
            while (N > 1)
            {
                Exchange(pq, 1, N--);
                Sink(pq, 1, N);
            }
        }

        /// <summary>
        /// Time complexity: worst case = average case = (2 * NlgN), best case = (NlgN)
        /// </summary>
        /// <param name="pq"></param>
        public static void Sort(TKey[] pq)
        {
            int N = pq.Length;
            for (int k = N / 2; k >= 1; k--)
                Sink(pq, k, N);
            while (N > 1)
            {
                Exchange(pq, 1, N--);
                Sink(pq, 1, N);
            }
        }
        protected static void Sink(TKey[] a, int k, int N)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Less(a, j, j + 1)) j++;
                if (!Less(a, k, j)) break;
                Exchange(a, j, k);
                k = j;
            }
        }

        protected static void Sink(List<TKey> a, int k, int N)
        {
            while (2 * k <= N)
            {
                int j = 2 * k;
                if (j < N && Less(a, j, j + 1)) j++;
                if (!Less(a, k, j)) break;
                Exchange(a, j, k);
                k = j;
            }
        }

        protected static bool Less(TKey[] pq, int i, int j)
        {
            return pq[i - 1].CompareTo(pq[j - 1]) < 0;
        }
        protected static bool Less(List<TKey> pq, int i, int j)
        {
            return pq[i - 1].CompareTo(pq[j - 1]) < 0;
        }
        protected static bool Less(TKey a, TKey b)
        {
            return a.CompareTo(b) < 0;
        }

        protected static void Exchange(TKey[] pq, int i, int j)
        {
            TKey swap = pq[i - 1];
            pq[i - 1] = pq[j - 1];
            pq[j - 1] = swap;
        }

        protected static void Exchange(List<TKey> pq, int i, int j)
        {
            TKey swap = pq[i - 1];
            pq[i - 1] = pq[j - 1];
            pq[j - 1] = swap;
        }

        public static bool IsSorted(TKey[] pq)
        {
            for (int i = 1; i < pq.Length; i++)
            {
                if (Less(pq[i], pq[i - 1])) return false;
            }

            return true;
        }
        public static bool IsSorted(List<TKey> pq)
        {
            for (int i = 1; i < pq.Count; i++)
            {
                if (Less(pq[i], pq[i - 1])) return false;
            }

            return true;
        }


    }


}
