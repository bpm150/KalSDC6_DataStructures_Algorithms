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
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = String.Empty,
                //},
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = "notaromannumeraleither",
                //},
                new TestCase
                {
                    CorrectFirstPalindrome = "bab",
                    InputString = "babad",
                },
                //new TestCase
                //{
                //    CorrectFirstPalindrome = 12,
                //    InputString = "XII",
                //},
                //new TestCase
                //{
                //    CorrectFirstPalindrome = 3999,
                //    InputString = "MMMCMXCIX",
                //},
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

                Console.WriteLine($"Input: { testCases[i].InputString }");
                Console.WriteLine($"Output: { testCases[i].CorrectFirstPalindrome }");

                var testCaseResult = LongestPalindrome(testCases[i].InputString);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectFirstPalindrome)
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

        private class TestCase
        {
            public string InputString { get; set; }
            public string CorrectFirstPalindrome { get; set; }
        }

        public static string LongestPalindrome(string s)
        {
            var longestLen = 0;
            var longestPal = String.Empty;

            // This can go in the for loop after all
            var i = 0;
            if (s == null)
                throw new ArgumentNullException("string s is null");

            // Despite my banter in the prelim vid,
            // I think this may not be necessary after all:
            if (s.Length == 1)
                return String.Empty;
            // (Because I decided that a single-character substring is not a palindrome)
            // TODO: Decide for realsies if a single-character substring is a palindrome or not

            for (; i < s.Length; ++i)
            {
                if (longestLen >= s.Length - i)
                    break;

                for (var j = i + 1; j < s.Length; ++j)
                {
                    var subsLen = j - i + 1;
                    if (subsLen <= longestLen)
                        continue;
                    var candPal = s.Substring(i, subsLen);
                    var isPali = IsPalindrome(candPal);
                    if (isPali && subsLen > longestLen)
                    {
                        longestLen = subsLen;
                        longestPal = candPal;
                    }
                    // Inequality holds if you add 1 to both sides
                    if (s.Length - j - 1 < subsLen - 1)
                        break;
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
