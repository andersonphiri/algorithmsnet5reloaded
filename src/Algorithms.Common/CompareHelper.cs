using System;

namespace Algorithms.Common
{
    public class CompareHelper<T> where T : IComparable
    {
        public static bool Less(T a, T b)
        {
            return a.CompareTo(b) < 0;
        }

        public static void Exchange(T[] a, int index1, int index2)
        {
            T temp = a[index1];
            a[index1] = a[index2];
            a[index2] = temp;
        }
    }
}
