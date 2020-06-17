using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem6
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutputInt = 3,
                    InputString = "babac",
                },
                new TestCase
                {
                    CorrectOutputInt = 7,
                    InputString = "abcdeboiler",
                },
                new TestCase
                {
                    CorrectOutputInt = 5,
                    InputString = "bdebile",
                },
                new TestCase
                {
                    CorrectOutputInt = 1,
                    InputString = "fff",
                },
                new TestCase
                {
                    CorrectOutputInt = 2,
                    InputString = "afafafa",
                },
                new TestCase
                {
                    CorrectOutputInt = 2,
                    InputString = "afafafaf",
                },
                new TestCase
                {
                    CorrectOutputInt = 3,
                    InputString = "abca",
                },
                new TestCase
                {
                    CorrectOutputInt = 0,
                    InputString = String.Empty,
                },
                new TestCase
                {
                    CorrectOutputInt = 3,
                    InputString = "abc",
                },
                new TestCase
                {
                    CorrectOutputInt = 5,
                    InputString = "bacacababcbdbeac",
                },
            };

            string intro =
                "==============\n" +
                "= Problem #6 =\n" +
                "==============\n" +
                "\n" +
                "Given a string, find the length of the longest substring with " +
                "all distinct characters.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: {testCases[i].InputString}");
                Console.WriteLine($"Output: { testCases[i].CorrectOutputInt }");

                var testCaseResult = LongestDistinctSubstring(testCases[i].InputString);

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectOutputInt)
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
            public int CorrectOutputInt { get; set; }
        }

        public static int LongestDistinctSubstring(string s)
        {
            if (s == null)
                throw new ArgumentNullException("parameter string s is null");

            var longestLen = 0;
            var currLen = 0;
            var dict = new Dictionary<int, int>();

            for (var i = 0; i < s.Length; ++i)
            {
                // BUG!
                // Checking Dictionary<TKey, TValue> for a key: ContainsKey!
                //if (dict.Contains(s[i]) == false)
                //if (dict.HasKey(s[i]) == false)
                if (!dict.ContainsKey(s[i]))
                    ++currLen;
                    // Added to dict after else
                else
                {
                    //if (s[i] != s[k])
                    //if (s[i] == s[k])
                    //    --currLen;
                    //else

                    // Don't drop chars if the curr char is the same as the
                    // first/(only?) char that we would drop from the dict
                    // we don't want to drop it, we just overwrote it
                    // it's fine, leave it alone
                    // ...though might be simpler to simply dict[s[i]] = i;
                    // last after the if/else, as it exists in both the if and
                    // the else and would remove the need to be conditional
                    // about this for loop, I think?
                    var k = dict[s[i]];
                    var leftMostDrop = /*currLen > i ? currLen - i :*/ i - currLen;
                    // How to explain the abs here?
                    // Maybe leftMost Drop had always been 0, so that's why my
                    // subtraction was ?incorrect
                    // Yep, that's it. Glad to catch it now!

                    longestLen = currLen > longestLen ? currLen : longestLen;

                    if (s.Length - i < longestLen)
                        return longestLen;

                    currLen += leftMostDrop - k;

                    for (; k >= leftMostDrop; --k)
                        dict.Remove(s[k]);
                        // For cases when s[k] == s[i],
                        // note that s[i] is (re-)added after this else
                }
                dict[s[i]] = i;
            }
            return currLen > longestLen ? currLen : longestLen;
        }
    }
}
