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
                //new TestCase
                //{
                //    CorrectOutputInt = -1,
                //    RomanNumeralString = String.Empty,
                //},
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
                    CorrectOutputInt = 1000,
                    RomanNumeralString = "M",
                },
                new TestCase
                {
                    CorrectOutputInt = 1100,
                    RomanNumeralString = "MC",
                },
                new TestCase
                {
                    CorrectOutputInt = 2000,
                    RomanNumeralString = "MM",
                },
                new TestCase
                {
                    CorrectOutputInt = 2100,
                    RomanNumeralString = "MMC",
                },
                new TestCase
                {
                    CorrectOutputInt = 3000,
                    RomanNumeralString = "MMM",
                },
                new TestCase
                {
                    CorrectOutputInt = 900,
                    RomanNumeralString = "CM",
                },
                new TestCase
                {
                    CorrectOutputInt = 1900,
                    RomanNumeralString = "MCM",
                },
                new TestCase
                {
                    CorrectOutputInt = 2900,
                    RomanNumeralString = "MMCM",
                },
                new TestCase
                {
                    CorrectOutputInt = 3900,
                    RomanNumeralString = "MMMCM",
                },
                new TestCase
                {
                    CorrectOutputInt = 2564,
                    RomanNumeralString = "MMDLXIV",
                },
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
            Ones = 0,
            Tens = 1,
            Hunds = 2,
            Thous = 3,
        }

        private enum Step
        {
            Trail = 0,
            Fiveish = 1,
            Tenish = 2,
            Lead = 0,
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
            // The only Thousands place char is 'M', and that is handled
            // in a special case of ProcessPlace
        };

        private static readonly string err =
            " is not a valid roman numeral that can be expressed between" +
            "1 (I) and 3999 (MMMCMXCIX).";

        // Improvement could be to ignore leading and trailing whitespace
        public static int RomanToInt(string str)
        {
            if (str == null)
                throw new ArgumentNullException("string str is null");

            if (str == String.Empty)
                throw new ArgumentException($"\"{str}\"{err}");

            int result = 0;
            var i = str.Length - 1;
            var place = Place.Ones;

            while (i >= 0 && place < Place.Thous)
            {
                // "Variable can be inlined"
                //int placeResult;
                i = ProcessPlace(place++, str, i, out int placeResult);
                result += placeResult;
                //++p; // Does work instead of:
                // p = (Place)((int)p + 1);
            }

            // Best way to refactor around while loop condition and this if check??
            if (i >= 0 && place == Place.Thous)
            {
                // NOTE: ProcessPlace represents the quantity 1000 in the Hunds place
                // when no other quantity would be represnted in the Hunds place
                // This parsing approach simplifies parsing for Ones and Tens, and also
                // requires an adjustment to parsing the Thous place

                // Examples:    M == 1000 (with M represented in the Hunds place)
                //              MC == 1100 (with M represented in the Thous place and C represented in the Hunds place)
                //              MMM == 3000 (with MM represented in the Thous place and M represented in the Hunds place)
                //              MMC == 2100 (with MM represented in the Thous place and C represented in the hunds place)
                // If result is already more than 1000 before the Thous place is handled,
                // then max Ms from the Thous place is two, otherwise three Ms are allowed from the Thous place

                int mLimit = result > 1000 ? 2 : 3;
                // Note that MMMM is an invalid roman numeral:
                // http://romannumerals.babuo.com/roman-numerals-1-5000
                
                var mCount = 0;
                for (; i >= 0; --i)
                {
                    if (str[i] == 'M')
                    {
                        result += 1000;
                        ++mCount;

                        if (mCount > mLimit)
                            throw new ArgumentException($"\"{str}\"{err}");
                    }
                    else
                        // Only Ms are valid at this point in str. Handles all
                        // cases where unparseable "garbage" is found in str
                        // Previous logic would give a subtotal of 0 and not
                        // advance i, so bad input finally gets caught here.
                        throw new ArgumentException($"\"{str}\"{err}");
                }
            }

            return result;
        }

        // Returns the index to the left of the last character processed
        // while parsing the current place of str
        // Handles Ones, Tens and Hunds places
        // Does not handle Thous place (handled as a special case up in RomanToInt)
        private static int ProcessPlace(Place place, string str, int idx, out int result)
        {
            result = 0;
            var i = idx;

            var placei = (int)place;
            var trailCount = 0;
            for (; i >= 0; --i)
            {
                // Add up the effect of the trailing Is, Xs, or Cs (depending on place being parsed)
                if (str[i] == NL[placei][(int)Step.Trail])
                {
                    result += 1;
                    ++trailCount;
                }
                else
                    break;
            }
            // Limit to three trailing repeated chars (disallow trailing "IIII", etc.)
            if (trailCount > 3)
                throw new ArgumentException($"\"{str}\"{err}");

            if (i >= 0)
            {
                // Count the effect of the V, L, or D
                if (str[i] == NL[placei][(int)Step.Fiveish])
                {
                    result += 5;
                    --i;
                } // OR count the effect of the X, C, or M
                else if (str[i] == NL[placei][(int)Step.Tenish])
                {
                    result += 10;
                    --i;
                }

                // Remove the effect of a leading I, X or C
                if (i >= 0 && str[i] == NL[placei][(int)Step.Lead])
                {
                    result -= 1;
                    --i;
                }
            }

            // Scale the accumulated effects according to the place
            // currently being parsed
            if (place == Place.Tens)
                result *= 10;
            else if (place == Place.Hunds)
                result *= 100;

            return i;
        }

    }
}
