using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assignment7
{
    class Problem3
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    K = 3,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 3, 2, 1, 6, 5, 4, 9, 8, 7, }),
                },
                new TestCase
                {
                    K = 4,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6, 7, 8,}),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 4, 3, 2, 1, 8, 7, 6, 5, }),
                },
                new TestCase
                {
                    K = 4,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6, 7, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 4, 3, 2, 1, 7, 6, 5, }),
                },
                new TestCase
                {
                    K = 2,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6,}),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 2, 1, 4, 3, 6, 5, }),
                },
                new TestCase
                {
                    K = 2,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 2, 1, 4, 3, 5, }),
                },
                new TestCase
                {
                    K = 3,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 3, 2, 1, 6, 5, 4, 8, 7, }),
                },
                new TestCase
                {
                    K = 3,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 3, 2, 1, 5, 4,}),
                },
                new TestCase
                {
                    K = 3,
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 3, 2, 1, }),
                },
            };

            string intro =
                "==============\n" +
                "= Problem #3 =\n" +
                "==============\n" +
                "\n" +
                "Given a linked list, write a function to reverse every k nodes, " +
                "where k is an input to the function.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input:          \"{ (testCases[i].InputNodeList) }\"");
                Console.WriteLine($"k == :          \"{ (testCases[i].K) }\"");
                Console.WriteLine($"Correct output: \"{ testCases[i].CorrectOutput }\"");

                var testCaseResult = ReverseEveryKNodes(testCases[i].InputNodeList, testCases[i].K);

                string resultMessage;

                if (testCaseResult.ToString() == testCases[i].CorrectOutput.ToString())
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}! \n" +
                                  $"Your answer is: \"{testCaseResult}\".");
            }

            var testCount = testCases.Count;
            var testSuccessCount = testCount - testOopsCount;

            Console.WriteLine($"\n\nOut of {testCount} tests total,\n");
            Console.WriteLine($"{testSuccessCount}/{testCount} tests succeeded, and");
            Console.WriteLine($"{testOopsCount}/{testCount} tests oopsed.\n");

            if (testOopsCount == 0)
            {
                Console.WriteLine($"YAY! All tests succeeded! :D\n");
            }
        }

        private class TestCase
        {
            public int K { get; set; }

            public Node<int> InputNodeList { get; set; }

            public Node<int> CorrectOutput { get; set; }
        }

        /// <summary>
        /// Reverses the order of the elements in a list in groups of k elements.
        /// </summary>
        /// <param name="head">Head of the list to be reversed.</param>
        /// <param name="k">Length of the sublist groups to reverse.</param>
        /// <returns>Head of the reversed list.</returns>
        public static Node<T> ReverseEveryKNodes<T>(Node<T> head, int k)
        {
            if (head == null)
                throw new ArgumentNullException("head is null");

            if (k < 0)
                throw new ArgumentException("k must be non-neg");

            if (k == 0 || k == 1)
                return head;

            // ? Syntax for declaring an out param in a method call
            // ? Is this relevant here? Think I need the symbol on a future loop iteration
            // Yes, twice even.
            // out Node<T> finalReversedListHead

            // Save off, will stitch onto this node on first loop iteration (if any)
            var currReversedGroupTail = head;
            var nextGroupHeadToReverse = ReverseSublistGroup(head, k, out Node<T> finalReversedListHead);

            while (nextGroupHeadToReverse != null)
            {
                // Will stitch onto this node during this iteration
                var prevReversedGroupTail = currReversedGroupTail;

                // Save off, will stitch onto this node during next iteration (if any)
                currReversedGroupTail = nextGroupHeadToReverse;
                nextGroupHeadToReverse =
                    ReverseSublistGroup(nextGroupHeadToReverse, k, out Node<T> currReversedGroupHead);

                // Where to declare ?currReversedGroupHead
                // We are generating the head of the reversed group
                // and then stitching it on the tail of the prev reversed group
                // during a single loop iteration, so can declare within loop scope

                // Stitch groups together
                prevReversedGroupTail.Next = currReversedGroupHead;
            }

            return finalReversedListHead;
        }





        //    Node<T> newHead = null;

        //    var nextLeftHead = ReverseSublist(head, k, out newHead);
        //    Node<T> revSLHead = null;

        //        while (nextLeftHead != null)
        //        {
        //            // TODO:
        //            // Stitching of the groups together

        //            nextLeftHead = ReverseSublist(nextLeftHead, k, out revSLHead);
        //}


        //        return newHead;


        /// <summary>
        /// Helper method. Reverses a sublist group of a larger list.
        /// </summary>
        /// <param name="inHead">Head of the sublist to be reversed</param>
        /// <param name="k">Length of sublist to be reversed</param>
        /// <param name="outHead">Head of reversed sublist (post-reverse)</param>
        /// <returns>The head of the next sublist group, or null if no next group exists.
        /// (Referred to as nextLeftHead in the calling method.)</returns>
        private static Node<T> ReverseSublistGroup<T>(Node<T> inHead, int k, out Node<T> outHead)
        {
            if (inHead.Next == null)
            {
                outHead = inHead;
                return null;
            }

            var prev = inHead;
            var curr = inHead.Next;
            // VS suggests not setting references to null after declaring them
            // "Unnecessary assignment of a value."
            // null is the default value of a reference.

            // BUG:
            // THIS WAS THE KEY ISSUE IN THIS HELPER:
            // IMPORTANT FOR THE REVERSED SUBLIST TO END WITH NULL...
            // But how to describe why?
            // Does it work to take this back out (no, at minimum Node<T>.ToString depends on it
            // After the "stitiching it together" step in the calling method is complete?
            inHead.Next = null;

            // So Node<T>.ToString infinite loops without this null set
            // Do cases where n > k work with the null set in? Yes.

            for (var i = k - 1; i >= 1 && curr != null; --i)
            {
                var temp = curr.Next;
                curr.Next = prev;
                prev = curr;
                // BUG:
                // FORGOT TO ADVANCE CURR (AND USE THE SAVED TEMP)
                curr = temp;
            }

            outHead = prev;

            //if (curr != null)
            //{
            //    // Either just processed last group of n % k == 0,
            //    // or we're not at last group yet, so don't know yet about n % k

            //    // Will be null iff this was the last group
            //    return curr;
            //}
            //// if curr == null, then this was the last group
            //// and also n % k != 0

            return curr;
        }

        public static Node<T> CreateNodeList<T>(IEnumerable<T> items)
        {
            if (items.Count() == 0)
                return null;

            // VER DONE WITH DUMMY HEAD 
            var dummyHead = new Node<T>();
            //{
            //    // Cannot apply indexing with [] to an expression of type
            //    // IEnumerable<int>
            //    // However, can use ElementAt
            //    //Data = ints[0],
            //    Data = ints.ElementAt(0),
            //};

            var curr = dummyHead;
            foreach (var item in items)
            {
                curr.Next = new Node<T>();
                curr = curr.Next;
                curr.Data = item;
            }

            return dummyHead.Next;

            // VER DONE WITH PREV   
            //var head = new Node<T>();
            ////{
            ////    // Cannot apply indexing with [] to an expression of type
            ////    // IEnumerable<int>
            ////    // However, can use ElementAt
            ////    //Data = ints[0],
            ////    Data = ints.ElementAt(0),
            ////};

            //var curr = head;
            //Node<T> prev = null;
            //foreach (var i in ints)
            //{
            //    curr.Data = i;
            //    curr.Next = new Node<T>();
            //    prev = curr;
            //    curr = curr.Next;
            //}
            //prev.Next = null;
        }

        public class Node<T>
        {
            public T Data;
            public Node<T> Next;

            public override string ToString()
            {
                var sb = new StringBuilder();
                var curr = this;

                do
                {
                    sb.Append($"{curr.Data} -> ");
                    curr = curr.Next;
                } while (curr != null);

                sb.Append("null");

                return sb.ToString();
            }
        }


        //Hrm...what need happen to use Node<T> here?
        // Looks like it is probably as simple as declaring the method itself to be generic on T
        //private static Node<T> ReverseSublist<T>(Node<T> inHead, int k, out Node<T> outHead)
        //{
        //    outHead = default;

        //    return default;
        //}

        //private class Node<T>
        //{
        //    public T Data;
        //    public Node<T> Next;
        //}
    }
}
