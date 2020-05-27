using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4
{
    public class Problem9
    {
		public static void RunTests()
        {
			// TODO: Find out what is the best approach to wanting a single collection
			// of TestCase that are templatized on different T types

            var testCasesInt = new List<TestCase<int>>
			{
				new TestCase<int>
				{
					Test2DArray = new int[][]
						{ new int[]{  1,  2,  3,  4 },
						  new int[]{  5,  6,  7,  8 },
						  new int[]{  9, 10, 11, 12 },
						  new int[]{ 13, 14, 15, 16 }},
					CorrectSpiralString = "1 2 3 4 8 12 16 15 14 13 9 5 6 7 11 10",
				},
				new TestCase<int>
				{
					Test2DArray = new int[][]
						{ new int[]{  1,  2,  3,  4,  5,  6},
						  new int[]{  7,  8,  9, 10, 11, 12 },
						  new int[]{ 13, 14, 15, 16, 17, 18 }},
					CorrectSpiralString = "1 2 3 4 5 6 12 18 17 16 15 14 13 7 8 9 10 11",
				},
				new TestCase<int>
				{
					Test2DArray = new int[][]
						{ new int[]{  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12 },
						  new int[]{ 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 },
						  new int[]{ 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 },
						  new int[]{ 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 },},
					CorrectSpiralString = "1 2 3 4 5 6 7 8 9 10 11 12 24 36 48 47 46 45 44 43 42 41 40 39 38 37 25 13 14 15 16 17 18 19 20 21 22 23 35 34 33 32 31 30 29 28 27 26",
				},
			};

			var testCasesString = new List<TestCase<string>>
			{
				new TestCase<string>
				{
					Test2DArray = new string[][]
						{ new string[]{  "one",  "two",  "three",  "four" },
						  new string[]{  "five", "six",  "seven",  "eight" },
						  new string[]{  "nine", "ten", "eleven", "twelve" },
						  new string[]{ "thirteen", "fourteen", "fifteen", "sixteen" }},
					CorrectSpiralString = "one two three four eight twelve sixteen fifteen fourteen thirteen nine five six seven eleven ten",
				},
			};


			string intro =
                "==============\n" +
                "= Problem #9 =\n" +
                "==============\n" +
                "\n" +
                "Given a 2D array, print it in spiral form.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCasesInt.Count; ++i)
            {
                Console.WriteLine($"\nInt Test #{i + 1}:");


                Console.WriteLine($"For the array: ");
                foreach (var row in testCasesInt[i].Test2DArray)
                {
                    Console.WriteLine($"{Utility.CollectionToString(row)}");                  
                }

                Console.WriteLine($"The correct result is: {testCasesInt[i].CorrectSpiralString}.");

                var testCaseResult = Print2DArrayInSpiralForm(testCasesInt[i].Test2DArray);

                string resultMessage;

				Console.WriteLine($"Your answer is:        {testCaseResult}.");


				if (testCaseResult == testCasesInt[i].CorrectSpiralString)
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}!");
            }

			for (var i = 0; i < testCasesString.Count; ++i)
			{
				Console.WriteLine($"\nString Test #{i + 1}:");


				Console.WriteLine($"For the array: ");
				foreach (var row in testCasesString[i].Test2DArray)
				{
					Console.WriteLine($"{Utility.CollectionToString(row)}");
				}

				Console.WriteLine($"\nThe correct result is: {testCasesString[i].CorrectSpiralString}.");

				var testCaseResult = Print2DArrayInSpiralForm(testCasesString[i].Test2DArray);

				string resultMessage;

				Console.WriteLine($"\nYour answer is:        {testCaseResult}.\n");


				if (testCaseResult == testCasesString[i].CorrectSpiralString)
				{
					resultMessage = "SUCCESS";
				}
				else
				{
					++testOopsCount;
					resultMessage = "OOPS";
				}

				Console.WriteLine($"{resultMessage}!");
			}

			var testCount = testCasesInt.Count + testCasesString.Count;
            var testSuccessCount = testCount - testOopsCount;

            Console.WriteLine($"\n\nOut of {testCount} tests total,\n");
            Console.WriteLine($"{testSuccessCount}/{testCount} tests succeeded, and");
            Console.WriteLine($"{testOopsCount}/{testCount} tests oopsed.\n");

            if (testOopsCount == 0)
            {
                Console.WriteLine($"YAY! All tests succeeded! :D\n");
            }
        }



		// Possible to declare an enum in local (method/function) scope?
		// Apparently not. But can still make it inner to a class and private
		// to that class (in this case, class Problem9) 
		private enum Direction
		{
			RIGHT = 0,
			DOWN = 1,
			LEFT = 2,
			UP = 3,
		};

		public static string Print2DArrayInSpiralForm<T>(T[][] arr)
		{
			// What is possible in terms of nulls with 2D arrays in C#?
			// Certainly arr can be null
			// If arr is not null, then is arr[0] safe?
			// TODO: Can arr[0] be null?

			if (arr == null || arr[0] == null)
				throw new ArgumentNullException("Parameter int[][] arr or row arr[0] is null.");

			int length = arr[0].Length;
			for (var i = 1; i < arr.Length; ++i)
			{
				// TODO: How about arr[x]? Must check for null every time, I think?
				if (arr[i] == null)
					throw new ArgumentNullException("Parameter int[][] contains a null row.");

				// Kal guaranteed that array is truly 2D, is not jagged, but here were my two approach
				// ideas for how to protect against jagged-ness:

					//if (arr[i].Length != length)
					//	throw new ArgumentException("A jagged array cannot be printed in spiral form.");

					//if(arr.Select(r => r.Length).Distinct().Count() > 1)
					//	throw new ArgumentException("Jagged array cannot be printed in spiral form.");
					// Hrm, but this doesn't allow to easily throw for null rows the way the foreach approach does
			}
			// TODO: Cleverly integrate null checks for the first access of an element (column) in
			// a row that hasn't been accessed before
			// This would be in the first phase


			// Up, Right, Down, Left
			const int STEPS_PER_PHASE = 4;

			var currCoords = new Coord(0, 0);
			//var ssBuilder = new StringBuilder();
			var ssBuilder = new StringBuilder($"{arr[currCoords.i][currCoords.j]} ");

			for (var step = 0; currCoords != null; ++step)
			{
				// Note that neither steps nor phases "cycle" around to be small numbers again
				// both increase without bound (until currCoords == null) 
				var phase = step / STEPS_PER_PHASE;

				// Had this written with const locals
				// Then converted to enum
				// WAS BUG of forgetting to do the required type casting to/from the Direction type

				// Direction cycles 0, 1, 2, 3 during each phase (four steps per phase)
				var direction = (Direction)(step % STEPS_PER_PHASE);

				switch (direction)
				{
					case Direction.RIGHT:
						currCoords = GoRight(arr, currCoords, phase, ref ssBuilder);
						break;
					case Direction.DOWN:
						currCoords = GoDown(arr, currCoords, phase, ref ssBuilder);
						break;
					case Direction.LEFT:
						currCoords = GoLeft(arr, currCoords, phase, ref ssBuilder);
						break;
					case Direction.UP:
						currCoords = GoUp(arr, currCoords, phase, ref ssBuilder);
						break; // Remember: Must break at the end of a case in a switch case
				}
			}

			// Drop the single trailing space
			ssBuilder.Remove(ssBuilder.Length - 1, 1);

			return ssBuilder.ToString();
		}





		// Wish I could pass arr by const ref...what if arr is large?
		private static Coord GoRight<T>(T[][] arr, Coord currCoords, int phase, ref StringBuilder ssBuilder)
		{
			if (ssBuilder == null)
				ssBuilder = new StringBuilder();

			var j = currCoords.j + 1;

			// WAS BUG
			var jMax = arr[0].Length - (phase + 1);
			// Right and down need (phase + 1) instead of phase in order to use the same "Max"
			// pattern as Up and Left (see diagram)

			// Can't go right. All elements already added to string.
			if (j > jMax)
				return null;

			for (; j <= jMax; ++j)
			{
				ssBuilder.Append($"{arr[currCoords.i][j]} ");
			}

			// Move j back to actual that was used for the lookup and Append
			return new Coord(currCoords.i, --j);
		}


		private static Coord GoDown<T>(T[][] arr, Coord currCoords, int phase, ref StringBuilder ssBuilder)
		{
			if (ssBuilder == null)
				ssBuilder = new StringBuilder();

			var i = currCoords.i + 1;

			// WAS BUG
			var iMax = arr.Length - (phase + 1);
			// Right and down need (phase + 1) instead of phase in order to use the same "Max"
			// pattern as Up and Left (see diagram)

			// Can't go down. All elements already added to string.
			if (i > iMax)
				return null;


			for (; i <= iMax; ++i)
			{
				ssBuilder.Append($"{arr[i][currCoords.j]} ");
			}

			// Move i back to actual that was used for the lookup and Append
			return new Coord(--i, currCoords.j);
		}


		private static Coord GoLeft<T>(T[][] arr, Coord currCoords, int phase, ref StringBuilder ssBuilder)
		{
			if (ssBuilder == null)
				ssBuilder = new StringBuilder();

			var j = currCoords.j - 1;

			// BUG, WAS:
			//var jMin = arr[0].Length - phase;
			var jMin = phase;

			// Can't go left. All elements already added to string.
			if (j < jMin)
				return null;

			for (; j >= jMin; --j)
			{
				ssBuilder.Append($"{arr[currCoords.i][j]} ");
			}

			// Move j back to actual that was used for the lookup and Append
			return new Coord(currCoords.i, ++j);
		}


		private static Coord GoUp<T>(T[][] arr, Coord currCoords, int phase, ref StringBuilder ssBuilder)
		{
			// TODO: Any other error cases to handle?

			if (ssBuilder == null)
				ssBuilder = new StringBuilder();

			var i = currCoords.i - 1;

			// Hard to explain, pls see diagram
			var iMin = phase + 1;

			// Can't go up. All elements already added to string.
			if (i < iMin)
				return null;

			for (; i >= iMin; --i)
			{
				// Is it Append or Add?
				ssBuilder.Append($"{arr[i][currCoords.j]} ");
			}

			// Move i back to actual that was used for the lookup and Append
			return new Coord(++i, currCoords.j);
		}


		// Or could be struct, but then we'd need to use nullabe for the return
		// type on each of the Go Up/Right/Down/Left methods

		// Also, the internet makes it clear that "mutable structs are evil"
		private class Coord
		{
			public Coord(int _i, int _j)
			{
				i = _i;
				j = _j;
			}

			public int i { get; set; }
			public int j { get; set; }
		}


		private class TestCase<T>
        {
            public T[][] Test2DArray { get; set; }

            public string CorrectSpiralString { get; set; }
        }
    }

}
