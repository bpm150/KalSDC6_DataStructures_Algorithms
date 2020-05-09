using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    class Problem3
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntArray = new int[]{1, 4, 20, 3, 10, 5},
                    InputSum = 33,
                    OutputArrayIndexes = $"Sum found between indexes 2 and 4",
                },
                new TestCase
                {
                    InputIntArray = new int[]{1, 4, 0, 0, 3, 10, 5},
                    InputSum = 7,
                    OutputArrayIndexes = $"Sum found between indexes 1 and 4",
                },
                new TestCase
                {
                    InputIntArray = new int[]{1, 4},
                    InputSum = 0,
                    OutputArrayIndexes = $"No subarray found",
                },
                new TestCase
                {
                    InputIntArray = new int[]{9, 4, 2, 9, 1, 11, 34},
                    InputSum = 1,
                    OutputArrayIndexes = $"Sum found at index 4",
                },
                new TestCase
                {
                    InputIntArray = new int[]{5, 4, 20, 3, 10, 0},
                    InputSum = 33,
                    OutputArrayIndexes = $"Sum found between indexes 2 and 4",
                },
                new TestCase
                {
                    InputIntArray = new int[]{4, 0, 1},
                    InputSum = 0,
                    OutputArrayIndexes = $"Sum found at index 1",
                },
            };

            #region better test cases
            //var testCases = new List<TestCase>
            //{
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{1, 4, 20, 3, 10, 5},
            //        InputSum = tempSum = 33,
            //        OutputArrayIndexes = $"Sum {tempSum} found between indexes 2 and 4",
            //    },
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{1, 4, 0, 0, 3, 10, 5},
            //        InputSum = tempSum = 7,
            //        OutputArrayIndexes = $"Sum {tempSum} found between indexes 1 and 4",
            //    },
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{1, 4},
            //        InputSum = tempSum = 0,
            //        OutputArrayIndexes = $"No subarray found for {tempSum}",
            //    },
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{9, 4, 2, 9, 1, 11, 34},
            //        InputSum = tempSum = 1,
            //        OutputArrayIndexes = $"Sum {tempSum} found at index 4",
            //    },
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{5, 4, 20, 3, 10, 0},
            //        InputSum = tempSum = 33,
            //        OutputArrayIndexes = $"Sum {tempSum} found between indexes 2 and 4",
            //    },
            //    new TestCase
            //    {
            //        InputIntArray = new int[]{4, 0, 1},
            //        InputSum = tempSum = 0,
            //        OutputArrayIndexes = $"Sum {tempSum} found between indexes 2 and 4",
            //    },
            //};
            #endregion

            string intro =
                "==============\n" +
                "= Problem #3 =\n" +
                "==============\n" +
                "\n" +
                "Given an unsorted array of nonnegative integers,\n" +
                "find a continous subarray which adds to a given number..";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                var listCount = testCases[i].InputIntArray.Length;

                Console.WriteLine($"Input: arr[] = {Utility.CollectionToString(testCases[i].InputIntArray)}");
                Console.WriteLine($"Sum = { testCases[i].InputSum }");
                Console.WriteLine($"Output: {testCases[i].OutputArrayIndexes}.");

                var testCaseResult = FindSumInContinuousSubarray(testCases[i].InputIntArray, testCases[i].InputSum);

                string resultMessage;

                if (testCaseResult == testCases[i].OutputArrayIndexes)
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}! Your answer is {testCaseResult}.");
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


        public static string FindSumInContinuousSubarray(int[] arr, int targetSum)
        {
            if (arr == null)
                throw new ArgumentNullException("Parameter int[] arr is null");

            if (arr.Length == 0)
                return ConstructSumResultString(null);

            if (targetSum < 0)
                return ConstructSumResultString(null);

            var leftIndex = 0;
            var rightIndex = 0;

            long sumAccum = arr[0];

            while (true)
            {
                if (sumAccum == targetSum)
                    return ConstructSumResultString(leftIndex, rightIndex);

                if (sumAccum < targetSum)
                {
                    ++rightIndex;
                    if (rightIndex >= arr.Length)
                        return ConstructSumResultString(null);
                    sumAccum += arr[rightIndex];
                }
                else // sumAccum > targetSum
                {
                    ++leftIndex;
                    if (leftIndex >= arr.Length)
                        return ConstructSumResultString(null);
                    sumAccum -= arr[leftIndex - 1];

                    if (leftIndex > rightIndex)
                    {
                        ++rightIndex;
                        sumAccum += arr[rightIndex];
                    }
                }
            }
            // TODO: Throw an exception if somehow execution reaches here
        }

        public static string ConstructSumResultString(int? leftIndex = null, int? rightIndex = null)
        {
            if (leftIndex == null || rightIndex == null)
                return "No subarray found";
            else if (leftIndex.Value == rightIndex.Value)
                return $"Sum found at index {leftIndex.Value}";
            else
                return $"Sum found between indexes {leftIndex.Value} and {rightIndex.Value}";
        }

        private class TestCase
        {
            public int[] InputIntArray { get; set; }

            public int InputSum { get; set; }

            public string OutputArrayIndexes { get; set; }
        }

    }
}
