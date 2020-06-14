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
                return (sa == sb);
            

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


            // Check for possible rotation around the right end of the string
            if (sb[^2] == sa[0] && sb[^1] == sa[1])
            {
                var sbSub = sb.Substring(0, sb.Length - 2);
                // If you want your substring to go to the end of the string,
                // you can omit the second param (length)
                var saSub = sa.Substring(2);
                return saSub == sbSub;
            }

            // Check for possible rotation around the left end of the string
            if (sb[0] == sa[^2] && sb[1] == sa[^1])
            {
                var sbSub = sb.Substring(2);
                var saSub = sa.Substring(0, sa.Length - 2);
                return saSub == sbSub;
            }

            return false;   
        }
    }
}
