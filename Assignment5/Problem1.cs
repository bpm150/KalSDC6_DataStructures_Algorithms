using DataStructuresAndAlgos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment5
{
    class Problem1
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntArray = new int[]{ 4, 5, 2, 25 },
                    CorrectNGEString = "{ {4, 5}, {5, 25}, {2, 25}, {25, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 13, 7, 6, 12 },
                    CorrectNGEString = "{ {13, -1}, {7, 12}, {6, 12}, {12, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 50, 2, 15, 0, 5, -3, -8, 7, 4, 1, 10, 6, 5, 12, 8, 2, 20 },
                    CorrectNGEString = "{ {50, -1}, {2, 15}, {15, 20}, {0, 5}, {5, 7}, {-3, 7}, {-8, 7}, {7, 10}, {4, 10}, {1, 10}, {10, 12}, {6, 12}, {5, 12}, {12, 20}, {8, 20}, {2, 20}, {20, -1} }",
                },
                new TestCase
                {
                    // Same principle as using String.Empty instead of ""
                    InputIntArray = Array.Empty<int>(),
                    //InputIntArray = new int[]{},
                    CorrectNGEString = "{}",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 4 },
                    CorrectNGEString = "{ {4, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 12, 8, 2, 20, 10, 5, 3, -4 },
                    CorrectNGEString = "{ {12, 20}, {8, 20}, {2, 20}, {20, -1}, {10, -1}, {5, -1}, {3, -1}, {-4, -1} }",
                },
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 12, 9, 20, 10, 8, 0, 6, 9, 5, 10, 11},
                //    CorrectLeadersString = $"leaders are 20 and 11",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 0, -6, -10, -12, -20, -8, -5},
                //    CorrectLeadersString = $"leaders are 0 and -5",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ -20, -5, -20, -10, -5, -10},
                //    CorrectLeadersString = $"leaders are -5 and -10",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{0},
                //    CorrectLeadersString = $"leader is 0",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{0, 1, 2, 3, 4, 5, 6},
                //    CorrectLeadersString = $"leader is 6",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 4, 3, 1, 2},
                //    CorrectLeadersString = $"leaders are 4, 3 and 2",
                //},
            };


            string intro =
                "==============\n" +
                "= Problem #1 =\n" +
                "==============\n" +
                "\n" +
                "Given an array, print the Next Greater Element (NGE) for every element. " +
                "The Next greater Element for an element x is the first greater element " +
                "on the right side of x in array. Elements for which no greater element " +
                "exist, consider next greater element as -1.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");


                Console.WriteLine($"For the array: {Utility.CollectionToString(testCases[i].InputIntArray)}");
                Console.WriteLine($"The correct result is: {testCases[i].CorrectNGEString}");

                var testCaseResult = PrintElementsWithNextGreaterElements(testCases[i].InputIntArray);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectNGEString)
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}!");
                Console.WriteLine($"Your answer is:        {testCaseResult}");
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



        // BUG: Remember that each value in an enum declaration ends with
        // a comma, not a semicolon
		private enum CompResult
		{
			Default = 0,
			NoNotLessThan = 1,
			YesLessThan = 2,
		}


        // Well, not really a bug in the whiteboard sense:
        // For testing, these methods must all be static (not require an object instance)
		public static string PrintElementsWithNextGreaterElements(int[] arr)
		{
			const int DEFAULT_NGE = -1;

            if (arr == null)
				throw new ArgumentNullException();

			if (arr.Length == 0)
				return "{}";

            var builder = new StringBuilder(" }");
            // Using the helper here instead of writing out the pattern multiple times
            // to be consistent
            PrependPair(arr[^1], DEFAULT_NGE, builder);
            // The last element, which may also be the first, and if it is:

            // Main logic requires array length of at least two
            if (arr.Length == 1)
            {
                builder[0] = '{';
                return builder.ToString();
            }
            //// From back when we were using a paralell array to store NGEs
            //// Remember that the Array.Clone method returns an object[], must cast it to appropriate type
            //var arrNGE = (int[])arr.Clone();
            //// By definition
            //arrNGE[^1] = DEFAULT_NGE;


            var localMaxes = new Stack<int>();
            // remains empty until the right element of a pair is actually larger than the left element of a pair

            // Will refer to result of last comparison to determine if arr[i + 1] is determined to be a new local maximum
            var lastResult = CompResult.NoNotLessThan;
			// NoNotLessThan is correct for the "comparison" between arr[^1] and the non-element to its right, since arr[^1] will be a local max if arr[i] < arr[i + 1]
			// Default is reserved to essentially mean uninitialized

			var thisResult = CompResult.Default;


            // walk array from right to left
            // i starts at next-to-last index
            // compare the elements in pairs
            for (var i = arr.Length - 2; i >= 0; --i)
			{
				if (arr[i] < arr[i + 1])
				// Immediately adjacent element is NGE
				{
					thisResult = CompResult.YesLessThan;
                    PrependPair(arr[i], arr[i + 1], builder);

                    // From back when we were using a paralell array to store NGEs
					//arrNGE[i] = arr[i + 1];

					if (lastResult == CompResult.NoNotLessThan)
					{
						localMaxes.Push(arr[i + 1]);
					}
				}
				else // arr[i] >= arr[i + 1]
					 // Immediately adjacent element is not NGE
				{
					thisResult = CompResult.NoNotLessThan;

                    // (Destructive) check of previous local maxes for NGE
                    bool foundNGE = false;
                    while(!foundNGE && localMaxes.Count > 0)
                    {
                        if (arr[i] < localMaxes.Peek())
                        {
                            PrependPair(arr[i], localMaxes.Peek(), builder);

                            // From back when we were using a paralell array to store NGEs
                            //arrNGE[i] = localMaxes.Peek();
                            
                            // Leave the NGE that we just used where we found it in localMaxes, it is still a candidate to be an NGE again in the future
                            foundNGE = true;
                        }
                        else
                        {
                            // WAS BUG: Must check of stack has elements before Pop
                            localMaxes.Pop();
                        }
                    }

					if (localMaxes.Count == 0)
					// No candidate NGEs exist, or arr[i] was greater than each candidate NGE
					{
                        PrependPair(arr[i], DEFAULT_NGE, builder);

                        // From back when we were using a paralell array to store NGEs
                        //arrNGE[i] = DEFAULT_NGE;
                    }

                    // Not pushing onto localMaxes here, since we won't know until next iteration whether the current arr[i] is a local max
                }

				// Advance state of tracking for next loop iteration
				lastResult = thisResult;
				thisResult = CompResult.Default;
			}

            // Now construct the string left to right (or print it out as you go...but not since we're doing fixup at the end of the string building...and we're headed back to build the string as we go in the next version anyway)

            // Example: { {4, 5}, {5, 25}, {2, 25}, {25, -1} }

            // TODO: Tell the builder how big it needs to be to avoid later reallocation
            // (Since we already know)


            // From back when we were using a paralell array to store NGEs
            //         // BUG: it's either k < arr.Length OR k <= arr.Length-1
            //for (var k = 0; k < arr.Length; ++k)
            //{
            //	// It's Append, not Add, for StringBuilder, right?
            //	builder.Append($"{{{arr[k]}, {arrNGE[k]}}}, ");
            //             // BUG: array index var for this loop is k, not i
            //             // But this loop is going away in the next version anyway
            //}

            //         // Drop the last comma
            //         // param1: remove starting from what index
            //         // param2: how many to remove
            //         // that is: builder.Remove(where, howmany);
            //         builder.Remove(builder.Length - 2, 1);

            //// Add the final curly brace
            //builder.Append('}');


            // Overwrite the final leading comma inserted by PrependPair
            builder[0] = '{';

            return builder.ToString();


			// Go ahead and finish writing this, test that this solution works, then tomorrow adapt it to build the string right to left during initial traversal of the array (instead of cloning the array)

			// That would actually make the StringBuilder approach kind of good, even if simply printing the result at the end

			// Would reduce space complexity by O(n), since would not need the clone array
			// Would reduce time complexity by O(n), since would not need to traverse the array again to build the string at the end...the string would be built as you go

		}


        // Thought about telling StringBuilder how big to be,
        // but that depends on how many digits each key and value
        // ends up being.
        // Let's leave out this complexity for now.

        private static void PrependPair(int ele, int nge, StringBuilder sb)
        {
            // Example of overall finished string:
            // "{ {4, 5}, {5, 25}, {2, 25}, {25, -1} }"

            // Each call adds this much:
            // ", {25, -1}"

            // param1: insert starting at what index
            // param2: what to insert
            // that is: sb.Insert(where, what);

            // For an interpolated string, use "{{" for a single '{' and "}}" for a single '}'

            sb.Insert(0, $", {{{ele}, {nge}}}");


            // Character array would be more performant
            // (placing characters in the array exactly where they belong vs.
            // continuously inserting into the sb at index 0)
        }


        private class TestCase
        {
            public int[] InputIntArray { get; set; }

            public string CorrectNGEString { get; set; }
        }
    }
}
