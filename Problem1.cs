using System;
using System.Collections.Generic;

namespace Assignment4
{
    public static class Problem1
    {
        public static void RunTests()
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
                new TestCase
                {
                    testArray = new int[]{ 20, -10, 50, -10, 40, -80, 60, -40, 10, -30, 60, -40, 10, -60 },
                    largestSum = 70,
                },
                new TestCase
                {
                    testArray = new int[]{ 10, 5, 5, -5, -5, 10, 0, 40, -10, 20, 20, -60, -20, 30, 10, 20, -5, -5, -30, 10, -30, 30, 30, -10, -30, 5, 5, -10, -50 },
                    largestSum = 70,
                },
            };

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"Test #{i + 1}:");

                var testCaseResult = GetLargestSumOfContiguousSubarray(testCases[i].testArray);

                Console.WriteLine($"For the array: {Utility.CollectionToString(testCases[i].testArray)}");
                Console.WriteLine($"The largest sum of a contiguous subarray is {testCases[i].largestSum}.");

                var resultMessage = testCaseResult == testCases[i].largestSum ? "SUCCESS" : "OOPS";

                Console.WriteLine($"{resultMessage}! Your answer is {testCaseResult}.\n");

                // Apparently, cannot nest string literals within string interpolation?
            }

        }


        public static int GetLargestSumOfContiguousSubarray(int[] arr)
        {
            // Try typing code exactly as I've written it and see if it works
            // as well in scripted testing as it did by hand

            if (arr == null)
            {
                throw new ArgumentNullException("Parameter int[] arr is null.");
            }
            if (arr.Length == 0)
            {
                throw new ArgumentException("Parameter int[] arr is an empty array.");
            }
            if (arr.Length == 1)
            {
                return arr[0];
            }

            // Prepare for more complex would-be branching return later on
            //int? largestSumOfContiguousSubarray = null;

            var currSum = arr[0];
            var largestValue = arr[0];
            var groupSums = new List<int>();
            int? largestGroupSum = null;
            int? largestGroupSumIndex = null;

            for (var i = 1; i < arr.Length; ++i)
            {
                largestValue = (arr[i] > largestValue) ? arr[i] : largestValue;

                if (!(currSum * arr[i] < 0))
                {
                    currSum += arr[i];
                }
                else
                {
                    groupSums.Add(currSum);

                    if (currSum > largestGroupSum.GetValueOrDefault())
                    {
                        largestGroupSum = currSum;
                        largestGroupSumIndex = groupSums.Count - 1;
                    }

                    currSum = arr[i];
                }
            }

            groupSums.Add(currSum);
            if (currSum > largestGroupSum.GetValueOrDefault())
            {
                largestGroupSum = currSum;
                largestGroupSumIndex = groupSums.Count - 1;

                // When two sum groups are tied for largest sum
                // The group at the lowest index is always used,
                // Though any of them would work for the rest of the logic
            }


            if (groupSums.Count == 1)
            {
                if (groupSums[0] > 0)
                    return groupSums[0];
                else
                    return largestValue;
            }
            else
            {
                int finalSumAccum = largestGroupSum.GetValueOrDefault();


                // Include any sum groups to the right worth including

                int currRightGroupSum = 0;
                int? largestRightGroupSum = null;
                // Only nullable for readability so that the symbol
                // best represents its name. 
                // Optionally could remove nullability if performance delta matters

                for (var r = largestGroupSumIndex.Value + 1; r < groupSums.Count; ++r)
                {
                    currRightGroupSum += groupSums[r];

                    if (currRightGroupSum > largestRightGroupSum.GetValueOrDefault())
                        largestRightGroupSum = currRightGroupSum;
                }

                if (largestRightGroupSum.GetValueOrDefault() > 0)
                    finalSumAccum += largestRightGroupSum.Value;



                // Include any sum groups to the left worth including

                int currLeftGroupSum = 0;
                int? largestLeftGroupSum = null;
                // Again nullability optional

                for (var l = largestGroupSumIndex.Value - 1; l >= 0; --l)
                {
                    currLeftGroupSum += groupSums[l];

                    if (currLeftGroupSum > largestLeftGroupSum.GetValueOrDefault())
                        largestLeftGroupSum = currLeftGroupSum;
                }

                if (largestLeftGroupSum.GetValueOrDefault() > 0)
                    finalSumAccum += largestLeftGroupSum.Value;



                return finalSumAccum;
            }


            //Test #1:
            //For the array: { 2, 1, 4, 5, 3 }
            //The largest sum of a contiguous subarray is 15.
            //SUCCESS! Your answer is 15.

            //Test #2:
            //For the array: { -1, -5, -4, -3, -2 }
            //The largest sum of a contiguous subarray is -1.
            //SUCCESS! Your answer is -1.

            //Test #3:
            //For the array: { -3, -5, -4, -1, -2 }
            //The largest sum of a contiguous subarray is -1.
            //SUCCESS! Your answer is -1.

            //Test #4:
            //For the array: { 0, -100, 0, 10, -200, 10 }
            //The largest sum of a contiguous subarray is 10.
            //SUCCESS! Your answer is 10.

            //Test #5:
            //For the array: { 10, 10, -20, 10, 10, 10, -40, 0, 10, 10, 0 }
            //The largest sum of a contiguous subarray is 30.
            //SUCCESS! Your answer is 30.

            //Test #6:
            //For the array: { 20, -10, 50, -10, 40, -80, 60, -40, 10, -30, 60, -40, 10, -60 }
            //The largest sum of a contiguous subarray is 70.
            //SUCCESS! Your answer is 70.

            //Test #7:
            //For the array: { 10, 5, 5, -5, -5, 10, 0, 40, -10, 20, 20, -60, -20, 30, 10, 20, -5, -5, -30, 10, -30, 30, 30, -10, -30, 5, 5, -10, -50 }
            //The largest sum of a contiguous subarray is 70.
            //SUCCESS! Your answer is 70.
        }


        private class TestCase
        {
            //public TestCase(int[] arr, int sum)
            //{
            //    testArray = arr; // Need to read up on C# arrays...do I need to allocate for this array?
            //    largestSum = sum;
            //}

            public int[] testArray { get; set; }
            public int largestSum { get; set; }
        }

        public static int IncorrectDesign_LargestSumOfSubarray(int[] arr)
        {
            // THE DESIGN OF THIS SOLUTION IS INCORRECT.

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

            return largestSum;
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
    }
}
