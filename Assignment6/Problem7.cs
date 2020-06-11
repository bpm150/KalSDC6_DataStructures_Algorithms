using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment6
{
    class Problem7
    {
        public static void RunTests()
        {
            int temp;

            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutputInt = temp = 24,
                    IntAsString = temp.ToString(),
                },
                new TestCase
                {
                    CorrectOutputInt = temp = 1024,
                    IntAsString = temp.ToString(),
                },
                new TestCase
                {
                    CorrectOutputInt = 64,
                    IntAsString = null,
                },
                new TestCase
                {
                    CorrectOutputInt = 64,
                    IntAsString = "64i",
                },
            };


            string intro =
                "==============\n" +
                "= Problem #7 =\n" +
                "==============\n" +
                "\n" +
                "Your task is to implement the function atoi." +
                "The function takes a string(str) as argument and converts" +
                "it to an integer and returns it.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: {testCases[i].IntAsString}");
                Console.WriteLine($"Output: { testCases[i].CorrectOutputInt }");

                var testCaseResult = atoi(testCases[i].IntAsString);

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
            public string IntAsString { get; set; }

            public int CorrectOutputInt { get; set; }
        }

        // Ways to extend:
        // Add in ignoring leading and trailing whitespace
        // Can either do with string library (simpler)
        // Actually IsWhiteSpace() is a utility method on the Char class that
        // returns a bool
        public static int atoi(string str)
        {
            if (str == null)
                throw new ArgumentNullException();

            int result = 0;

            for (var i = 1; i <= str.Length; ++i)
            {
                if (str[^i] < '0' || str[^i] > '9')
                    throw new ArgumentException("");

                result += (str[^i] - '0') * (int)Math.Pow(10, i - 1);
            }

            return result;
        }
    }
}
