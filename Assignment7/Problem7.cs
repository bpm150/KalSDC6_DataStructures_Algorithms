using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
using System.Text;

namespace Assignment7
{

    // Given a sorted linked list where every node
    // represents a sorted linked list and contains two
    // pointers of its type, next and bottom, flatten
    // the linked list to be a single, sorted, linked list.

    // Let the first list be list 0,
    // the second list be list 1, etc.

    // Merge list 1 into list 0
    // Note that the head node of list 1
    // belongs somewhere after the head node of list 0
    // Then merge list 2 in starting at the head node for list 1
    // (don't start back at the head node for list 0)



    // Node<T> doesn't need T to implement IComparable<T>
    // Only the 
    //  where T : IComparable<T>
    // ? How to: the relationship between T and U

    public class Node<T> where T : IComparable<T> // : IEnumerable<T>
    {
        public T Data { get; set; }

        public Node<T> Next { get; set; }
        public Node<T> Bottom { get; set; }

        public List<List<T>> To2DList()
        {
            var outputList = new List<List<T>>();

            var currBaseHead = this;

            // What all syntax can you do with the List<T> type?
            for (var i = 0; currBaseHead != null; ++i)
            {
                outputList.Add(new List<T>());
                outputList[i].Add(currBaseHead.Data);

                var currBottomNode = currBaseHead.Bottom;
                while (currBottomNode != null)
                {
                    outputList[i].Add(currBottomNode.Data);
                    currBottomNode = currBottomNode.Next;
                }

                currBaseHead = currBaseHead.Next;
            }
            return outputList;
        }

        public IEnumerable<T> EnumerateIfFlattened()
        {
            var currNode = this;

            while (currNode != null)
            {
                if (currNode.Bottom != null)
                    throw new InvalidOperationException(
                      "Cannot enumerate a Node list that is not already flattened.");

                yield return currNode.Data;

                currNode = currNode.Next;
            }

        }



        //public IEnumerator<T> GetEnumerator()
        //{
        //var currNode = this;

        //}

        //IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //return this.GetEnumerator();

        //}


        public static Node<T> ConstructNodeListFrom2DArray(T[][] inputArray)
        {
            if (inputArray == null || inputArray.Length == 0)
                return null;

            // Dummy to make the loops nicer
            // Will chop off before returning
            var dummyStarterBaseHeadNode = new Node<T>();
            var currBaseHeadNode = dummyStarterBaseHeadNode;

            for (var i = 0; i < inputArray.Length; ++i)
            {
                if (inputArray[i] == null || inputArray[i].Length < 1)
                    throw new ArgumentException("Each input array must have at least one element.");

                // Only make a new base head node when we know for certain that there
                // is another array of at least one element to add

                currBaseHeadNode.Next = new Node<T>();
                currBaseHeadNode = currBaseHeadNode.Next;
                currBaseHeadNode.Data = inputArray[i][0];

                if (inputArray[i].Length < 2)
                    continue;

                // If at least two elements in the current array,
                // then the second one becomes the current bottom node
                currBaseHeadNode.Bottom = new Node<T>();
                var currBottomNode = currBaseHeadNode.Bottom;
                currBottomNode.Data = inputArray[i][1];

                for (var j = 2; j < inputArray[i].Length; ++j)
                {
                    // If there are three or more elements in the current array,
                    // Then it and all of the remaining elements of that array
                    // follow each other as next elements

                    currBottomNode.Next = new Node<T>();
                    currBottomNode = currBottomNode.Next;
                    currBottomNode.Data = inputArray[i][j];
                }
            }

            var actualBaseHeadNode = dummyStarterBaseHeadNode.Next;
            return actualBaseHeadNode;
        }


    }

    public static class Problem7
    {
        // ALGORITHM WALKTHROUGH:
        // Hold reference to the head of list 0, 1 and 2
        // For each list 0 and list 1,
        // point its Next reference at its Bottom reference object
        // (Thus tearing off the Bottom list and flattening it into Next)
        // Walk list 0 looking for an element that is greater than the element at the head of list 1
        // If find,
        // Promote the next object of list 1 to be the head of list 1 and
        // insert the old head of list 1 into list 0 in its new position
        // (keep holding reference to this element, it's where we'll start merging in list 2)
        // If not find,
        // Add the remaining items from list 1 to the end of list 0

        // When run out of elements in either list 0 or list 1, stop merging and move on to list 2
        // Save off a reference to list 3
        // For list 2 (as we did with list 0 and list 1),
        // point its Next reference at its Bottom reference object
        // (Thus tearing off the Bottom list and flattening it into Next)

        // Begin merging list 2 where the head of list 1 was merged
        // Walk and merge as above

        // When you've merged in the last element from the merging-in list,
        // or added the remaining unmerged items to the end of the merged list
        // Move to the next numbered (base) list
        // If there is none, (base list ptr is null), then merge is complete

        // All of the linked list elements are merged into a single, flattened, sorted linked list

