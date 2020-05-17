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
                new TestCase
                {
                    intList = new List<int>{ 3, 4, 1 },
                    missingInt = 2,
                },
            };

            string intro =
                "==============\n" +
                "= Problem #2 =\n" +
                "==============\n" +
                "\n" +
                "You are given a list of n-1 integers and these integers are in the range of 1 to n.\n" +
                "There are no duplicates in list.\n" +
                "One of the integers is missing in the list.\n" +
                "Write an efficient code to find the missing integer.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                var listCount = testCases[i].intList.Count;

                Console.WriteLine($"For the list: {Utility.CollectionToString(testCases[i].intList)}");
                Console.WriteLine($"n == { listCount + 1 }");
                Console.WriteLine($"n-1 == { listCount }");
                Console.WriteLine($"The the missing integer is {testCases[i].missingInt}.");

                var testCaseResult = FindMissingInt(testCases[i].intList);

                string resultMessage;

                if (testCaseResult == testCases[i].missingInt)
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

        public static int OVERFLOW_RISK_FindMissingInteger(List<int> intList)
        {
            var n = intList.Count + 1;

            // Triangluar numbers formula
            // https://en.wikipedia.org/wiki/Triangular_number
            var sumOfAllPossibleValues = (n * (n + 1)) / 2;
            // I can see why >> 2 doesn't work on the inside, but why doesn't it work on the outside?

            foreach (var num in intList)
                sumOfAllPossibleValues -= num;

            // All that's left after subtracting the found integers is
            // a value equal to the missing one
            return sumOfAllPossibleValues;

            // Solved in linear time complexity: O(n)
            // and in constant space complexity: O(1)
            //...which, for algorithms, is pretty much the best you can hope for!
        }

        public static int FindMissingInt(List<int> intList)
        {
            var accum = 0;

            for (var i = 0; i < intList.Count; ++i)
            {
                accum += (i + 1) - intList[i];
            }

            var lastFactor = intList.Count + 1;

            return accum + lastFactor;
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
