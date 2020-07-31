using DataStructuresAndAlgos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerRank
{
    public class SampleTest
    {
        public static void RunTests()
        {
             var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntList = new List<int>{5, 10, 15, 20, -20, 50},
                    EquilibriumIndex = 3,
                },
                new TestCase
                {
                    InputIntList = new List<int>{5, 10, 15, 20, -20, 50},
                    EquilibriumIndex = 3,
                },
                new TestCase
                {
                    InputIntList = new List<int>{-7, 1, 5, 2, -4, 3, 0},
                    EquilibriumIndex = 3,
                },
                new TestCase
                {
                    InputIntList = new List<int>{2, 3, 1, 5},
                    EquilibriumIndex = 2,
                },
                new TestCase
                {
                    InputIntList = new List<int>(),
                    EquilibriumIndex = -1,
                },
                new TestCase
                {
                    InputIntList = new List<int>{2, 2},
                    EquilibriumIndex = -1,
                },
                new TestCase
                {
                    InputIntList = new List<int>{0},
                    EquilibriumIndex = -1,
                },
            };


            string intro =
                "=======================================\n" +
                "= Outco Fundamentals Check Problem #2 =\n" +
                "=======================================\n" +
                "\n" +
                "Equilibrium index of an array is an index such that the sum of elements at " +
                "lower indexes is equal to the sum of elements at higher indexes." +
                "Write a function int equilibrium(int[] arr, int n); that given a sequence " +
                "arr[] of size n, returns an equilibrium index (if any) or -1 if no " +
                "equilibrium indexes exist.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: A[] = {Utility.CollectionToString(testCases[i].InputIntList)}");
                Console.WriteLine($"Output: { testCases[i].EquilibriumIndex }");

                var testCaseResult = FindFulcrum(testCases[i].InputIntList);

                string resultMessage;

                if (testCaseResult == testCases[i].EquilibriumIndex)
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
            public List<int> InputIntList { get; set; }
            public int EquilibriumIndex { get; set; }
        }
        // I think that there is more to the problem than simply my end-case comparison in the while loop
        // Entire main loop structure needs to get refactored to fix the bug, I think

        public static int FindFulcrum(List<int> numbers)
        {
            // TODO: check for base cases

            const int NO_FULCRUM_EXISTS = -1;
            if (numbers.Count() == 0)
                return NO_FULCRUM_EXISTS;

            // Assume at least three elements for now

            // Set up the first comparison for the first candiate fulcrum:
            // First leftSideSum is element [0]
            // First candidateFulcrum is element [1]
            // First rightSideSum is sum of elements [2] through [^1]

            var leftSideSum = numbers[0];

            const int FIRST_CANDIDATE_FULCRUM = 1;
            var currentCandidateFulcrumIndex = FIRST_CANDIDATE_FULCRUM;

            var rightSideSum = 0;
            for (var i = FIRST_CANDIDATE_FULCRUM + 1; i < numbers.Count(); ++i)
                rightSideSum += numbers[i];

            // Last possible fulcrum is the next-to-last element
            while (currentCandidateFulcrumIndex < numbers.Count() - 1)
            {
                if (leftSideSum == rightSideSum)
                    return currentCandidateFulcrumIndex;
                // Return the candidate fulcrum, as it is the first occurring actual fulcrum

                // Update sums for next iteration
                leftSideSum += numbers[currentCandidateFulcrumIndex];
                rightSideSum -= numbers[currentCandidateFulcrumIndex++ + 1];
            }

            return NO_FULCRUM_EXISTS;
        }
    }
}