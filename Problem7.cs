using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    class Problem7
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntArray = new int[]{ 16, 17, 4, 3, 5, 2},
                    CorrectLeadersString = $"leaders are 17, 5 and 2",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 12, 9, 20, 10, 8, 0, 6, 9, 5, 10, 11},
                    CorrectLeadersString = $"leaders are 20 and 11",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 0, -6, -10, -12, -20, -8, -5},
                    CorrectLeadersString = $"leaders are 0 and -5",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ -20, -5, -20, -10, -5, -10},
                    CorrectLeadersString = $"leaders are -5 and -10",
                },
                new TestCase
                {
                    InputIntArray = new int[]{0},
                    CorrectLeadersString = $"leader is 0",
                },
                new TestCase
                {
                    InputIntArray = new int[]{0, 1, 2, 3, 4, 5, 6},
                    CorrectLeadersString = $"leader is 6",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 4, 3, 1, 2},
                    CorrectLeadersString = $"leaders are 4, 3 and 2",
                },
            };


            string intro =
                "==============\n" +
                "= Problem #7 =\n" +
                "==============\n" +
                "\n" +
                "Write a program to print all the LEADERS in the array." +
                "An element is leader if it is greater than all the elements to its right side." +
                "And the rightmost element is always a leader." +
                "For example int the array { 16, 17, 4, 3, 5, 2}, leaders are 17, 5 and 2.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");


                Console.WriteLine($"For the array: {Utility.CollectionToString(testCases[i].InputIntArray)}");
                Console.WriteLine($"The correct result is: {testCases[i].CorrectLeadersString}.");

                var testCaseResult = PrintLeadersToString(testCases[i].InputIntArray);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectLeadersString)
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}! Your answer is: {testCaseResult}.");
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


        public static string PrintLeadersToString(int[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException("The int[] arr parameter is null.");

            if (arr.Length == 0)
                throw new ArgumentException("The parameter int[] arr is empty.");

            var largestLeader = arr[arr.Length - 1];

            //var leaders = new Queue<int> { largestLeader }; // Can you init a queue in this way? nope.
            var leaders = new Queue<int>();
            leaders.Enqueue(largestLeader);

            // Walk through arr right to left, starting at the the element to the left of the default leader
            for (var i = arr.Length - 2; i >= 0; --i)
            {
                //if (arr[i] > arr[i + 1]) // No, don't compare to the element to the right
                // Compare to the largestLeader
                if (arr[i] > largestLeader)
                {
                    largestLeader = arr[i];
                    leaders.Enqueue(largestLeader);
                }
            }

            return ConstructLeaderResultString(leaders);
        }


        private static string ConstructLeaderResultString(Queue<int> leaders)
        {
            // Example: "leaders are 17, 5 and 2"

            if (leaders.Count == 0)
                throw new ArgumentException("Queue<int> leaders parameter is empty.");

            if (leaders.Count == 1)
                return $"leader is {leaders.Dequeue()}";


            // Build from the right of the string to the left
            var builder = new StringBuilder($" and {leaders.Dequeue()}");

            // Insert at 0, or prepend, whatever the method is for that
            builder.Insert(0, leaders.Dequeue());
            
            // When you Insert into a StringBuilder (probably onto the front of it,
            // as a prepend), the index (probably zero) is the first param, while
            // the string or thing-to-be-stringified is the second param


            while (leaders.Count > 0)
            {
                builder.Insert(0, $"{leaders.Dequeue()}, ");
            }

            builder.Insert(0, "leaders are ");

            return builder.ToString();
        }


        public static void PrintLeaders(int[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException("The int[] arr parameter is null.");

            if (arr.Length == 0)
                throw new ArgumentException("The parameter int[] arr is empty.");

            var largestLeader = arr[arr.Length - 1];

            var leaders = new Queue<int>();
            leaders.Enqueue(largestLeader);

            // Walk through arr right to left,
            // starting at the the element to the left of the default leader
            for (var i = arr.Length - 2; i >= 0; --i)
            {
                // Compare to the largestLeader
                if (arr[i] > largestLeader)
                {
                    largestLeader = arr[i];
                    leaders.Enqueue(largestLeader);
                }
            }

            // Example: "leaders are 17, 5 and 2"
            if (leaders.Count == 1)
                Console.WriteLine($"leader is {leaders.Dequeue()}");
            else
            {
                // Build from the right of the string to the left
                var builder = new StringBuilder($" and {leaders.Dequeue()}");

                // Use Insert at 0 as prepend
                builder.Insert(0, leaders.Dequeue());

                while (leaders.Count > 0)
                {
                    builder.Insert(0, $"{leaders.Dequeue()}, ");
                }

                builder.Insert(0, "leaders are ");

                Console.WriteLine(builder.ToString());
            }
        }


        private class TestCase
        {
            public int[] InputIntArray { get; set; }

            public string CorrectLeadersString { get; set; }
        }
    }
}
