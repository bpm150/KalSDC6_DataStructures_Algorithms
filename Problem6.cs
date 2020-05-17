using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    class Problem6
    {

        public static void RunTests()
        {
            int[] tempArrVar;

            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntArray = tempArrVar = new int[]{},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = -1,
                },
                new TestCase
                {
                    InputIntArray = new int[]{2, 2},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = -1,
                },
                new TestCase
                {
                    InputIntArray = new int[]{0},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = -1,
                },
                new TestCase
                {
                    InputIntArray = new int[]{5, 10, 15, 20, -20, 50},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = 3,
                },
                new TestCase
                {
                    InputIntArray = new int[]{-7, 1, 5, 2, -4, 3, 0},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = 3,
                },
                new TestCase
                {
                    InputIntArray = new int[]{2, 3, 1, 5},
                    ArrayLengthN = tempArrVar.Length,
                    EquilibriumIndex = 2,
                },
            };


            string intro =
                "==============\n" +
                "= Problem #6 =\n" +
                "==============\n" +
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

                Console.WriteLine($"Input: A[] = {Utility.CollectionToString(testCases[i].InputIntArray)}");
                Console.WriteLine($"Output: { testCases[i].EquilibriumIndex }");

                var testCaseResult = equilibrium(testCases[i].InputIntArray, testCases[i].InputIntArray.Length);

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

        public static int equilibrium(int[] arr, int n)
		{
			if (arr == null)
				throw new ArgumentNullException("Parameter int[] arr is null.");

			if (arr.Length != n)
				throw new ArgumentException("Length parameter n must match arr.Length");

			const int SENTINEL_VALUE = -1;

			if (n < 3) // Only arrays with 3 or more elements can possibly have an equilibrium index
				return SENTINEL_VALUE;


			// First possible equilibrium index is [1]

			// Sum of elements to the left of [1]
			var leftSum = arr[0];

			// Sum of elements to the right of [1]
			int rightSum = 0;
			for (var k = 2; k < n; ++k)
			{
				rightSum += arr[k];
			}

            // Remember the meaning of the integer return type is the array index of the equilibrium
			for (var i = 1; i < n - 1; ++i)
			{
				if (leftSum == rightSum)
					return i;

				// Update the sums to match i's next loop
				leftSum += arr[i];
				rightSum -= arr[i+1]; // Careful, there is a gap between leftSum and rightSum
                                        // That gap is the equilibium index
			}

			return SENTINEL_VALUE;
		}

		private class TestCase
		{
			public int[] InputIntArray { get; set; }

            public int ArrayLengthN { get; set; }

            public int EquilibriumIndex { get; set; }
		}

	}
}
