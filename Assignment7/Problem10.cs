using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment7
{
    class Problem10
    {
        // Given a singly linked list of integers,
        // Your task is to complete the function
        // isPalindrome that returns true if the
        // given list is palindrome, else returns false.

        public class IntNode
        {
            public int Data { get; set; }
            public IntNode Next { get; set; }

            public int ListLength
            {
                get
                {
                    return this.ToIEnumerable().Count();
                }
            }

            // Can you return an IEnumerable<int> from any method?
            // (As an alternative for implementing IEnumerable<T>
            // for the Node<int> class?)
            // Must this be an instance method on Node<T>?
            // Else must it be a static method on Node<T>?
            // Else can it be any method on any class?

            public static IntNode CollToIntNode(IEnumerable<int> coll)
            {
                var dummyHead = new IntNode();
                var curr = dummyHead;

                foreach (var item in coll)
                {
                    curr.Next = new IntNode();
                    curr = curr.Next;
                    curr.Data = item;
                }

                return dummyHead.Next;
            }

            public IEnumerable<int> ToIEnumerable()
            {
                var curr = this;

                while (curr != null)
                {
                    yield return curr.Data;
                    curr = curr.Next;
                }
            }

            public static IEnumerable<int> NodeListToIEnumerable(IntNode head)
            {
                if (head == null)
                    return null;

                return head.ToIEnumerable();
            }

            // I guess that a method isn't "an iterator" unless it actually contains
            // the yield keyword (as yield return and yield break)

            public static IEnumerable<int> NodeListToIEnumerableFromScratch(IntNode head)
            {
                if (head == null)
                    yield break;

                var curr = head;

                while (curr != null)
                {
                    yield return curr.Data;
                    curr = curr.Next;
                }
            }

            public List<int> ToList()
            {
                var outputList = new List<int>();

                var enumerator = this.ToIEnumerable();

                foreach (var item in enumerator)
                {
                    outputList.Add(item);
                }

                return outputList;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                foreach (var item in this.ToIEnumerable())
                {
                    sb.Append($"{item}, ");
                }

                return sb.ToString();
            }
        }

        public static bool IsPalindrome(IntNode head)
        {
            if (head == null)
                return false;

            // Traverse the list to count n
            var nodeListLength = head.ListLength;

            if (nodeListLength == 1)
                return true;

            // n == 2
            // true iff elements are equal
            //// Two node base case (not necessary)
            //if (nodeListLength == 2)
            //    {
            //        if (head.Data != head.Next.Data)
            //            return false;
            //    }
            //    // Three node base case (not necessary)
            //    if (nodeListLength == 3)
            //    {
            //        if (head.Data != head.Next.Next.Data)
            //            return false;
            //    }

            // Reverse first half of list 1/2 n
            // if n < 4, remember don't need to put list back together at the end
            // no reversing happens

            // reverse 1/2 n nodes (left half of the nodes, not including the middle node if n is odd)
            //if (nodeListLength > 1)
            //    {
            // reverse numNodesToReverse nodes at beginning of list
            // we'll undo this later to restore the list for the caller
            var numNodesToReverse = nodeListLength / 2;
            // This is true for both even and odd n 

            // I like Chris' idea of actually reversing the first half
            // of the list in-place (rather than splitting it off as a
            // seperate sublist, then needing to tack it back on again)
            // Write logic to reverse k nodes each time. Then easy
            // to use the same helper for both the original reverse,
            // and the ultimate restore.

            //Console.WriteLine(head);
            //Console.WriteLine(numNodesToReverse);
            //Console.WriteLine("\n");

            // Returns head of half-reversed list
            var leftRevHead = ReverseNodesAtStartOfNodeList(head, numNodesToReverse);
            // Tail of reversed part of list is the head that was passed in
            // Tail of the reversed part of the list is still hooked up to
            // the non-reversed part of the list

            //Console.WriteLine(leftRevHead);

            var nodeAfterReversedPart = head.Next;

            //var leftRevHead = prev;

            // Handles finding proper start head for comparision (based on even or odd n)
            var rightHalfHead = GetRightHalfHead(nodeAfterReversedPart, nodeListLength);

            // Comparisons 1/2 n
            var isPal = IntNodeListsDataAreEqual(leftRevHead, rightHalfHead);

            // Undo the reverse 1/2 n
            ReverseNodesAtStartOfNodeList(leftRevHead, numNodesToReverse);

            // Not necessary anymore:
            // Hook-up the right half of the original list
            // to the left half of the (restored) original list
            //leftRevHead.Next = nodeAfterReversedPart;

            //Console.WriteLine($"leftRevHead == {leftRevHead}");

            // Repoint what will be the restored tail now,
            // calling the restore method
            // It's the head of the (sub)list that is being resored
            // Then the reverse method just does its job normally
            // OR...do this after the helper finishes,
            // as maybe the helper nulls the Next reference on that node
            // (the undoPrev node in the helper, which is the leftRevHead outside the helper)

            //}
            return isPal;
        }

        // Returns head of (partially) reversed list
        private static IntNode ReverseNodesAtStartOfNodeList(IntNode head, int numNodesToReverse)
        {
            var prev = head;
            var curr = head.Next;
            // NOT nulling the Next at the end of the reversed sublist:
            //prev.Next = null;
            // Thus leaving the non-reversed nodes at the end of the reversed sublist
            // attached to the end of the reversed sublist
            // Makes this easier to undo later in that we can reuse this helper
            // Note that no Next references get nulled in this method's operation,
            // unless we reverse all of the nodes in the list, which we won't
            // in the IsPalindrome problem

            //Console.WriteLine(head);
            //Console.WriteLine(head.Next);
            //Console.WriteLine(numNodesToReverse);
            //Console.WriteLine("\n");

            for (var i = numNodesToReverse - 1; i > 0; --i)
            {
                var temp = curr.Next;
                curr.Next = prev;

                prev = curr;
                curr = temp;
            }

            head.Next = curr;

            // The trouble here is that when reversing a list (and up until now, a sublist)
            // I would null prev.Next before starting the loop
            // As Chris pointed out, not-nulling it is not enough, must re-point it
            // to the head of the non-reversed sublist on the right end of the list
            // (may be null in the general case if reversing every node, but not doing this for IsPalindrome)

            return prev;
        }

        private static IntNode GetRightHalfHead(IntNode nodeAfterReversedPart, int nodeListLength)
        {
            if (nodeListLength % 2 == 0)
                // When n is even, start comparing at the two middlemost nodes
                return nodeAfterReversedPart;

            // When n is odd, start comparing at the nodes to the left and right of
            // the middlemost node
            return nodeAfterReversedPart.Next;
        }

        private static bool IntNodeListsDataAreEqual(IntNode leftHead, IntNode rightHead)
        {
            var leftComp = leftHead;
            var rightComp = rightHead;

            // When changed design to Chris' suggestion of reversing the first half of the list in place
            // this means that there is no null "at the end of" the elements that we are comparing at the end
            // of the list. Makes it not a great way to decide when to stop
            // Could look for the null at the end of the collection, or do 1/2 n comparisons
            // This helper doesn't know about 1/2 n, so let's stop when we reach the null at the end of the
            // list

            while (rightComp != null)
            {
                //Console.WriteLine($"leftComp.Data == {leftComp.Data}");
                //Console.WriteLine($"rightComp.Data == {rightComp.Data}");
                //Console.WriteLine("\n");

                if (leftComp.Data != rightComp.Data)
                    return false;

                leftComp = leftComp.Next;
                rightComp = rightComp.Next;
            }

            return true;
        }

        //private static void ReverseList(IntNode head)
        //{
        //    var undoPrev = head;
        //    var undoCurr = undoPrev.Next;
        //    undoPrev.Next = null;

        //    while (undoCurr != null)
        //    {
        //        var temp = undoCurr.Next;
        //        undoCurr.Next = undoPrev;

        //        undoPrev = undoCurr;
        //        undoCurr = temp;
        //    }

        //}


        // Option A: Reverse first half in place, then re-reverse before returning

        // Option B: Store first half of list in an array to comapre to end of list
        // 1/2n space (twice as space efficient as naive solution of storing entire
        // list in an array)


        //}
    }
}


