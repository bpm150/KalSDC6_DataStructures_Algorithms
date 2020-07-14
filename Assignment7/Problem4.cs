using System.Collections.Generic;

namespace Assignment7
{



    public static class Problem4
    {

        public class Node<T>
        {
            public T Data { get; set; }
            public Node<T> Next { get; set; }

            // ? Does method need to be generic on T or not
            public static Node<T> CreateFromCollection(IEnumerable<T> collection)
            {
                var dummyHead = new Node<T>();
                var currNode = dummyHead;

                foreach (var item in collection)
                {
                    currNode.Next = new Node<T>();
                    currNode = currNode.Next;
                    currNode.Data = item;
                }
                var actualHead = dummyHead.Next;
                return actualHead;
            }
        }


        public static bool HasLoop<T>(Node<T> head)
        {
            // TODO: Base cases

            var fastPointer = head;
            var slowPointer = head;

            var advanceSlowPointer = false;

            while (fastPointer != null)
            {
                fastPointer = fastPointer.Next;

                if (advanceSlowPointer == true)
                {
                    slowPointer = slowPointer.Next;

                    if (slowPointer == fastPointer)
                        return true;

                    advanceSlowPointer = false;
                }
                else
                    advanceSlowPointer = true;
            }

            return false;
        }
    }
}




// NUNIT TESTS COMMENTED OUT
//namespace Testing
//{
//    using System.Collections.Generic;
//    using NUnit.Framework;
//    using Assignment7;
//    using static Assignment7.Problem4;

//    [TestFixture]
//    public class Tests
//    {
//        [Test]
//        public void NoLoopTest()
//        {
//            var inputList = new List<int> { 1, 2, 3, 4, 5, 6, };

//            var inputNodeList = Node<int>.CreateFromCollection(inputList);

//            var expectedResult = false;

//            Assert.AreEqual(expectedResult, HasLoop(inputNodeList));
//        }

//        [Test]
//        public void YesLoopTest()
//        {
//            var inputList = new List<int> { 1, 2, 3, 4, 5, 6, };

//            var inputNodeList = Node<int>.CreateFromCollection(inputList);

//            AddLoopToNodeList(inputNodeList);

//            var expectedResult = true;

//            Assert.AreEqual(expectedResult, HasLoop(inputNodeList));
//        }

//        private void AddLoopToNodeList<T>(Node<T> head)
//        {
//            var currNode = head;
//            var loopPoint = head.Next;

//            while (currNode.Next != null)
//                currNode = currNode.Next;

//            currNode.Next = loopPoint;

//            // Chris suggestions:

//            // Try test case of all same numbers: 1111
//            // (even though my implementation doesn't use the value of the objects
//            // in the loop-detection logic)


//            // Write helper function to always loop to head, then...
//            // (? Pass in a reference to the node in the list that the
//            // loop join should be)

//            // Note that I am still abbreviating my variable names
//            // Especially since we are using an IDE and I can copypasta
//            // Work on using longer, more descriptive variable names


//            // Proof that this implementation works
//            // Chris says is a proof by induction...which is evidently an unintuitive
//            // kind of proof.




//        }
//    }
//}