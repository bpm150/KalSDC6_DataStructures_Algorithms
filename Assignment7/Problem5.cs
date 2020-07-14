using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
        public static class Problem5
        {
        // Assumptions / Observations

        // If currToKthFE never advances, then there is no k'th node from the end.
        // n is too small.

        // k == 0 meaning last element, currTE and currToKthFE start together

        // k must be between 0 and n-1, else returns null

        // What behavior is desired if there is a loop in the list?

        public class Node<T>
        {
            public T Data { get; set; }

            // Type param T used at class level and for Next property on purpose
            // warning CS0693: Type parameter 'T' has the same name as the type
            // parameter from outer type 'Node<T>'

            public Node<T> Next { get; set; }


            // CollToNodeList does not need to be generic on T
            // In fact, it should not be
            // Need to resolve into Node<T> to get to CollToNodeList, as done
            // in the nunit tests: Node<int>.CollToNodeList
            public static Node<T> CollToNodeList(IEnumerable<T> coll)
//#pragma warning disable CS0693
//#pragma warning restore CS0693
            {
                var dummyHead = new Node<T>();
                var curr = dummyHead;

                foreach (var item in coll)
                {
                    curr.Next = new Node<T>
                    {
                        Data = item,
                    };
                    curr = curr.Next;
                }

                return dummyHead.Next;
            }

        }


        public static Node<T> GetKthFromEnd<T>(Node<T> head, int k)
            {
                if (head == null)
                    return null;

                if (k < 0)
                    return null;

                var currTE = head;
                var currToKthFE = head;

                var steps = 0;
                for (; currTE != null; ++steps)
                {
                    if (steps >= k + 1)
                        currToKthFE = currToKthFE.Next;

                    currTE = currTE.Next;
                }

                var n = steps + 1;
                if (k >= n)
                    return null;

                return currToKthFE;
            }
        }

}




//NUNIT TESTS


//namespace Testing
//{
//    using NUnit.Framework;
//    using Assignment7;
//    using static Assignment7.Problem5;

//    [TestFixture]
//    public class Tests
//    {
//        [Test]
//        public void SampleTest()
//        {
//            var arrInput = new int[] { 1, 2, 3, 4, 5 };
//            var nodeListHeadInput = Node<int>.CollToNodeList(arrInput);

//            var k = 2;

//            var expectedResult = arrInput[arrInput.Length - k - 1];

//            var actualResult = GetKthFromEnd(nodeListHeadInput, k);

//            Assert.AreEqual(expectedResult, actualResult.Data);
//        }


//        [Test]
//        public void LastElementTest()
//        {
//            var k = 0;
//            var arrInput = new int[] { 1, 2, 3, 4, 5 };

//            TestHelper(arrInput, k);
//        }

//        [Test]
//        public void FirstElementTest()
//        {
//            var arrInput = new int[] { 1, 2, 3, 4, 5 };
//            var k = arrInput.Length - 1;

//            TestHelper(arrInput, k);
//        }


//        [Test]
//        public void NullTest()
//        {
//            Node<int> nodeListHeadInput = null;

//            var actualResult = GetKthFromEnd(nodeListHeadInput, 100);

//            Assert.AreEqual(null, actualResult);
//        }

//        [Test]
//        public void NegKTest()
//        {
//            var arrInput = new int[] { 1, 2, 3, 4, 5 };
//            var k = -1;

//            var nodeListHeadInput = Node<int>.CollToNodeList(arrInput);

//            Node<int> expectedResult = null;

//            var actualResult = GetKthFromEnd(nodeListHeadInput, k);

//            Assert.AreEqual(expectedResult, actualResult);
//        }

//        private void TestHelper(int[] arrInput, int k)
//        {
//            var nodeListHeadInput = Node<int>.CollToNodeList(arrInput);

//            var expectedResult = arrInput[arrInput.Length - k - 1];

//            var actualResult = GetKthFromEnd(nodeListHeadInput, k);

//            Assert.AreEqual(expectedResult, actualResult.Data);
//        }

//    }
//}
