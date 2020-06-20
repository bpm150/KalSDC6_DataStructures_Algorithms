using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using DataStructuresAndAlgos;

namespace Assignment6
{
    class Problem9
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutput = "to",
                    InputStrArr = new List<string>
                    {
                        "TOP",
                       "TO",
                    },
                },
                new TestCase
                {
                    CorrectOutput = "f",
                    InputStrArr = new List<string>
                    {
                        "FOR",
                        "FAN"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "f",
                    InputStrArr = new List<string>
                    {
                        "FOX",
                        "FOR",
                        "FAN"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        "HUG",
                        "ME"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "ap",
                    InputStrArr = new List<string>
                    {
                        "apple",
                        "ape",
                        "April"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "foo",
                    InputStrArr = new List<string>
                    {
                        "foo",
                        "food"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        String.Empty,
                        "food"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        String.Empty,
                        String.Empty
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {},
                },
            };

            string intro =
                "==============\n" +
                "= Problem #9 =\n" +
                "==============\n" +
                "\n" +
                "Given an array of n strings, find the longest common prefix.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ Utility.CollectionToString(testCases[i].InputStrArr) }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectOutput }\"");

                var testCaseResult = LongestCommonPrefix(testCases[i].InputStrArr.ToArray());

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
            public List<string> InputStrArr { get; set; }
            public string CorrectOutput { get; set; }
        }

        // BUG:
        // Forgot to put the return type on the method
        public static string LongestCommonPrefix(string[] strArr)
        {
            if (strArr == null)
                throw new ArgumentNullException(
                    "parameter array is null");

            // BUG:
            // Can't call First() on an empty sequence/collection
            // The for loops are safe, we reach for shortestStr first
            if (strArr.Length < 1)
                return String.Empty;

            // TODO:
            // OH! We still need to check and make sure each individual
            // string in the array is not null

            var shortestStr = strArr.OrderBy(str => str.Length).First();
            var shortestLen = shortestStr.Length;

            for (var j = 0; j < shortestLen; ++j)
            {
                var compCh = Char.ToLower(strArr[0][j]);
                for (var i = 1; i < strArr.Length; ++i)
                {
                    var ch = Char.ToLower(strArr[i][j]);
                    if (compCh != ch)
                        // BUG:
                        // Index/Range syntax is "..," not "..."
                        // I think it was Ruby that had both two dots and
                        // three dots, where each meant something different
                        return strArr[0][..j].ToLower();
                }
            }
            return shortestStr.ToLower();
        }

    }
}
