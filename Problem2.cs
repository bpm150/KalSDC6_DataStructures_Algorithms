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
