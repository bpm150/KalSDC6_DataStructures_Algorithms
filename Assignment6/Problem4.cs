using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem4
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutputInt = 2564,
                    RomanNumeralString = "MMDLXIV",
                },
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = "IIII",
                //},
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = "XVLI",
                //},
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = "notaromannumeraleither",
                //},
                new TestCase
                {
                    CorrectOutputInt = 1,
                    RomanNumeralString = "I",
                },
                new TestCase
                {
                    CorrectOutputInt = 2,
                    RomanNumeralString = "II",
                },
                new TestCase
                {
                    CorrectOutputInt = 3,
                    RomanNumeralString = "III",
                },
                new TestCase
                {
                    CorrectOutputInt = 4,
                    RomanNumeralString = "IV",
                },
                new TestCase
                {
                    CorrectOutputInt = 5,
                    RomanNumeralString = "V",
                },
                new TestCase
                {
                    CorrectOutputInt = 6,
                    RomanNumeralString = "VI",
                },
                new TestCase
                {
                    CorrectOutputInt = 7,
                    RomanNumeralString = "VII",
                },
                new TestCase
                {
                    CorrectOutputInt = 8,
                    RomanNumeralString = "VIII",
                },
                new TestCase
                {
                    CorrectOutputInt = 9,
                    RomanNumeralString = "IX",
                },
                new TestCase
                {
                    CorrectOutputInt = 10,
                    RomanNumeralString = "X",
                },
                new TestCase
                {
                    CorrectOutputInt = 3295,
                    RomanNumeralString = "MMMCCXCV",
                },
                new TestCase
                {
                    CorrectOutputInt = 1943,
                    RomanNumeralString = "MCMXLIII",
                },
                new TestCase
                {
                    CorrectOutputInt = 158,
                    RomanNumeralString = "CLVIII",
                },
                new TestCase
                {
                    CorrectOutputInt = 12,
                    RomanNumeralString = "XII",
                },
                new TestCase
                {
                    CorrectOutputInt = 3999,
                    RomanNumeralString = "MMMCMXCIX",
                },
            };

            string intro =
                "==============\n" +
                "= Problem #4 =\n" +
                "==============\n" +
                "\n" +
                "Given an string in roman no format (s)  your task is to convert it to integer.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: {testCases[i].RomanNumeralString}");
                Console.WriteLine($"Output: { testCases[i].CorrectOutputInt }");

                var testCaseResult = RomanToInt(testCases[i].RomanNumeralString);

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
            public string RomanNumeralString { get; set; }
            public int CorrectOutputInt { get; set; }
        }

        private enum Place
        {
            One = 0,
            Ten = 1,
            Hun = 2,
            Tho = 3,
        }

        // This is weird...
        // Use new for the inner arrays but not for the outer one?
        // Outer array corresponds with the Place being determined
        // Inner array corresponds to the character currently being compared
        // against for the parsing logic (see ProcessPlace)
        private static readonly char[][] NL =
            {
                new char[]{'I', 'V', 'X',}, // Ones place chars
                new char[]{'X', 'L', 'C',}, // Tens place chars
                new char[]{'C', 'D', 'M',}, // Hundreds place chars
                // Only Thousands place char is 'M', and that is handled in a special case
            };

        private static readonly string err =
            " is not a valid roman numeral that can be expressed between" +
            "1 (I) and 3999 (MMMCMXCIX).";

        // Improvement could be to ignore leading and trailing whitespace
        public static int RomanToInt(string str)
        {
            if (str == null)
                throw new ArgumentNullException("string str is null");

            int result = 0;
            var i = str.Length - 1;
            var p = Place.One;

            while (i >= 0)
            {
                // "Variable can be inlined"
                //int placeResult;
                i = ProcessPlace(p++, str, i, out int placeResult);
                result += placeResult;
                //++p; // Works instead of:
                // p = (Place)((int)p + 1);
            }

            return result;
        }

        // Returns the index to the left of the last character processed
        // while parsing the current place of str
        private static int ProcessPlace(Place p, string str, int ind, out int result)
        {
            result = 0;
            var i = ind;

            // Note that roman numerals can only express up to 3999 without
            // having the "above bar" notation.
            // Special case for MM and MMM (two thousand and three thousand)
            if (p == Place.Tho)
            {
                for (; i >= 0; --i)
                {
                    if (str[i] == 'M')
                        result += 1000;
                    else
                        throw new ArgumentException($"{str}{err}");
                }

                // Bypass main logic below
                return i;
            }


            var pi = (int)p;
            var count = 0;
            for (; i >= 0; --i)
            {
                // Add up the effect of the trailing Is, Xs, or Cs (depending on place being parsed)
                if (str[i] == NL[pi][0])
                {
                    result += 1;
                    ++count;
                }
                else
                    break;
            }
            // Limit to three trailing repeated chars (disallow trailing "IIII", etc.)
            if (count > 3)
                throw new ArgumentException($"{str}{err}");

            if (i >= 0)
            {
                // Count the effect of the V, L, or D
                if (str[i] == NL[pi][1])
                {
                    result += 5;
                    --i;
                } // OR count the effect of the X, C, or M
                else if (str[i] == NL[pi][2])
                {
                    result += 10;
                    --i;
                }

                // Remove the effect of a leading I, X or C
                if (i >= 0 && str[i] == NL[pi][0])
                {
                    result -= 1;
                    --i;
                }
            }

            // Adjust the accumulated effects by the approprite place being parsed
            if (p == Place.Ten)
                result *= 10;
            else if (p == Place.Hun)
                result *= 100;

            return i;
        }

    }
}
