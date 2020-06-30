using System;
using System.Collections.Generic;
using System.Text;

using RelatedPractice;

namespace Assignment7
{
    class Problem1
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutput = 1,
                    InputSLList = new SLList<int>{1},
                },
                new TestCase
                {
                    CorrectOutput = 2,
                    InputSLList = new SLList<int>{1,2},
                },
                new TestCase
                {
                    CorrectOutput = 2,
                    InputSLList = new SLList<int>{1,2,3},
                },
                new TestCase
                {
                    CorrectOutput = 3,
                    InputSLList = new SLList<int>{1,2,3,4},
                },
                new TestCase
                {
                    CorrectOutput = 3,
                    InputSLList = new SLList<int>{1,2,3,4,5},
                },
                new TestCase
                {
                    CorrectOutput = 4,
                    InputSLList = new SLList<int>{1,2,3,4,5,6},
                },

                //new TestCase
                //{
                //    CorrectOutput = -1,
                //    InputSLList = new SLList<int>(),
                //},
            };

            string intro =
                "==============\n" +
                "= Problem #1 =\n" +
                "==============\n" +
                "\n" +
                "Given a singly linked list, find middle of the linked list. " +
                "If there are an even number of nodes, return the second of the" +
                " two middle elements.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ (testCases[i].InputSLList) }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectOutput }\"");

                // GetMiddle implemented on SLList in RelatedPractice\SLList.cs
                var testCaseResult = testCases[i].InputSLList.GetMiddle();

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
            public SLList<int> InputSLList { get; set; }
            public int CorrectOutput { get; set; }
        }

    }
}
