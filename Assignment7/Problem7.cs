using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment7
{
    public static class RNG
    {
        public static readonly RandomNumberGenerator = new Random();

    }
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

    public class Node<T> : IEnumerable<T>
    {
        public T Data { get; set; }

        public Node<T> Next { get; set; }
        public Node<T> Bottom { get; set; }

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


        public static Node<T> ConstructNodeListFrom2DCollection(IEnumerable<IEnumerable<T>> collection2D)
        {
            if (collection2D == null)
                return null;

            // Dummy to make the loops nicer
            // Will chop off before returning
            var dummyHead = new Node<T>();
            var currNode = dummyHead;

            foreach (var collection in collection2D)
            {
                var bottomElementDone = false;
                foreach (var element in collection)
                {
                    if (!bottomElementDone)
                    {
                        currNode.Bottom = new Node<T>();
                        currNode = currNode.Bottom;
                        currNode.Data = element;
                        bottomElementDone = true;
                    }
                    else
                    {
                        currNode.Next = new Node<T>();
                        currNode = currNode.Next;
                        currNode.Data = element;
                    }
                }

            }

            var actualHead = dummyHead.Next;
            return actualHead;
        }


        // Does this method need to be generic on T?
        // I think it may not since you need to resolve to it
        // as being a member of the Node<T> class
        public static Node<T> GenerateRandomNodeListFromCollection(IEnumerable<T> collection)
        {
            if (collection == null)
                return null;


            // Generate a sorted copy of the collection (do as built-in linked list, since we will be removing elements from random indicies)
            // (It is possible to do this non-destructively, but this generation is for testing and is probably unnecessary)
            var orderedQuery = collection.OrderBy(item => item);
            var sortedLinkedList = new LinkedList<T>(orderedQuery);

            if (sortedLinkedList.Count == 0)
                return null;

            var firstBaseHead = new Node<T>();
            var currBaseHead = firstBaseHead;

            // Remove the first element in the sorted collection to become the first base head node (by definition)
            currBaseHead.Data = sortedLinkedList.First.Value;
            sortedLinkedList.RemoveFirst();

            // Add as Next to make the loop below simpler, then change the first one to Bottom afterwards
            var currBottom = currBaseHead;
            // Remove a random number of elements -randomly- from the sorted collection to become the list on the Bottom reference of the base head node
            // (Note that all of these elements will be greater than the first base head node, by design)

            // Enumerate through the linked list
            // Advance a random number of steps to an element, copy it to a new element at the bottom of the base head node
            // repeat until walk off the end of the linked list
            // (**Remember to walk the bottom reference to remove those values from the linked list before getting the next batch of values)

            // (Effectively copying a random number of elements from the linked list to the bottom of the curr base head node)
            // Sounds like best practice is to use foreach with a linked list enumerator:
            // https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.getenumerator?view=netcore-3.1



            // HONESTLY, IT WOULD BE BETTER TO DO THIS BY SHUFFLING THE INDEX REPRESENTATIONS, THEN DEALING THE NUMBER YOU WANT
            // THEN ADVANCING TO THEM, FISHER-YATES STYLE
            // SINCE YOU WANT RANDOM UNIQUE RESULTS (WHERE EACH OUTCOME IS EQUALLY LIKELY)

            // TODO: CHECK THIS LINKED LIST TO BOTTOM LOGIC FOR OFF-BY-ONE
            var numberOfNodesToSkip = RandomNumberGenerator.Next(numberOfNodesToSkip, sortedLinkedList.Count);
            var skippingNodesCountdown = numberOfNodesToSkip;
            foreach (var node in sortedLinkedList)
            {
                if (skippingNodesCountdown > 0)
                    --skippingNodesCountdown;
                else
                {
                    currBottom.Next = new Node<T>();
                    currBottom = currBottom.Next;
                    currBottom.Data = node.Value;

                    numberOfNodesToSkip = RandomNumberGenerator.Next(numberOfNodesToSkip, sortedLinkedList.Count);
                }
            }

            // Random number of random elements to take
            // var randomNumberOfBottomElements = RandomNumberGenerator.Next(sortedLinkedList.Count);
            for (var i = randomNumberOfBottomElements; i > 0; --i)
            {
                // Choose that number of elements randomly
                var indexOfElementToMoveFromLinkedListToBottomOfBaseHeadNode = RandomNumberGenerator.Next(sortedLinkedList.Count);
                var elementToMoveFromLinkedListToBottomOfBaseHeadNode = sortedLinkedList.
            }


            // If no elements remain in the sorted collection, the node list is complete
            // (a base head node may have no Bottom node and/or no Next node)

            // Remove the current first element of the sorted collection to become the second base head node
            // Point the Next reference from the first base head node to the second base head node


            // VARIATION:
            // As written, the caller provides any items and the items are sorted and randomly distributed into a valid NodeList
            // Another option (to facilitate testing) may be to also have control about how many base nodes there are
            // and/or min/max on bottom node list length




            // WHAT I WROTE WHILE I WAS ON AUTOPILOT:
            // Dummy node to make loop logic nicer
            // Will chop off before returning
            var dummyNode = new Node<T>();
            var currNode = dummyNode;

            foreach (var item in collection)
            {
                currNode.Next = new Node<T>
                {
                    Data = item,
                };
                currNode = currNode.Next;
            }

            var actualHead = dummyNode.Next;
            return actualHead;





            return head;
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
            var nextBaseHead = head.Next;

            // At minimum, need to do the tear-off for an input of a single linked list
            // (with base having only Bottom node, and no Next node)  

            // dummy starts as "already" torn off/flattened, so the initial list can follow the logic
            // of all other lists
            {
                MergeInBaseHead(prevBaseHeadMerged, currBaseHead);

                prevBaseHeadMerged = currBaseHead;
                currBaseHead = nextBaseHead;
                nextBaseHead = nextBaseHead.Next;
            } while (nextBaseHead != null) ;

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
            // Flatten before merging
            baseHeadToMerge.Next = baseHeadToMerge.Bottom;
            baseHeadToMerge.Bottom = null;

            var currMerged = prevBaseHeadMerged;
            var currToMerge = baseHeadToMerge;
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
                if (currToMerge.Data < currMerged.Next.Data)
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
