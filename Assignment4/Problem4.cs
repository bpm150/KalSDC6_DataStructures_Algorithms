using DataStructuresAndAlgos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    class Problem4
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                //new TestCase
                //{
                //    InputIntArray = new int[]{1, 4, 20, 3, 10, 5},
                //    InputSum = 33,
                //    OutputArrayIndexes = $"Sum found between indexes 2 and 4",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{10, 2, -2, -20, 10},
                //    InputSum = -10,
                //    OutputArrayIndexes = $"Sum found between indexes 0 and 3",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{-10, 0, 2, -2, -20, 10},
                //    InputSum = 20,
                //    OutputArrayIndexes = $"No subarray with given sum exists",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 5, 10, 2 },
                //    InputSum = 2,
                //    OutputArrayIndexes = $"Sum found at index 2",
                //},
                new TestCase
                {
                    InputIntArray = new int[]{ 5, 10, -2, 0},
                    InputSum = 8,
                    OutputArrayIndexes = $"Sum found between indexes 1 and 2",
                },
                // CAREFUL! THESE OLD TEST CASES BELOW (FROM PROBLEM 3) HAVE THE OLD WORDING
                //new TestCase
                //{
                //    InputIntArray = new int[]{1, 4, 20, 3, 10, 5},
                //    InputSum = 33,
                //    OutputArrayIndexes = $"Sum found between indexes 2 and 4",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{1, 4, 0, 0, 3, 10, 5},
                //    InputSum = 7,
                //    OutputArrayIndexes = $"Sum found between indexes 1 and 4",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{1, 4},
                //    InputSum = 0,
                //    OutputArrayIndexes = $"No subarray found",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{9, 4, 2, 9, 1, 11, 34},
                //    InputSum = 1,
                //    OutputArrayIndexes = $"Sum found at index 4",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{5, 4, 20, 3, 10, 0},
                //    InputSum = 33,
                //    OutputArrayIndexes = $"Sum found between indexes 2 and 4",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{4, 0, 1},
                //    InputSum = 0,
                //    OutputArrayIndexes = $"Sum found at index 1",
                //},
            };


            string intro =
                "==============\n" +
                "= Problem #4 =\n" +
                "==============\n" +
                "\n" +
                "Given an unsorted array of integers, find a subarray which adds to a given number.\n" +
                "If there are more than one subarrays with sum as the given number, print any of them.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                var listCount = testCases[i].InputIntArray.Length;

                Console.WriteLine($"Input: arr[] = {Utility.CollectionToString(testCases[i].InputIntArray)}");
                Console.WriteLine($"Sum = { testCases[i].InputSum }");
                Console.WriteLine($"Output: {testCases[i].OutputArrayIndexes}.");

                var testCaseResult = FindSubarrayWithTargetSum(testCases[i].InputIntArray, testCases[i].InputSum);

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


        public static string FindSubarrayWithTargetSum(int[] arr, int targetSum)
        {
            if (arr == null)
                throw new ArgumentNullException("Parameter int[] arr is null");

            if (arr.Length == 0)
                return ConstructSumResultString(null);



            if (arr[0] == targetSum)
                return ConstructSumResultString(0, 0);

            var sumArr = new int[arr.Length];
            sumArr[0] = arr[0];

            // Populate sumArr with initial sums
            // Find sums beginning wtih arr[0]
            // also all sums of a single element
            for (var k = 1; k < sumArr.Length; ++k)
            {
                if (arr[k] == targetSum)
                    return ConstructSumResultString(k, k);
                sumArr[k] = sumArr[k - 1] + arr[k];

                if (sumArr[k] == targetSum)
                    return ConstructSumResultString(0, k);
            }

            // Now find sums beginning with all other elements
            // Adjust previous sums by the passed-over (and ruled-out)
            // element, thus leveraging past sum work
            for (var i = 1; i < sumArr.Length; ++i)
            {
                for (var j = i + 1; j < sumArr.Length; ++j)
                {
                    sumArr[j] -= arr[i - 1];

                    if (sumArr[j] == targetSum)
                        return ConstructSumResultString(i, j);
                }
            }

            return ConstructSumResultString(null);
        }

        public static string ConstructSumResultString(int? leftIndex, int? rightIndex = null)
        {
            if (leftIndex == null || rightIndex == null)
                return "No subarray with given sum exists";
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
