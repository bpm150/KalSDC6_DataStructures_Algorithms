using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment7
{
    class Problem8
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputNodeList =
                        CreateNodeList(new int[]{ 1, 2, 3, 4, 5, 6, 7, 8, 9, }),
                    CorrectOutput =
                        CreateNodeList(new int[]{ 2, 1, 4, 3, 6, 5, 8, 7, 9, }),
                },
            };

            string intro =
                "==============\n" +
                "= Problem #8 =\n" +
                "==============\n" +
                "\n" +
                "Given a singly linked list, write a function to swap elements " +
                "pairwise.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input:          \"{ (testCases[i].InputNodeList) }\"");
                Console.WriteLine($"Correct output: \"{ testCases[i].CorrectOutput }\"");

                var testCaseResult = SwapPairwise(testCases[i].InputNodeList);

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
            public Node<int> InputNodeList { get; set; }

            public Node<int> CorrectOutput { get; set; }
        }


        public static Node<T> SwapPairwise<T>(Node<T> head)
        {
            if (head == null)
                throw new ArgumentNullException("head is null");

            if (head.Next == null)
                return head;

            var currLeft = head;
            head = head.Next;

            Node<T> nextLeft;

            for (; ; )
            {
                nextLeft = currLeft.Next.Next;
                currLeft.Next.Next = currLeft;
                if (nextLeft == null)
                {
                    currLeft.Next = null;
                    break;
                }
                else if (nextLeft.Next == null)
                {
                    currLeft.Next = nextLeft;
                    break;
                }
                else
                    currLeft.Next = nextLeft.Next;
            }
            return head;
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
    }
}
