using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataStructuresAndAlgos;

namespace Assignment4
{
    class Problem10
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputArray         = new int[]{ 2, 5, 2, 8, 5, 6, 8, 8 },
                    CorrectOutputArray = new int[]{ 8, 8, 8, 2, 2, 5, 5, 6 },
                },
                new TestCase
                {
                    InputArray         = new int[]{ 2, 5, 2, 6, -1, 9999999, 5, 8, 8, 8 },
                    CorrectOutputArray = new int[]{ 8, 8, 8, 2, 2, 5, 5, 6, -1, 9999999 },
                },
                new TestCase
                {
                    InputArray         = new int[]{ 2, 5, 2, 8, 5, 6, 8, 8 },
                    CorrectOutputArray = new int[]{ 8, 8, 8, 2, 2, 5, 5, 6 },
                },
                new TestCase
                {
                    InputArray         = new int[]{ 9, 4, 5, 7, 5, 5, 9 },
                    CorrectOutputArray = new int[]{ 5, 5, 5, 9, 9, 4, 7 },
                },
            };

            string intro =
                "==============\n" +
                "= Problem #10 =\n" +
                "==============\n" +
                "\n" +
                "Print the elements of an array in the decreasing frequency if " +
                "2 numbers have same frequency then print the one which came first.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                var listCount = testCases[i].InputArray.Length;

                Console.WriteLine($"Input:          arr[] = {Utility.CollectionToString(testCases[i].InputArray)}");
                Console.WriteLine($"Correct output: arr[] = {Utility.CollectionToString(testCases[i].CorrectOutputArray)}");

                
                var testCaseResult = PrintElementsByFrequencyDecreasing(testCases[i].InputArray);

                string resultMessage;
                // SequenceEqual is a linq query. Requires "using System.Linq;"
                if (testCaseResult.SequenceEqual(Utility.CollectionToString(testCases[i].CorrectOutputArray)))
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}!");
                Console.WriteLine($"Your answer is: arr[] = {testCaseResult}");

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








        // WAS BUG: Forgot to put the return type
        public static string PrintElementsByFrequencyDecreasing(int[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException("Parameter int[] arr is null.");

            if (arr.Length == 0)
                return "{ }";

            var dict = new Dictionary<int, NumberStats>();

            for (var i = 0; i < arr.Length; ++i)
            {
                if (dict.ContainsKey(arr[i]))
                    // WAS BUG: Cannot use ++ (prefix or postfix) with the return value of a dictionary lookup
                    // return value of a dictionary lookup is not an lvalue (apparently)
                    // Has to do specifically with the fact that I had NumberStats set up as a struct (ValueType)
                    // Jon Skeet says mutable structs are evil
                    // Now that NumberStats is a reference type, we're not modifying the Value in the dictionary
                    // as now that value is merely a reference (to the NumberStats object on the heap)
                    // ++(dict[arr[i]].Frequency);
                    dict[ arr[i] ].Frequency += 1; // Pretty sure I don't need these parenthases, but let's be safe
                else
                    // WAS BUG: Cannot initialize NumberStats with a collection initializer
                    // because it does not implement IEnumerable
                    // Need to either write a parameterized constructor and call that
                    // or use the correct initializer syntax and reference the public properties
                    // by name
                    //dict.Add(arr[i], new NumberStats { i, 1 });
                    dict.Add(   arr[i],
                                new NumberStats { SourceIndex = i,
                                                  Frequency = 1 });
            }

            var orderedDict = dict.OrderByDescending(kvp => kvp.Value.Frequency)
                                  .ThenBy(kvp => kvp.Value.SourceIndex);

            var builder = new StringBuilder("{ ");

            foreach (var kvp in orderedDict)
            {
                // WAS BUG: Printing one too many copies of each number
                // for (var k = 1; k <= kvp.Value.Frequency; ++k)
                for (var k = 1; k <= kvp.Value.Frequency; ++k)
                {
                    // WAS BUG: StringBuilder as Append, not Add
                    //builder.Add($"{arr[kvp.Value.SourceIndex]}, ");
                    builder.Append($"{arr[kvp.Value.SourceIndex]}, ");
                }
            }

            // Drop the comma after the last element
            builder.Remove(builder.Length - 2, 1);

            // WAS BUG: Forgot to close the array curly brace
            builder.Append("}");

            var result = builder.ToString();

            return result;
        }


        public class NumberStats
        {
            public int SourceIndex { get; set; }
            public int Frequency { get; set; }
        }


        // Rewrite for shooting vid
        public static void PrintByFreq(int[] arr)
        {
            if (arr == null)
                throw new ArgumentNullException("Parameter int[] arr is null.");

            if (arr.Length == 0) // No elements to print
                return;

            var dict = new Dictionary<int, NumberStats>();

            for (var i = 0; i < arr.Length; ++i)
            {
                if ( dict.ContainsKey( arr[i] ) )
                    dict[ arr[i] ].Frequency += 1;
                else
                    dict.Add( arr[i], new NumberStats
                                        {
                                            SourceIndex = i,
                                            Frequency = 1
                                        });
            }

            // ^^ using System.Linq; ^^
            var orderedDict = dict.OrderByDescending(
                                        kvp => kvp.Value.Frequency)
                                  .ThenBy(
                                        kvp => kvp.Value.SourceIndex);

            var builder = new StringBuilder();

            foreach (var kvp in orderedDict)
            {
                for (var k = 1; k <= kvp.Value.Frequency; ++k)
                {
                    // StringBuilder has Append, not Add
                    builder.Append($"{ arr[ kvp.Value.SourceIndex ] } ");
                }
            }

            // Drop the extra space after the last element
            builder.Remove(builder.Length - 1, 1);

            Console.WriteLine( builder.ToString() );
        }




        private class TestCase
        {
            public int[] InputArray { get; set; }
            public int[] CorrectOutputArray { get; set; }
        }

    }
}
