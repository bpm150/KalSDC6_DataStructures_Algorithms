using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem5
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputS1 = "abcd",
                    InputS2 = "dbca",
                    CorrectLength = 2,
                },
                new TestCase
                {
                    InputS1 = "zxabcdezy",
                    InputS2 = "yzabcdezx",
                    CorrectLength = 6,
                },
                new TestCase
                {
                    InputS1 = "abcdxyz",
                    InputS2 = "zyxabcd",
                    CorrectLength = 4,
                },
                new TestCase
                {
                    InputS1 = "xyzabmn",
                    InputS2 = "qxyzmab",
                    CorrectLength = 3,
                },
                new TestCase
                {
                    InputS1 = "xyzabmn",
                    InputS2 = "qxyzmabxyzab",
                    CorrectLength = 5,
                },
                new TestCase
                {
                    InputS1 = "xyz",
                    InputS2 = "abc",
                    CorrectLength = 0,
                },
                new TestCase
                {
                    InputS1 = String.Empty,
                    InputS2 = "abc",
                    CorrectLength = 0,
                },
            };

            string intro =
                "==============\n" +
                "= Problem #5 =\n" +
                "==============\n" +
                "\n" +
                "Given two strings, find the length of the longest common" +
                " substring.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].InputS1 }\"");
                Console.WriteLine($"Input: \"{ testCases[i].InputS2 }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectLength }\"");

                var testCaseResult = LongestCommonSubstringLength(
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


        public static int LongestCommonSubstringLength(string s1, string s2)
        {
            if (s1 == null || s2 == null)
                throw new ArgumentNullException("a parameter string is null");

            var longestLen = 0;
            var i = 0;

            while (i < s1.Length)
            {
                if (s1.Length - i <= longestLen)
                    break;

                var j = 0;
                while (j < s2.Length)
                {
                    if (s2.Length - j <= longestLen)
                        break;

                    if (s1[i] != s2[j])
                        ++j;
                    else
                    {
                        var k = longestLen;
                        while (IdxOk(i + k, s1) &&
                              IdxOk(j + k, s2) &&
                              s1[i + k] == s2[j + k])
                                ++k;
                        if (k > longestLen &&
                            // BUG:
                            // Operator '+' cannot be applied to operands
                            // of type 'Range' and 'int'
                            // s1[i..i + longestLen] == s2[j..j + longestLen])
                            // C# SYNTAX ISSUE: PRECEDENCE FOR NEW INDEX/RANGE FEATURES
                            s1[i..(i + longestLen)] == s2[j..(j + longestLen)])
                                longestLen = k;
                        j += k;
                    }
                    // TYPO:
                    // HAD THIS I INCREMENT UP HERE WITHIN THE WHILE J LOOP
                    // NEEDED TO BE AFTER THE WHILE J LOOP
                    //++i;
                    // LESSON: WHEN I'VE WORKED OUT A PROBLEM AT WHITEBOARD
                    // OR ON PAPER, IF IT DOESN'T WORK AFTER TYPING IT UP
                    // CHECK FOR TYPOS FIRST BEFORE GETTING TOO CARRIED AWAY
                    // WITH DEBUGGING
                }
                ++i;
            }
            return longestLen;
        }

        private static bool IdxOk(int idx, string s)
            => idx < s.Length;
    }
}