// NUNIT TESTS
//namespace Testing
//{
//    using NUnit.Framework;
//    using Assignment7;
//    using static Assignment7.Problem10;
//    using System.Collections.Generic;
//    using System.Text;

//    [TestFixture]
//    public class Tests
//    {

//        [Test]
//        public void SingleNode()
//        {
//            var intArr = new int[] { 3, };
//            var isPal = true;

//            TestHelper(intArr, isPal);


//        }


//        [Test]
//        public void TwoNodeTrue()
//        {
//            var intArr = new int[] { 3, 3 };
//            var isPal = true;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void TwoNodeFalse()
//        {
//            var intArr = new int[] { 3, 2 };
//            var isPal = false;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void ThreeNodeTrue()
//        {
//            var intArr = new int[] { 3, 2, 3 };
//            var isPal = true;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void ThreeNodeFalse()
//        {
//            var intArr = new int[] { 3, 2, 1 };
//            var isPal = false;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void OddTestTrue()
//        {
//            var intArr = new int[] { 3, 2, 1, 2, 3, };
//            var isPal = true;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void OddTestFalse()
//        {
//            var intArr = new int[] { 3, 2, 1, 2, 4, };
//            var isPal = false;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void EvenTestTrue()
//        {
//            var intArr = new int[] { 3, 2, 2, 3, };
//            var isPal = true;

//            TestHelper(intArr, isPal);
//        }

//        [Test]
//        public void EvenTestFalse()
//        {
//            var intArr = new int[] { 3, 2, 1, 3, };
//            var isPal = false;

//            TestHelper(intArr, isPal);
//        }

//        private void TestHelper(int[] intArr, bool isPal)
//        {
//            var intNodeList = IntNode.CollToIntNode(intArr);

//            var originalList = intNodeList.ToList();

//            Assert.AreEqual(isPal, IsPalindrome(intNodeList));

//            TestContext.Out.WriteLine("originalList");
//            TestContext.Out.WriteLine(IntCollectionToString(originalList));
//            TestContext.Out.WriteLine("intNodeList.ToList()");
//            TestContext.Out.WriteLine(IntCollectionToString(intNodeList.ToList()));
//            TestContext.Out.WriteLine("\n");

//            Assert.AreEqual(originalList, intNodeList.ToList());

//        }

//        private string IntCollectionToString(IEnumerable<int> collection)
//        {
//            var builder = new StringBuilder();

//            foreach (var item in collection)
//                builder.Append($"{item}, ");

//            return builder.ToString();
//        }


//        [Test]
//        public void NullHead()
//        {
//            Assert.AreEqual(false, IsPalindrome(null));
//        }


//    }
//}