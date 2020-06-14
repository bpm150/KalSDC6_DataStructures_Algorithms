using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem2
    {

        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutput = "ay",
                    InputString = "azxxzy",
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputString = "caaabbbaacdddd",
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputString = String.Empty,
                },
                new TestCase
                {
                    CorrectOutput = "acac",
                    InputString = "acaaabbbacdddd",
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputString = "abcdeffedcba",
                },
            };

            string intro =
                "==============\n" +
                "= Problem #2 =\n" +
                "==============\n" +
                "\n" +
                "Given a string s, recursively remove adjacent duplicate characters" +
                "from the string. The output string should have no adjacent duplicates.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].InputString }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectOutput }\"");

                var testCaseResult = RemDupes(testCases[i].InputString);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectOutput)
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
            public string CorrectOutput { get; set; }
        }

        public static string RemDupes(string s)
        {
            if (s == null)
                throw new ArgumentNullException("string s is null");

            var sb = new StringBuilder(s);

            RemDupRecur(sb);

            return sb.ToString();
        }


        private static void RemDupRecur(StringBuilder sb)
        {
            bool didRemove = false;

            var i = 0;
            // Note that sb.Length can change during an interation of this while
            // loop (when sb.Remove(i, dupeCount);)
            while (i <= sb.Length - 2)
            {
                if (sb[i] == sb[i + 1])
                {
                    // BUG: We need j after the for loop
                    // Must declare ahead of the for loop
                    var j = i + 2;
                    for (; j < sb.Length; ++j)
                    {
                        if (sb[i] != sb[j])
                            break;
                    }
                    // typed/copied j - 1 instead of j - i
                    // blame on mobile workspace setup
                    var dupeCount = j - i;
                    sb.Remove(i, dupeCount);
                    didRemove = true;
                }
                else
                    ++i;
            }
            if (didRemove)
                RemDupRecur(sb);
        }

    }
}
