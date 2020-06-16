using System;
using System.Collections.Generic;
using System.Text;

namespace RelatedPractice
{
    class LCS
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputS1 = "abcd",
                    InputS2 = "dbcd",
                    CorrectLength = 2,
                },
                new TestCase
                {
                    InputS1 = "aab",
                    InputS2 = "azb",
                    CorrectLength = 2,
                },
                new TestCase
                {
                    InputS1 = "abcdgh",
                    InputS2 = "aedfhr",
                    CorrectLength = 3,
                },
                new TestCase
                {
                    InputS1 = "aggtab",
                    InputS2 = "gxtxayb",
                    CorrectLength = 4,
                },
                new TestCase
                {
                    InputS1 = String.Empty,
                    InputS2 = "abc",
                    CorrectLength = 0,
                },
            };

            string intro =
                "====================\n" +
                "= Related Practice =\n" +
                "====================\n" +
                "\n" +
                "Given two strings, find the length of the longest common" +
                " SUBSEQUENCE.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].InputS1 }\"");
                Console.WriteLine($"Input: \"{ testCases[i].InputS2 }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectLength }\"");

                var testCaseResult = LongestCommonSubsequenceLength_DP(
                    testCases[i].InputS1,
                    testCases[i].InputS2);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectLength)
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}! Your answer is \"{testCaseResult}\".");
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

        private class TestCase
        {
            public string InputS1 { get; set; }
            public string InputS2 { get; set; }
            public int CorrectLength { get; set; }
        }


        public static int LongestCommonSubsequenceLength_Recur(string s1, string s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException(
                    "a string parameter is null");

            return LCSL_Recur_Helper(s1, s2);
        }

        private static int LCSL_Recur_Helper(string s1, string s2)
        {
            if (s1 == String.Empty || s2 == String.Empty)
                return 0;
            else if (s1[^1] == s2[^1])
                return 1 + LCSL_Recur_Helper(s1[..^1], s2[..^1]);
            else
            {
                var delFroms1 = LCSL_Recur_Helper(s1[..^1], s2);
                var delFroms2 = LCSL_Recur_Helper(s1, s2[..^1]);
                return delFroms1 > delFroms2 ? delFroms1 : delFroms2;
            }
        }

        public static int LongestCommonSubsequenceLength_DP(string s1, string s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException(
                    "a string parameter is null");

            // BUG: Can only create a 2D int array this way if
            // the second expression is a compile-time constant
            // var dpChart = new int[s1.Length + 1][s2.Length + 1];
            var dpChart = new int[s2.Length + 1][];
            // s2 goes "down the left" of the chart

            // Cannot assign to 'arr' because it is a 'foreach iteration variable'
            //foreach (var arr in dpChart)
            //{
            //    arr = new int[s2.Length + 1];
            //}
            for (var k = 0; k < dpChart.Length; ++k)
                dpChart[k] = new int[s1.Length + 1];
            // s1 goes "across the top" of the chart

            // 0 is the default value for int, so the first row and
            // the first column already have the correct value


            // (Now, populate the chart
            // Already know that the optimal solution (this one, by DP)
            // is O(m*n) so nested loops is appropriate)

            for (var i = 1; i < dpChart.Length; ++i)
            {
                for (var j = 1; j < dpChart[0].Length; ++j)
                {
                    if (s2[i-1] == s1[j-1])
                        // Going diagonal + 1 is you being able to extend
                        // the LCS by one with that pair of matching characters
                        // "removed" (using the recursive solution as a basis)
                        dpChart[i][j] = dpChart[i-1][j-1] + 1;
                    else
                        dpChart[i][j] = dpChart[i-1][j] > dpChart[i][j-1] ?
                                dpChart[i-1][j] : dpChart[i][j-1];
                }
            }

            return dpChart[^1][^1];
        }
    }
}
