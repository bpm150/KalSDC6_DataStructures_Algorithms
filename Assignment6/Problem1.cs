using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem1
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectFirstLongestPalindrome = "tacocat",
                    InputString = "babadtacocat",
                },
                new TestCase
                {
                    CorrectFirstLongestPalindrome = "FOOF",
                    InputString = "babadFOOF",
                },
                new TestCase
                {
                    CorrectFirstLongestPalindrome = String.Empty,
                    InputString = String.Empty,
                },
                new TestCase
                {
                    CorrectFirstLongestPalindrome = String.Empty,
                    InputString = "a",
                },
                new TestCase
                {
                    CorrectFirstLongestPalindrome = "bab",
                    InputString = "babad",
                },
            };

            string intro =
                "==============\n" +
                "= Problem #1 =\n" +
                "==============\n" +
                "\n" +
                "Given a string s, find the longest palindromatic substring of string s.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].InputString }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectFirstLongestPalindrome }\"");

                var testCaseResult = LongestPalindrome(testCases[i].InputString);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectFirstLongestPalindrome)
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
            public string InputString { get; set; }
            public string CorrectFirstLongestPalindrome { get; set; }
        }

        public static string LongestPalindrome(string s)
        {
            var longestPal = String.Empty;

            if (s == null)
                throw new ArgumentNullException("string s is null");

            // Empty string and string of length 1 don't need to be special-cased
            // main logic correctly gives String.Empty as a result

            for (var i = 0; i < s.Length; ++i)
            {
                if (longestPal.Length >= s.Length - i)
                    break;

                for (var j = i + 1; j < s.Length; ++j)
                {
                    var subsLen = j - i + 1;
                    if (subsLen <= longestPal.Length)
                        continue;

                    var candPal = s.Substring(i, subsLen);
                    if (IsPalindrome(candPal) && subsLen > longestPal.Length)
                        longestPal = candPal;

                    // THIS PART DOESN'T WORK (SEE DRAFT VIDEO FOR DISCUSSION)
                    // Inequality holds if you add 1 to both sides
                    //if (s.Length - j - 1 < subsLen - 1)
                    //    break;
                }
            }
            return longestPal;
        }

        private static bool IsPalindrome(string s)
        {
            var finalInd = s.Length - 1;
            for (var i = 0; i <= finalInd; ++i)
            {
                // BUG: "+ cannot be applied to operands of type Index and int"
                // (requires parenthases)
                if (s[i] != s[^(i + 1)])
                    return false;
            }
            return true;
        }
    }
}
