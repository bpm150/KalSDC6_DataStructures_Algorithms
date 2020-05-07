using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    public class TestCase
    {
        //public TestCase(int[] arr, int sum)
        //{
        //    testArray = arr; // Need to read up on C# arrays...do I need to allocate for this array?
        //    largestSum = sum;
        //}

        public int[] testArray { get; set; }
        public int largestSum { get; set; }
    }


class Program
    {
        static void Main(string[] args)
        {
            //Test_ArrayToString();

            Problem1();



        }


        private static void Problem1()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    testArray = new int[]{ 2, 1, 4, 5, 3},
                    largestSum = 15, // Assumes that full array is considered a valid subarray of itself
                },
                new TestCase
                {
                    testArray = new int[]{ -1, -5, -4, -3, -2},
                    largestSum = -1,
                },
                new TestCase
                {
                    testArray = new int[]{ -3, -5, -4, -1, -2},
                    largestSum = -1,
                },
                new TestCase
                {
                    testArray = new int[]{ 0, -100, 0, 10, -200, 10},
                    largestSum = 10,
                },
                new TestCase
                {
                    testArray = new int[]{ 10, 10, -20, 10, 10, 10, -40, 0, 10, 10, 0},
                    largestSum = 30, 
                },
            };

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"Test #{i+1}:");

                var testCaseResult = LargestSumOfSubarray(testCases[i].testArray);

                Console.WriteLine($"For the array: {ArrayToString(testCases[i].testArray)}");
                Console.WriteLine($"The largest sum of a contiguous subarray is {testCases[i].largestSum}.");

                var resultMessage = testCaseResult == testCases[i].largestSum ? "SUCCESS" : "OOPS";

                Console.WriteLine($"{resultMessage}! Your answer is {testCaseResult}.\n");

                // Apparently, cannot nest string literals within string interpolation?
            }

        }





        public static int LargestSumOfSubarray(int[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException();

            int length = arr.Length;

            if (length == 0)
                return 0;

            if (length == 1)
                return arr[0];

            int currSum = arr[0];

            int largestSum = currSum;

            for (int i = 1; i < length; ++i)
            {
                // Algebra:
                //if (currSum + arr[i] > 0)
                //if (currSum + arr[i] - arr[i] > 0 - arr[i])
                //if (currSum > -arr[i])
                if (currSum > arr[i] * -1)
                {
                    ThrowIfSumWillOverflow(currSum, arr[i]);
                    currSum += arr[i];
                }
                else if (currSum > largestSum)
                    largestSum = currSum;
                else
                    currSum = 0;
            }

            return currSum;
        }

        // Try these with zeroes in the permutations
        // How might that impact a refactor
        // Strict > and < are relevant, since:
        // if a is zero, there is no chance of overflow/underflow
        // regardless of the value of b, (and vice versa)
        public static void ThrowIfSumWillOverflow(int a, int b)
        {
            // Potential for a + b to overflow int.MaxValue
            if (a > 0 && b > 0)
            {
                // Algebra:
                //if (a + b > int.MaxValue )
                //if (a + b - b > int.MaxValue - b)
                if (a > int.MaxValue - b)
                    throw new OverflowException();
            }
            // Potential for a + b to underflow int.MinValue
            else if (a < 0 && b < 0)
            {
                if (a < int.MinValue - b)                           // Concrete example to see the logic play out on this one
                    throw new OverflowException();
            }

            return;
        }



        public static string ArrayToString<T>(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (array.Length == 0)
            {
                return "{ }";
                // Avoids awkwardly removing an extra space later
            }

            var builder = new StringBuilder("{ ");

            foreach (var item in array)
            {
                builder.Append($"{item}, ");
            }

            var length = builder.Length;
            builder.Remove(length - 2, 2).Append(" }");

            return builder.ToString();
        }


        private static void Test_ArrayToString()
        {
            // Test ArrayToString for
            // null
            // length == 0
            // length == 1
            // length == 2(+)



            // (Permanently) empty array
            // Arrays cannot change size
            // Empty array in C# is as close as you can get to an immutable collection
            // in C#
            // Can use the same instance for all purposes, as you can with String.Empty

            var intArray = new int[] { };
            Console.WriteLine(ArrayToString(intArray));

            intArray = new int[] { 42 };
            Console.WriteLine(ArrayToString(intArray));

            intArray = new int[] { 7, 11 };
            Console.WriteLine(ArrayToString(intArray));

            intArray = null;
            //Console.WriteLine(ArrayToString(intArray));

        }
    }
}
