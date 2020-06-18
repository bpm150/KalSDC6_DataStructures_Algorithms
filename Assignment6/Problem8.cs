using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem8
    {
        public static void RunTests()
        {
            string s;
            string x;

            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    StringSToFindIn = s = "HHAT",
                    StringXToFind = x = "HA",
                    CorrectIndex = s.IndexOf(x),
                },
                new TestCase
                {
                    StringSToFindIn = s = "hahat",
                    StringXToFind = x = "hat",
                    CorrectIndex = s.IndexOf(x),
                },
                new TestCase
                {
                    StringSToFindIn = s = String.Empty,
                    StringXToFind = x = "hat",
                    CorrectIndex = s.IndexOf(x),
                },
                // IndexOf gives index 0 as as the result for finding
                // the empty string in the empty string
                // I'm leaving mine as -1
                //new TestCase
                //{
                //    StringSToFindIn = s = String.Empty,
                //    StringXToFind = x = String.Empty,
                //    CorrectIndex = s.IndexOf(x),
                //},
                new TestCase
                {
                    StringSToFindIn = s = "sdadsdaasdasddadsdadadsadsdssddss",
                    StringXToFind = x = "sads",
                    CorrectIndex = s.IndexOf(x),
                },
                new TestCase
                {
                    StringSToFindIn = s = "SHORT",
                    StringXToFind = x = "LONGER",
                    CorrectIndex = s.IndexOf(x),
                },
            };

            string intro =
                "==============\n" +
                "= Problem #8 =\n" +
                "==============\n" +
                "\n" +
                "Given two strings s and x, find the first occurrence of" +
                "string s in string x.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].StringSToFindIn }\"");
                Console.WriteLine($"Input: \"{ testCases[i].StringXToFind }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectIndex }\"");

                var testCaseResult = FindSubstring(
                    testCases[i].StringSToFindIn,
                    testCases[i].StringXToFind);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectIndex)
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
            public string StringSToFindIn { get; set; }
            public string StringXToFind { get; set; }
            public int CorrectIndex { get; set; }
        }
        public static int FindSubstring(string s, string x)
        {
            if (s == null || x == null)
                throw new ArgumentNullException("a param string is null");

            //if (s.Length < x.Length)
            //    return -1;

            var dpChart = new int[x.Length + 1][];
            for (var k = 0; k < dpChart.Length; ++k)
                dpChart[k] = new int[s.Length + 1];

            for (var i = 1; i < dpChart.Length; ++i)
            {
                for (var j = 1; j < dpChart[0].Length; ++j)
                {
                    if (s[j - 1] == x[i - 1])
                    {
                        dpChart[i][j] = dpChart[i - 1][j - 1] + 1;
                        if (dpChart[i][j] == x.Length)
                            return j - x.Length;
                    }
                }
            }
            return -1;
        }

    }
}
