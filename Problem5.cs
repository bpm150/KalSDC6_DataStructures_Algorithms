using System;
using System.Collections.Generic;
using System.Linq; // For sequence equal
using System.Text;

namespace Assignment4
{
    class Problem5
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputArray =  new int[]{1,2,1,2,2,0,1,2,0,0,0},
                    OutputArray = new int[]{0,0,0,0,1,1,1,2,2,2,2},
                },
                new TestCase
                {
                    InputArray =  new int[]{0,0,0},
                    OutputArray = new int[]{0,0,0},
                },
                new TestCase
                {
                    InputArray =  new int[]{1,1,1},
                    OutputArray = new int[]{1,1,1},
                },
                new TestCase
                {
                    InputArray =  new int[]{2,2,2},
                    OutputArray = new int[]{2,2,2},
                },
                new TestCase
                {
                    InputArray =  new int[]{0,0,0,1,0,1,2,2,2},
                    OutputArray = new int[]{0,0,0,0,1,1,2,2,2},
                },
                new TestCase
                {
                    InputArray =  new int[]{1,2,0,2,0},
                    OutputArray = new int[]{0,0,1,2,2},
                },
            };


            string intro =
                "==============\n" +
                "= Problem #5 =\n" +
                "==============\n" +
                "\n" +
                "Write a program to sort an array of 0's, 1's and 2's in ascending order.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                var listCount = testCases[i].InputArray.Length;

                Console.WriteLine($"Input: arr[]      = {Utility.CollectionToString(testCases[i].InputArray)}");
                
                var testCaseResult = SortArrayOf0s1s2s(testCases[i].InputArray);

                Console.WriteLine($"Output: sortedArr = {Utility.CollectionToString(testCaseResult)}");

                string resultMessage;
                if ( testCaseResult.SequenceEqual(testCases[i].OutputArray) )
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}!");
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



        public static int[] SortArrayOf0s1s2s(int[] arr)
        {
            // TODO: COME BACK AND HANDLE TRIVIAL CASES

            var outputArr = new int[arr.Length];
            var leftIndex = 0;
            var rightIndex = arr.Length - 1;

            for (var i = 0; i < arr.Length; ++i)
            {
                if (arr[i] == 0)
                    // put it at outputArr[leftIndex]
                    // Actually don't have to do this as 0 is the default value for int
                    ++leftIndex; // But still need to advance leftIndex
                                 // In the case of all 0s, leftIndex *will* walk one step off the end of arr (to leftIndex == arr.Length)
                                 // This is not an issue, because in that case we won't have any 1s to place later.
                else if (arr[i] == 2)
                    outputArr[rightIndex--] = 2;
                // In the case of all 2s, rightIndex will walk one step off the front of arr (to rightIndex == -1)
                // This is not an issue, because in that case we won't have any 1s to place later.
                // We don't need to do anything if arr[i] == 1, we handle that next
            }

            // Finally, fill in the space between the 0s and the 2s with 1s
            // if leftIndex <= rightIndex, there is at least one 1 to place
            for (var k = leftIndex; k <= rightIndex; ++k)
            {
                outputArr[k] = 1;
            }

            return outputArr;
        }


        private class TestCase
        {
            public int[] InputArray { get; set; }
            public int[] OutputArray { get; set; }
        }
    }
}