        public static void Flatten2DList<T>(Node<T> head) where T : IComparable<T>
        {
            if (head == null)
                // What using statement needed for exceptions? System?
                throw new ArgumentNullException("head is null");

            // Dummy head to make looping logic simpler.
            // We would chop it off before we return the final list
            // But the head of the final list is guaranteed to be the same as the head
            // that was passed in by the caller, so there's no point in returning it
            var dummyStartBaseHead = new Node<T>();
            //{
            // XX Has same Data value as the head node, so that the head node naturally
            // merges after it in the main logic
            // Data value here doesn't matter.
            // If a node is already merged, we don't need to compare to it
            //Data = head.Data,
            //};

            var prevBaseHeadMerged = dummyStartBaseHead;
            var currBaseHead = head;
            Node<T> nextBaseHead;

            // At minimum, need to do the tear-off for an input of a single linked list
            // (with base having only Bottom node, and no Next node)  

            // dummy starts as "already" torn off/flattened, so the initial list can follow the logic
            // of all other lists

            while (currBaseHead != null)
            {
                nextBaseHead = currBaseHead.Next;

                MergeInBaseHead(prevBaseHeadMerged, currBaseHead);

                prevBaseHeadMerged = currBaseHead;
                currBaseHead = nextBaseHead;


            }


        }


        // lastBaseHead is where to start merging (already flattened)
        // baseHeadToMerge is what to start merging (not flattened yet)

        // XX Returns reference to the next base head after the current one (not flattened yet)
        // or null if there are no more base heads
        // Caller responsible for backing up next base head before calling
        private static void MergeInBaseHead<T>(
          Node<T> prevBaseHeadMerged,
          Node<T> baseHeadToMerge) where T : IComparable<T>
        {
            var currToMerge = baseHeadToMerge;

            // Flatten before merging
            currToMerge.Next = currToMerge.Bottom;
            currToMerge.Bottom = null;

            var currMerged = prevBaseHeadMerged;

            // Note that we don't need to keep track of the baseHeadToMerge reference for the caller
            // Caller responsible for noting that and passing it back in on next call


            // Merge nodes until we run out of nodes to merge
            // (or we reach the end of nodes already merged to compare to, that is, when
            // all remaining nodes to merge are greater than the largest node already merged)
            while (currToMerge != null)
            {
                if (currMerged.Next == null)
                {
                    // No more nodes left to compare to
                    // Add the nodes-to-merge directly to the end of the merged list
                    currMerged.Next = currToMerge;
                    return;
                }

                // Maybe a helper for doing the merge, maybe not?
                // CompareTo value: Less than zero 	meaning: This instance precedes obj in the sort order.
                // https://docs.microsoft.com/en-us/dotnet/api/system.icomparable.compareto?view=netcore-3.1#notes-to-implementers
                if (currToMerge.Data.CompareTo(currMerged.Next.Data) < 0)
                {
                    var nodeMergingNow = currToMerge;
                    currToMerge = currToMerge.Next;

                    var nodeBiggerThanMergingNow = currMerged.Next;
                    currMerged.Next = nodeMergingNow;
                    nodeMergingNow.Next = nodeBiggerThanMergingNow;
                }

                currMerged = currMerged.Next;
            }

        }

    }
}



// NUNIT TESTING

//using System.Collections.Generic;
//using System.Collections;
//using System;
//using System.Linq;
//using System.Text;

//namespace Testing
//{
//    using NUnit.Framework;
//    using Assignment7;
//    using static Assignment7.Problem7;

//    [TestFixture]
//    public class Tests
//    {
//        [Test]
//        public void KalTest()
//        {

//            var arrayInput = new int[][]
//            {
//        new int[]{5,7,8,30,},
//        new int[]{10,20,},
//        new int[]{19,22,50,},
//        new int[]{28,35,40,45,},
//            };

//            // TODO: Write a visualization for 2D array to use during debugging

//            var expectedOutput = new int[] { 5, 7, 8, 10, 19, 20, 22, 28, 30, 35, 40, 45, 50, };

//            var nodeList = Node<int>.ConstructNodeListFrom2DArray(arrayInput);

//            var nodeListAs2DList = nodeList.To2DList();

//            Flatten2DList(nodeList);

//            var outputFlattenedToArray = nodeList.EnumerateIfFlattened().ToArray();

//            TestContext.Out.WriteLine($"expectedOutput == {CollectionToString(expectedOutput)}");
//            TestContext.Out.WriteLine($"outputFlattenedToArray == {CollectionToString(outputFlattenedToArray)}");

//            Assert.AreEqual(expectedOutput, outputFlattenedToArray);
//        }


//        // Does this method need to be generic on T?
//        private string CollectionToString<T>(IEnumerable<T> collection)
//        {
//            var builder = new StringBuilder();

//            foreach (var item in collection)
//                builder.Append($"{item}, ");

//            return builder.ToString();
//        }
//    }
//}
