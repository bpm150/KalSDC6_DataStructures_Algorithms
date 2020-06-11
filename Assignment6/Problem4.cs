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
                    CorrectOutputInt = 7,
                    RomanNumeralString = "VII",
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
        // Use new for hte inner arrays but not for the outer one?
        private static readonly char[][] NL =
            {
                new char[]{'I', 'V', 'X',},
                new char[]{'X', 'L', 'C',},
                new char[]{'C', 'D', 'M',},
            };


        public static int RomanToInt(string str)
        {
            int result = 0;
            var i = str.Length - 1;
            var p = Place.One;

            while (i >= 0 && p < Place.Tho)
            {
                // "Variable can be inlined"
                int newResult;
                i = ProcessPlace(p, str, i, out newResult);
                result += newResult;
                ++p; // Works instead of:
                // p = (Place)((int)p + 1);
            }
            for (var t = i; t >= 0; --t)
            {
                if (str[t] == 'M')
                    result += 1000;
            }
            return result;
        }

        private static int ProcessPlace(Place p, string str, int ind, out int result)
        {
            result = 0;
            var i = ind;

            for (; i >= 0; --i)
            {
                if (str[i] == NL[(int)p][0])
                    result += 1;
                else
                    break;
            }

            if (i >= 0)
            {
                if (str[i] == NL[(int)p][1])
                {
                    result += 5;
                    --i;
                }
                else if (str[i] == NL[(int)p][2])
                {
                    result += 10;
                    --i;
                }

                if (i >= 0 && str[i] == NL[(int)p][0])
                {
                    result -= 1;
                    --i;
                }
            }

            if (p == Place.Ten)
                result *= 10;
            else if (p == Place.Hun)
                result *= 100;

            return i;
        }

    }
}
