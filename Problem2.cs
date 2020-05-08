using System;
using System.Collections.Generic;

namespace Assignment4
{
    public static class Problem2
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    intList = new List<int>{ 2 },
                    missingInt = 1,
                },
                new TestCase
                {
                    intList = new List<int>{ 1, 3 },
                    missingInt = 2,
                },
                new TestCase
                {
                    intList = new List<int>{ 3, 2, 1 },
                    missingInt = 4,
                },
                new TestCase
                {
                    intList = new List<int>{ 4, 1, 5, 3 },
                    missingInt = 2,
                },
                new TestCase
                {
                    intList = new List<int>{ 6, 3, 4, 2, 1 },
                    missingInt = 5,
                },
                new TestCase
                {
                    intList = new List<int>{ 5, 2, 4, 3, 6, 7 },
                    missingInt = 1,
                },
            };


            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"Test #{i + 1}:");

                var testCaseResult = FindMissingIntegerFromUnique(testCases[i].intList);

                Console.WriteLine($"For the list: {Utility.CollectionToString(testCases[i].intList)}");
                Console.WriteLine($"The the missing integer is {testCases[i].missingInt}.");

                var resultMessage = testCaseResult == testCases[i].missingInt ? "SUCCESS" : "OOPS";

                Console.WriteLine($"{resultMessage}! Your answer is {testCaseResult}.\n");
            }

        }

        public static int FindMissingIntegerFromUnique(List<int> intList)
        {
            var n = intList.Count + 1;

            // Triangluar numbers formula
            var sumOfRange = (n * (n + 1)) / 2;
            // I can see why >> 2 doesn't work on the inside, but why doesn't it work on the outside?

            foreach (var num in intList)
                sumOfRange -= num;

            return sumOfRange;
        }

        private class TestCase
        {
            //public TestCase(int[] arr, int sum)
            //{
            //    testArray = arr; // Need to read up on C# arrays...do I need to allocate for this array?
            //    largestSum = sum;
            //}

            public List<int> intList { get; set; }
            public int missingInt { get; set; }
        }

    }
}
