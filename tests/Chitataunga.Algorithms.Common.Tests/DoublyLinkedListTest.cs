using Chitataunga.Algorithms.Common.DataStructures;
using Chitataunga.Algorithms.Common.Exceptions;
using System;
using System.Diagnostics;
using Xunit;

namespace Chitataunga.Algorithms.Common.Tests
{
    public class DoublyLinkedListTest
    {
        private DoublyLinkedList<string> _emptyCollection;
        public DoublyLinkedListTest()
        {
            _emptyCollection = new DoublyLinkedList<string>();
        }

        [Fact]
        public void Test_IfEmptyCount_And_LongCount_Return_Zero_And_IsEmpty_Return_True()
        {
            Assert.True(_emptyCollection.IsEmpty());
            Assert.Equal<uint>(0, _emptyCollection.Count);
            Assert.Equal<ulong>(0, _emptyCollection.LongCount);
        }
        [Fact]
        public void Test_IfEmptyTryPeekTryReturnFalse_ShouldReturnFalse_AndOutResultSetToDefaults()
        {
            string result;
            Assert.False(_emptyCollection.TryPeekBack(out result));
            Assert.Null(result);
            Assert.False(_emptyCollection.TryPeekFront(out result));
            Assert.Null(result);

        }

        [Fact]
        public void Test_IfNotEmptyTryPeekTryDeleteShouldReturnTrue_AndReturnFalseAFterClear()
        {
            var collection = CreateCollection();
            string result;
            Assert.True(collection.TryPeekBack(out result));
            Assert.False(string.IsNullOrEmpty(result));
            Assert.True(collection.TryPeekFront(out result));
            Assert.False(string.IsNullOrEmpty(result));
            collection.Clear();
            Assert.False(collection.TryPeekBack(out result));
            Assert.Null(result);

        }

        [Fact]
        public void Test_InsertFrontWithInsertBack_AndPeekFrontAndPeekBack()
        {
            var collection = CreateCollection();
            var count = collection.LongCount;
            collection.InsertAtBack("back 1");
            collection.InsertAtBack("back 2");
            collection.InsertAtBack("back 3");
            Assert.Equal("my", collection.PeekFront());
            Assert.Equal("back 3", collection.PeekBack());
            Assert.Equal<ulong>(count + 3, collection.LongCount);
            string result;
            Assert.True(collection.TryPeekBack(out result));
            Assert.False(string.IsNullOrEmpty(result));
            Assert.True(collection.TryPeekFront(out result));
            Assert.False(string.IsNullOrEmpty(result));
            collection.Clear();
            Assert.ThrowsAny<EmptyCollectionException>(() => collection.PeekBack());
            Assert.ThrowsAny<EmptyCollectionException>(() => collection.PeekFront());
            Assert.False(collection.TryPeekBack(out result));
            Assert.Null(result);
            Assert.False(collection.TryPeekFront(out result));
            Assert.Null(result);
        }


        [Fact]
        public void TestIfEmptyReadAndDeleteWillThrowException()
        {
            Assert.ThrowsAny<EmptyCollectionException>(() => _emptyCollection.DeleteAtBack());
            Assert.ThrowsAny<EmptyCollectionException>(() => _emptyCollection.DeleteAtFront());
            Assert.ThrowsAny<EmptyCollectionException>(() => _emptyCollection.PeekBack());
            Assert.ThrowsAny<EmptyCollectionException>(() => _emptyCollection.PeekFront());
        }
        [Fact]
        public void Test_InsertFront_ShouldReturnLastOrFirstInsertedElementIfPeeked_CountSHouldReturnInsertedCountSoFarAdjustingForDeleteBacks()
        {
            DoublyLinkedList<string> collection = new();
            collection.InsertAtFront("Anderson");
            collection.InsertAtFront("is");
            collection.InsertAtFront("name");
            collection.InsertAtFront("my"); // my name is anderson
            Print(collection);
            Assert.Equal<uint>(4, collection.Count);
            Assert.Equal<ulong>(4, collection.LongCount);

            // back ops
            string next = collection.DeleteAtBack(); // my name is
            Assert.Equal("Anderson", next); // 
            Assert.Equal<uint>(3, collection.Count);
            Assert.Equal<ulong>(3, collection.LongCount);
            next = collection.PeekBack(); // my name <is>
            Assert.Equal("is", next);

            // front ops
            next = collection.DeleteAtFront(); // name is
            Assert.Equal("my", next);
            Assert.Equal<uint>(2, collection.Count);
            Assert.Equal<ulong>(2, collection.LongCount);
            next = collection.PeekFront(); // <name> is
            Assert.Equal("name", next);

            // back and front ops
            next = collection.DeleteAtFront(); // is
            Assert.Equal("name", next);
            Assert.Equal<uint>(1, collection.Count);
            Assert.Equal<ulong>(1, collection.LongCount);
            next = collection.PeekFront(); //  <is>
            string nextTemp = collection.PeekBack(); //  <is>
            Assert.Equal("is", next);
            Assert.Equal("is", nextTemp);

            // remove last item
            next = collection.DeleteAtFront(); // 
            Assert.Equal("is", next);
            Assert.Equal<ulong>(0, collection.LongCount);



        }
        private static void Print<T>(DoublyLinkedList<T> list)
        {
            foreach (var item in list)
            {
                Debug.WriteLine(item.ToString());
            }
        }

        /// <summary>
        /// // calls InsertFronts in the order:
        /// Anderson is name my
        /// </summary>
        /// <returns></returns>
        private static DoublyLinkedList<string> CreateCollection()
        {
            DoublyLinkedList<string> collection = new();
            collection.InsertAtFront("Anderson");
            collection.InsertAtFront("is");
            collection.InsertAtFront("name");
            collection.InsertAtFront("my"); // my name is anderson
            return collection;
        }
    }
}
