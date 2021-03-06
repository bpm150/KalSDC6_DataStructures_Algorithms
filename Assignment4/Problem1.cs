﻿using DataStructuresAndAlgos;
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
                    testArray = new int[]{10, -5, 20, 10, -20, 5},
                    largestSum = 35,
                },
                new TestCase
                {
                    testArray = new int[]{0},
                    largestSum = 0,
                },
                new TestCase
                {
                    testArray = new int[]{-10},
                    largestSum = -10,
                },
                new TestCase
                {
                    testArray = new int[]{3},
                    largestSum = 3,
                },
                new TestCase
                {
                    testArray = new int[]{ 2, 1, 4, 5, 3},
                    largestSum = 15,
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
                    testArray = new int[]{ -10, 500, -10, 500, -10, 500, -10, 500, -5000, 1000 },
                    largestSum = 1970,
                },
                new TestCase
                {
                    testArray = new int[]{ 20, -10, 50, -10, 40, -80, 60, -40, 10, -30, 60, -40, 10, -60 },
                    largestSum = 90,
                },
                new TestCase
                {
                    testArray = new int[]{ 10, 5, 5, -5, -5, 10, 0, 40, -10, 20, 20, -60, -20, 30, 10, 20, -5, -5, -30, 10, -30, 30, 30, -10, -30, 5, 5, -10, -50 },
                    largestSum = 90,
                },  
            };


            string intro =
                "==============\n" +
                "= Problem #1 =\n" +
                "==============\n" +
                "\n" +
                "Write an efficient program to find the sum of contiguous subarray " +
                "within a one-dimensional array of numbers which has the largest sum.";

            Console.WriteLine(intro);


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
            if (arr == null)
            {
                throw new ArgumentNullException("Parameter int[] arr is null.");
            }

            if (arr.Length == 0)
            {
                throw new ArgumentException("Parameter int[] arr is an empty array.");
            }

            long currSum = arr[0];
            var largestValue = arr[0];
            var groupSums = new List<long>();

            // Walk arr and "collapse" adjacent values of same sign (or zero)
            // into summed groups (delimited on change of sign of elements in arr)
            for (var i = 1; i < arr.Length; ++i)
            {
                // largestValue will be the final result in the event that only
                // non-positive values exist in arr
                largestValue = arr[i] > largestValue ? arr[i] : largestValue;

                // Detect sign change from currSum to arr[i]
                if ((currSum > 0 && arr[i] < 0) || (currSum < 0 && arr[i] > 0))
                {
                    // Don't add a negative group if groupSums is empty. (Discard it)
                    // Prevents doing a RemoveAt call later that has O(n) time complexity 
                    // Helps keep main loop (coming up) tidy
                    if (groupSums.Count > 0 || currSum > 0)
                        groupSums.Add(currSum);

                    // Begin accumulation of values with the new sign
                    currSum = arr[i];
                }
                // (DONE) Better way to detect sign change that doesn't involve multiplication?
                // Is the sign of the current element the same as the current sum we're working on?
                // (Zero is considered to be the same sign as everything for this purpose, that is, not a change)
                //if (!(currSum * arr[i] < 0))
                else // Sign not changed
                {
                    currSum += arr[i];
                }
            }

            // The final group whose sum is waiting in currSum
            // Don't add it if it is negative
            if (currSum > 0)
            {
                groupSums.Add(currSum);

            }


            // Ok, now the List of groups is all made
            // Possibilities are:
            // {} (empty set)
            // {+}
            // {+, -, +}
            // {+, -, +, -, +}
            // etc.
            // If groupSums contains any groups,
            // It begins with a positive group an ends with a positive group


            // This handles all cases where the final result is negative
            // (Negative groups are only ever added in-between positive groups)
            // So, if no positive groups are added, no negative groups are added
            if (groupSums.Count == 0)
                return largestValue;
            // In that case, the single largest element in arr is the largest sum
            // This would be a negative number (the "least negative" number, or zero)


            else if (groupSums.Count == 1)
            {
                // We were using longs to avoid overflowing during intermediate
                // calculations, but per Kal, the return type for this method
                // is to be int so, if the result doesn't fit in an int
                if (groupSums[0] != ((int)groupSums[0]))
                    throw new OverflowException(
                        "Result does not fit in an int.");

                return (int)groupSums[0];
            }

            // Contains best candidate so far for final largest sum result
            // Gets replaced each time we find a better one
            long largestSumResult = 0;
            
            // Running total of summed groups in the current "combo"
            // Gets reset when we find a number so negative that it
            // Offsets all of the benefit of the currently accumulated sum of groups
            long currSumOfGroups = 0;

            // groupSums alternates in sign +, -, +, -
            // Walk groupSums pointing i to the + of each +/- pair
            // Final loop iteration is the next-to-last pair
            // (Last pair and the solo pos group at the end are handled seperately after)
            for (var i = 0; i <= groupSums.Count - 5; i+=2)
            {
                // Net effect of the +/- pair
                var sumOfPosNegPair = groupSums[i] + groupSums[i + 1];

                if (sumOfPosNegPair > 0) // Combo continues
                {
                    currSumOfGroups += sumOfPosNegPair;

                    largestSumResult =
                        currSumOfGroups > largestSumResult ?
                            currSumOfGroups : largestSumResult;
                }
                else // Broken combo
                {
                    // Found a negative value so negative
                    // (compared to the positive values before it)
                    // that there's no way the largest sum could span it

                    // Only add the positive value from the current +/- pair
                    currSumOfGroups += groupSums[i];
                    // Negative group is discarded

                    largestSumResult =
                        currSumOfGroups > largestSumResult ?
                            currSumOfGroups : largestSumResult;

                    // Reset for start of next combo
                    currSumOfGroups = 0;
                }
            }

            // Three groups left: {+, -, +}
            // Add the + group unconditionally

            currSumOfGroups += groupSums[^3];

            largestSumResult =
                currSumOfGroups > largestSumResult ?
                    currSumOfGroups : largestSumResult;


            // Add the final (inverted) pair {-, +} if it is worth taking
            if (groupSums[^1] + groupSums[^2] > 0)
            {
                currSumOfGroups += groupSums[^1] + groupSums[^2];

                largestSumResult =
                    currSumOfGroups > largestSumResult ?
                        currSumOfGroups : largestSumResult;
            }

            // We were using longs to avoid overflowing during intermediate
            // calculations, but per Kal, the return type for this method
            // is to be int so, if the result doesn't fit in an int
            if (largestSumResult != ((int)largestSumResult))
                throw new OverflowException(
                    "Result does not fit in an int.");

            return (int)largestSumResult;
        }






        // THIS INCORRECT SOLUTION
        // Incorrectly assumes that the largest positive group will be in the largest sum
        // This is not true:
        // -10, 500, -10, 500, -10, 500, -10, 500, -5000, 1000
        // The largest positive group (and thus, also the largest group) is 1000
        // But the largest sum is 1970 and does not contain the largest group
        public static int ANOTHER_INCORRECT_SOLUTION_GetLargestSumOfContiguousSubarray(int[] arr)
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

        public static int INCORRECT_DESIGN_LargestSumOfSubarray(int[] arr)
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
