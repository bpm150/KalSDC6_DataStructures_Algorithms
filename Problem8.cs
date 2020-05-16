﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Assignment4
{
    class Problem8
    {

		public static void RunTests()
		{
			var testCases = new List<TestCase>
			{
				new TestCase
				{
					InputIntArray = new int[]{7, 10, 4, 3, 20, 15},
					K = 3,
					OutputKthSmallestElement = 7,
				},
				new TestCase
				{
					InputIntArray = new int[]{7, 10, 4, 3, 20, 15},
					K = 4,
					OutputKthSmallestElement = 10,
				},
			};


			string intro =
				"==============\n" +
				"= Problem #8 =\n" +
				"==============\n" +
				"\n" +
				"Given an array and a number k where k is smaller than size of array, " +
				"we need to find the k’th smallest element in the given array. " +
				"It is given that all array elements are distinct..";

			Console.WriteLine(intro);

			int testOopsCount = 0;

			for (var i = 0; i < testCases.Count; ++i)
			{
				Console.WriteLine($"\nTest #{i + 1}:");

				Console.WriteLine($"Input: A[] = {Utility.CollectionToString(testCases[i].InputIntArray)}");
				Console.WriteLine($"Output: { testCases[i].OutputKthSmallestElement }");

				var testCaseResult = FindKthSmallestElement(testCases[i].InputIntArray, testCases[i].K);

				string resultMessage;

				if (testCaseResult == testCases[i].OutputKthSmallestElement)
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



		public static int FindKthSmallestElement(int[] _arr, int k)
		{
			var n = _arr.Length;

			if (_arr == null)
				throw new ArgumentNullException("Paremeter int[] arr is null.");

			if (n == 0)
				throw new ArgumentException("Parameter int[] arr contains no elements.");

			if (k < 1 || k > n) // Remember that element at k-1 is the kth smallest element
				throw new ArgumentException("Parameter int k is out of bounds.");

			// Protect the caller's array
			var arr = (int[])_arr.Clone();
			// Well, the clone is O(n)
			// That brings us up to 2 * O(n), which is still linear

			// Remember that you have to cast when you clone an array
			// as Clone returns object type
			// Clone is a shallow copy (whether the array contains reference types or value types)

			int iLeft = 0;          // Starts left, walks right (++)
			int iRight = n - 1;     // Starts right, walks left (--)
									// Note that the Pivot isn't set until inside the helper call 
			int iKthSmallest = k - 1;   // Index of element that we want in the sorted set

			// WAS BUG: Forgot part of the helper method name
			return _Helper_FindKthSmallestElement(arr, iLeft, iRight, iKthSmallest);

		}   // end FindKthSmallestElement


		// Write a swap helper method, or not? dunno. it's not that much code and its kind of nice to see all the things right there



		private static int _Helper_FindKthSmallestElement(int[] arr, int iLeftBound, int iRightBound, int iKthSmallest)
		{
			// WAS BUG:
			// length of arr doesn't matter in here
			// caller has already specified set/partition on which to operate
			//var n = arr.Length;



			// Don't need to protect arr, it's not the user's array, it's our clone
			// actually probably want to make it private class data so we don't keep putting copies of the array on the call stack


			// **TODO: Come back and fix this median-of-three logic for when n is too small to use it
			// ?? How few elements in a set/partition before you don't bother with the median-of-three step anymore?
			// begin by assuming that there are enough elements to do this, then...

			// PHASE 0: Median-of-three
			// Adds work to each iteration/recursion of the algorithm, but that work is done in O(1) constant time, so it doesn't impact the time complexity of the algorithm

			// Determine the median of the first, middle and last elements of the array, use that median as the first pivot
			// (handle cases where there are an even number of elements, either of the middle elements will do, this is not a precise process)
			//	If chose the first element of the array or the middle element of the array, swap it with the last element of the array


			// At how small of a set should we stop bothering to median-of-three?
			// Three is the smallest set where it is possible
			// Is there a larger count of elements where is best to not bother and simply use the Right element as pivot as-is?
			var elementsInSet = iRightBound - iLeftBound + 1;

			if (elementsInSet >= 3)
			{
				var iMiddle = iLeftBound + ((iRightBound - iLeftBound) / 2);

				// Using the XOR approach to median:

				// Left is median (greater than exclusively one of middle or last (but not both of them)
				if ((arr[iLeftBound] > arr[iMiddle]) != (arr[iLeftBound] > arr[iRightBound]))
				{
					var temp = arr[iRightBound];
					arr[iRightBound] = arr[iLeftBound];
					arr[iLeftBound] = temp;

				}
				// Middle is median (greater than exclusively one of first or last (but not both of them)
				else if ((arr[iMiddle] > arr[iLeftBound]) != (arr[iMiddle] > arr[iRightBound]))
				{
					var temp = arr[iRightBound];
					arr[iRightBound] = arr[iMiddle];
					arr[iMiddle] = temp;
				}
				// else Right is median, thus it is already in place to be the pivot of this cycle



			}

			// Ahead of the element comparisons,	
			// RECALL THAT ALL ELEMENTS OF arr are defined to be unique by the problem, that is, there are no i and j such that arr[i] == arr[j]


			// WAS BUG: Was setting the Pivot wrt the entire arr
			// should be the right most index of the set/partition that we are about to sort
			//var iPivot = n - 1;
			var iPivot = iRightBound; // Index of value that we will compare all elements in the set to during a cycle
			//PHASE 1:
			// COME BACK AND TEST WTIH SIMPLE PIVOT SELECT IF MEDIAN OF THREE ISN'T WORKING OUT FOR YA

			// Create working copies of params to mutate
			// Will need original param values for generating recursive calls later
			var iLeft = iLeftBound;
			// WAS BUG: Harder to notice since the Pivot was being set incorrectly, too...
			// Right should always start just to the left of the Pivot
			// (using rightmost index of partition as iPivot after swapping
			// the median into arr[iPivot] if needed)
			var iRight = iPivot - 1;    // Start walking to the left just from the left of iPivot




			// ASSERT:
			// Assure that at this point median-of-three logic has already been applied and the appropriate value has been swapped to arr[iPivot]

			// While arr[iLeft] and arr[iRight] still have swaps to do
			while (iLeft < iRight)
			{
				// TODO: CAUTION:
				// It's not possible for arr[iLeft] == arr[iPivot] unless iLeft == iPivot
				// Concerns about walking too far, or in a case where we don't mean to?

				while (arr[iLeft] < arr[iPivot])    // Then element arr[iLeft] belongs where it currently is, it won't be swapped with either arr[iPivot] or arr[iRight] 
					++iLeft;            // Keep advancing to the right until we find an element that needs swapping
										// Now arr[iLeft] > arr[iPivot], which means that arr[iLeft] is about to be swapped with either arr[iRight] or arr[iPivot]

				// ?? Only possible for iLeft to increment all the way up to be equal to iPivot if the median-of-three wasn't applied...
				// ?? OR AT NEAR-FINISH TRIVIAL CASE...WILL HAVE TO STEP THROUGH THIS AND WATCH IT PLAY OUT

				// arr[iLeft] > arr[iPivot] 	// Wait for either swap with arr[iRight] or arr[iPivot] (the latter only in the case where iLeft == iRight before swapping arr[iPivot], since our pivot is on the right
				// ** BUT WAIT, OUR PIVOT MAY NOT ALWAYS BE ON THE RIGHT...DEPENDS WHERE K-1 IS AT THE END OF A CYCLE
				//{
					// 	Nothing?
				//}


				while (arr[iRight] > arr[iPivot])   // While element arr[iRight] belongs where it currently is, that is, while it doesn't need to be swapped with either arr[iPivot] or arr[iLeft]
					--iRight;           // Keep advancing to the left until we find an element that needs swapping
										// Now arr[iRight] < arr[iPivot], which means that arr[iRight] is about to be swapped with either arr[iLeft] or arr[iPivot]


				// If iLeft and iRight haven't yet met either by touching (as adjacent) or colliding (as equivelent)
				//if(iLeft < iRight - 1) // At least one index between
				//{

				//}
				var temp = arr[iRight];

				// WAS BUG: 
				// arr[iRight++] = arr[iLeft]; // 
				arr[iRight--] = arr[iLeft]; // Advance right to the left
				arr[iLeft++] = temp; // Advance left to the right
									 // Swap their values, then advance


			} // end while(true)		

			// If they walked past each other, undo their last steps before the rest of the logic
			// Does this happen every time, or not when they would end up on top of each other
			// in the (? uncommon) case where the pivot is on the end?
			if (iRight == iLeft - 1)
			{
				++iRight;
				--iLeft;
			}


			// iLeft and iRight walked towards each other and swapped until they ended up adjacent
			// arr[iLeft] is still < arr[iPivot] and arr[iRight] is still > arr[iPivot]
			if (iLeft == iRight - 1) // TODO: Fix this redundant if check (redundant to above)
				{
					// Swap element arr[iPivot] with either arr[iLeft] or arr[iRight] to put it in its final place
					// (Depending on whther iPivot is the rightmost or the leftmost index in the current set/partition)
					if (iPivot > iRight)
					{
						// WAS BUG:
						// "A local or parameter named 'temp' cannot be decalred in this scope because that name
						// is used in an enclosing local scope to define a local or a parameter
						var temp2 = arr[iRight];
						arr[iRight] = arr[iPivot];
						arr[iPivot] = temp2;
						var iPivotSwapDest = iRight;

						// Element arr[iRight] is now in its final position
						// All elements to the left of it are smaller
						// All elements to the right of it are larger

						// Stop if we found it
						if (iPivotSwapDest == iKthSmallest)
							return arr[iKthSmallest]; // DONE

						// HERE IS WHERE YOU RECURSE.
						// Make sure you pass in the correct indicies for the new set/partition
						// Remember to only recurse on the side that has contains k-1
						// Ignore/discard/don't sort the other side

						if (iKthSmallest < iPivotSwapDest)
						{
							return _Helper_FindKthSmallestElement(arr, iLeftBound, iPivotSwapDest - 1, iKthSmallest); // Recurse on the left set/partition (only), permanently ignoring/discarding/not sorting the right					
						}
						else
						{
							// WAS BUG? When omit curly braces for an if statement, if the if clause is a return statement, then no else can be paired with that if?
							//else // iKthSmallest > iRight
							return _Helper_FindKthSmallestElement(arr, iPivotSwapDest + 1, iRightBound, iKthSmallest); // Recurse on the right set/partition (only), permanently ignoring/discarding/not sorting the left					
						}

					}
					else // if iPivot < iLeft // TODO: Check to see if this would ever happen in logic as written, I think maybe not
					{
						// WAS BUG:
						// "A local or parameter named 'temp' cannot be decalred in this scope because that name
						// is used in an enclosing local scope to define a local or a parameter
						var temp3 = arr[iLeft];
						arr[iLeft] = arr[iPivot];
						arr[iPivot] = temp3;
					}
				}
				// Element arr[iPivot] is already in its final place, and has been for this entire cycle...
				// I think this can't happen unless we are almost done and/or the median-of-three wasn't performed

				// XX vv I think this else if block can be merged down to the scope above it if all of this gets re-run as (? tail-end) recursion rather than a while true loop
				// Hrm, wait, I think we do need the loop because after every swap of arr[iLeft] and arr[iRight] where there remain indices between them afterwards will require looping around to check and advance them again
				else if (iLeft == iRight)
				{
					// Bummer. No swaps needed at all this cycle. Our pivot was pretty bad (one of the two possible worst)

					// Stop if we found it
					if (iPivot == iKthSmallest)
						return arr[iKthSmallest];

					// HERE IS WHERE YOU RECURSE.
					// Make sure you pass in the correct indicies for the new set/partition
					// In this case, there was no L/R partitions created, only a single partition that must now contain k-1	

					// Collided with iPivot to the right (which I think is the only way for to happen, as written)
					if (iRight == iPivot - 1)
						return _Helper_FindKthSmallestElement(arr, iLeftBound, iPivot - 1, iKthSmallest);
				}



			throw new Exception("Whoops! Don't know how we got all the way down here.");

		}
		//end _Helper_FindKthSmallestElement



		private class TestCase
		{
			public int[] InputIntArray { get; set; }

			// "kth smallest element"
			public int K { get; set; }

			public int OutputKthSmallestElement { get; set; }
		}

	}
}
