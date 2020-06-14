using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem3
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputSBStart = "azonam",
                    InputSATarget = "amazon",
                    CorrectTrueFalse = true,
                },
                new TestCase
                {
                    InputSBStart = "onamaz",
                    InputSATarget = "amazon",
                    CorrectTrueFalse = true,
                },
                new TestCase
                {
                    InputSBStart = "zonama",
                    InputSATarget = "amazon",
                    CorrectTrueFalse = false,
                },
                new TestCase
                {
                    InputSBStart = "zoama",
                    InputSATarget = "amazon",
                    CorrectTrueFalse = false,
                },
                new TestCase
                {
                    InputSBStart = "a",
                    InputSATarget = "a",
                    CorrectTrueFalse = true,
                },
                new TestCase
                {
                    InputSBStart = "b",
                    InputSATarget = "a",
                    CorrectTrueFalse = false,
                },
                new TestCase
                {
                    InputSBStart = String.Empty,
                    InputSATarget = String.Empty,
                    CorrectTrueFalse = true,
                },
            };

            string intro =
                "==============\n" +
                "= Problem #3 =\n" +
                "==============\n" +
                "\n" +
                "Given two strings sa and sb, determine if string sa can be" +
                "obtained by rotating another string sb by two places.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ testCases[i].InputSATarget }\"");
                Console.WriteLine($"Output: \"{ testCases[i].InputSBStart }\"");

                var testCaseResult = CanRotate(testCases[i].InputSATarget, testCases[i].InputSBStart );

                string resultMessage;

                if (testCaseResult == testCases[i].CorrectTrueFalse)
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
            public string InputSATarget { get; set; }
            public string InputSBStart { get; set; }

            public bool CorrectTrueFalse { get; set; }
        }

        public static bool CanRotate(string sa, string sb)
        {
            if (sa == null || sb == null)
                throw new ArgumentNullException("an input string is null");

            if (sa.Length != sb.Length)
                return false;

            // Empty strings defined to be true
            // Strings of length 1 defined to be true if the chars are equal
            if (sa.Length < 2)
                return sa == sb;


            //if (sa == String.Empty && sb == String.Empty)
            //    return true;
            //// WHOOPS. BUG.
            ////else
            ////  return false;
            //// THIS WOULD RETURN FALSE ANYTIME EITHER STRING HAS ANY CHARACTERS!

            //// Note that Equals() does not require LINQ (is on IEquatable, right?)
            //if (sa.Length == 1)
            //    return sa.Equals(sb);

            //if (sa.Length == 1 && sa.Equals(sb))
            //    return true;
            //else
            //    return false;
            // NO AGAIN WITH THE RETURN FALSE
            // WOULD MAKE THE REST OF THE METHOD UNREACHABLE
            // HAVING THEM BE THE SAME LENGTH IN ORDER TO BE TRUE IS HANDLED
            // NEXT...ACTUALLY BEST TO BAIL FOR THIS FIRST
            // MAKES THESE BAD FALSES LESS TEMPTING



            // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-8.0/ranges
            // C# 8.0 has a new concept of a range
            // allows us to slice collections at runtime
            // this includes strings
            // (as an immmutable collection of immutable characters)
            // Usually I would use the string Substring method
            // but this is just too cool.
            // Saw this kind of thing when I was doing stuff in Ruby
            // Really glad to see it show up in C#

            // Check for possible rotation around the right end of the string


            // REMEMBER WHEN YOU ARE SPECIFYING A RANGE
            // IF YOU SPECIFY A START, YOU ARE SPECIFYING THE ELEMENT TO START ON
            // IF YOU SPECIFY AN END, YOU ARE SPECIFYIGN THE ELEMENT AFTER
            // THE ELEMENT TO END ON

            // FROM END (^) AND FROM START INDEX CREATION WORKS FOR SPECIFYING
            // BOTH THE START AND END OF A RANGE
            // OMIT THE START OF THE RANGE TO START AT THE FIRST ELEMENT
            // OMIT THE END OF THE RANGE TO INCLUDE THE LAST ELEMENT

            if (sb[^2..] == sa[..2])
                return sa[2..] == sb[..^2];

            // Check for possible rotation around the left end of the string
            if (sb[..2] == sa[^2..])
                return sa[..^2] == sb[2..];

            // Now convert the initial substring comparisons to use range
            // notation too, think it will improve readabilty
            // (There really isn't a good reason to have the two steps
            // written using different strategies)

            return false;   
        }
    }
}
