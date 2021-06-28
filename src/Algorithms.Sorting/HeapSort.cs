using Algorithms.Common;
using System;

namespace Algorithms.Sorting
{
    public class HeapSort<TKey> where TKey : IComparable
    {
        /// <summary>
        /// Worst Case: O(2 * NlgN), Average Case: O(2 * NlgN), best case: O(NlgN) with in place sorting
        /// </summary>
        /// <param name="a"></param>
        public static void Sort(TKey[] a)
        {
            int N = a.Length;
            Construct(a, N);
            ExchangeThenSink(a, N);
        }

        private static void ExchangeThenSink(TKey[] a, int n)
        {
            while (n > 1)
            {
                Exchange(a, 1, n--);
                Sink(a, 1, n);
            }
        }
        /// <summary>
        /// Takes : 1 + lgN compares
        /// Power struggle: a better subordinate is promoted
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        private static void Swim(TKey[] a, int k)
        {
            int kStore = k;
            while (k > 1 && Less(a, kStore / 2, kStore))
            {
                Exchange(a,kStore, kStore/ 2);
                kStore /= 2;
            }
        }
        /// <summary>
        /// PETER Principle: Node promoted to  level of incompetence. Need to sink
        /// Takes : 1 + lgN compares
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <param name="n"></param>
        private static void Sink(TKey[] a, int k, int n)
        {
            while (2 * k <= n)
            {
                int j = 2 * k;
                if (j < n && Less(a, j, j + 1)) j++; // find max child index
                if (!Less(a, k, j)) break; // now heapified so exit
                Exchange(a, k, j); // swap
                k = j;

            }
        }

        private static void Construct(TKey[] a, int n)
        {
            for (int k = n / 2; k >= 1; k--)
            {
                Sink(a,k,n);
            }
        }

        private static bool Less(TKey[] a, int i, int j) => CompareHelper<TKey>.Less(a[i], a[j]);
        private static void Exchange(TKey[] a, int i, int j) => CompareHelper<TKey>.Exchange(a, i, j);
    }
}
