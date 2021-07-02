using System;
using Xunit;

namespace Chitataunga.Algorithms.Sorting.Tests
{
    public class HeapSortGeneralTest
    {
        [Fact]
        public void TestSortedStringFalseBeforeSortAndTrueAfterSort()
        {
            IComparable[] names = {"my", "name", "is", "anderson", "from" , "a", "country"};
            bool isSorted = HeapSortGeneral.IsSorted(names);
            Assert.False(isSorted);
            HeapSortGeneral.Sort(names);
            isSorted = HeapSortGeneral.IsSorted(names);
            Assert.True(isSorted);
        }

        [Fact]
        public void TestSortedGenericIntFalseBeforeSortAndTrueAfterSort()
        {
            int[] nums = {2, 4, 6, 9, 1, 200, 120, 22, 0, -1, 900, 2300};
            bool isSorted = HeapSortGeneral<int>.IsSorted(nums);
            Assert.False(isSorted);
            HeapSortGeneral<int>.Sort(nums);
            isSorted = HeapSortGeneral<int>.IsSorted(nums);
            Assert.True(isSorted);
        }
    }
}
