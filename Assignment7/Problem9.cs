using System;
using System.Collections.Generic;
using System.Linq;

// Spent an hour and 3 mins setting up the tests and the Node<T> type

// 2 hrs 3 mins to first actual test attempt of implementation that I thought might work

// Got to working for common cases at 2h 31 mins


// Whoops! Forgot about how can't set a generic constraint that T supports the binary '+'' operator
// Workarounds for this are an interesting conversation and not the point of this problem.

// Assumptions:
// Each node's Data is of type int
// Each node's Data is a number between 0 and 9, inclusive
// There is no way to represent negative numbers in this scheme
// Throw if either list is null

// Max value? Doesn't need to be Int32.MaxValue (we aren't storing the operands or the sums in Int32)
// At some point you'd run out of memory


// Observations:
// Numbers may have a diff number of digits (may encounter null in one number before the other)


namespace Assignment7
{
    class Problem9
    {

        public class IntNode : IEnumerable<int>
        {
            public int Data;
            public IntNode Next;

            public static IntNode ConstructNodeList(IEnumerable<int> input)
            {
                if (input == null || !input.Any())
                    return null;

                var dummyNode = new IntNode();
                var curr = dummyNode;

                foreach (var item in input)
                {
                    curr.Next = new IntNode();
                    curr = curr.Next;
                    curr.Data = item;
                }

                var nodeList = dummyNode.Next;
                return nodeList;
            }

            // Better to implement IEnumerable<int>, or create a ToArray method?
            // Let's see how NUnit responds to getting an IEnumerable<int> and an int[]

            // Convert from NodeList to collection
            public IEnumerator<int> GetEnumerator()
            {
                var curr = this;
                while (curr != null)
                {
                    yield return curr.Data;
                    curr = curr.Next;
                }
                // Magic (to review how yield works, specifically how it knows on the final iteration that it is the final iteration)
            }

            // This is important, at least in some situations
            // https://www.codeproject.com/articles/474678/a-beginners-tutorial-on-implementing-ienumerable-i
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
        }

        public static class Program
        {
            public static IntNode SumList(IntNode leftHead, IntNode rightHead)
            {
                if (leftHead == null || rightHead == null)
                    throw new ArgumentNullException("an operand is null");

                // ? Convenient to Ensure main logic starts with at least one node from each list
                // Not necessary.

                var currLeft = leftHead;
                var currRight = rightHead;

                // Don't think it helps to make a dummy head to chop later
                var dummyResultHead = new IntNode();
                var currResult = dummyResultHead;

                var carryOver = 0;
                // TODO: (Verify) Watch out for hitting null in one operand list before the other
                // (as when the number represented by one of the operand lists has more digits than the number represented by the other operand list)

                while (currLeft != null || currRight != null)
                {
                    currResult.Next = new IntNode();
                    currResult = currResult.Next;

                    var digitSum = carryOver;
                    digitSum += currLeft != null ? currLeft.Data : 0;
                    digitSum += currRight != null ? currRight.Data : 0;

                    if (digitSum <= 9)
                    {
                        // Fits in current digit of result
                        currResult.Data = digitSum;
                        carryOver = 0;
                    }
                    else
                    {
                        currResult.Data = digitSum % 10;
                        carryOver = 1;
                    }

                    // Null-conditional operator ?.
                    // https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-
                    // Advance currLeft and currRight 
                    currLeft = currLeft?.Next;
                    currRight = currRight?.Next;
                }


                // Now will need to make an extra node iff carryOver == 1
                // Better than keeping two refs instead of one for the result list

                if (carryOver == 1)
                {
                    currResult.Next = new IntNode();
                    currResult = currResult.Next;
                    currResult.Data = 1;
                }


                // Can still need carryover all the way to the end of the other list, even if one list runs out early
                // ? Way to keep using the same loop and elegantly continue even if one list has already run out
                // That one evaluate this unless it is null, then don't

                var resultHead = dummyResultHead.Next;
                return resultHead;
            }
        }


    }
}


// NUNIT TESTS

//namespace Testing
//{
//    using NUnit.Framework;
//    using System.Collections.Generic;
//    using Project;
//    using static Project.Program;
//    using System.Linq;
//    using System;

//    [TestFixture]
//    public class Tests
//    {
//        [Test]
//        public void KalTest()
//        {
//            SumTestHelperLong(345, 45);
//        }

//        [Test]
//        public void OneDigitTwoDigitsThreeDigitsSum()
//        {
//            SumTestHelperLong(4, 98);
//        }

//        [Test]
//        public void LeadingZeroes()
//        {

//            var leftNum = 345;
//            var rightNum = 45;
//            var expectedSum = leftNum + rightNum;

//            var leftIntNode = LongToIntNode(leftNum);

//            // Add a leading zero
//            var curr = leftIntNode;
//            while (curr.Next != null)
//                curr = curr.Next;
//            curr.Next = new IntNode();

//            var rightIntNode = LongToIntNode(rightNum);
//            var expectedSumIntNode = LongToIntNode(expectedSum);

//            foreach (var digit in leftIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");

//            foreach (var digit in rightIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");

//            foreach (var digit in expectedSumIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");


//            var sumListResult = SumList(leftIntNode, rightIntNode);

//            foreach (var digit in sumListResult.ToArray())
//                TestContext.Out.WriteLine(digit);

//            Assert.AreEqual(expectedSumIntNode.ToArray(),
//                            sumListResult.ToArray());
//        }


//        private void SumTestHelperLong(int leftNum, int rightNum)
//        {
//            var expectedSum = (long)leftNum + rightNum;

//            var leftIntNode = LongToIntNode(leftNum);
//            var rightIntNode = LongToIntNode(rightNum);
//            var expectedSumIntNode = LongToIntNode(expectedSum);

//            foreach (var digit in leftIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");

//            foreach (var digit in rightIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");

//            foreach (var digit in expectedSumIntNode.ToArray())
//                TestContext.Out.WriteLine(digit);
//            TestContext.Out.WriteLine("\n");


//            var sumListResult = SumList(leftIntNode, rightIntNode);

//            foreach (var digit in sumListResult.ToArray())
//                TestContext.Out.WriteLine(digit);

//            Assert.AreEqual(expectedSumIntNode.ToArray(),
//                            sumListResult.ToArray());
//        }


//        [Test]
//        public void NinesTest()
//        {
//            SumTestHelperLong(999, 999);
//        }

//        [Test]
//        public void SingleDigitOperandsMultiDigitSum()
//        {
//            SumTestHelperLong(1, 9);
//        }

//        [Test]
//        public void SingleDigitOperandsSingleDigitSum()
//        {
//            SumTestHelperLong(1, 4);
//        }

//        [Test]
//        public void IntMaxValueTest()
//        {
//            SumTestHelperLong(int.MaxValue, int.MaxValue);
//        }


//        [Test]
//        public void LongToIntNodeTest()
//        {
//            var numArr = new int[] { 4, 2, 5, };
//            var numArrReversed = IntNode.ConstructNodeList(numArr.Reverse());

//            Assert.AreEqual(numArrReversed,
//                            LongToIntNode(425).ToArray());
//        }

//        private IntNode LongToIntNode(long number)
//        {
//            // Assuming well-formed input: non-negative

//            var num = number;

//            var head = new IntNode();
//            var curr = head;

//            for (; ; )
//            {
//                curr.Data = (int)(num % 10);

//                num /= 10;

//                if (num > 0)
//                {
//                    curr.Next = new IntNode();
//                    curr = curr.Next;
//                }
//                else
//                    break;
//            }

//            return head;
//        }

//        [Test]
//        public void NullTest()
//        {
//            Assert.Throws<ArgumentNullException>(() => { SumList(null, null).ToArray(); });
//        }

//        [Test]
//        public void EmptyTest()
//        {
//            foreach (var num in SumList(new IntNode(), new IntNode()))
//                TestContext.Out.WriteLine(num);

//            Assert.AreEqual(new int[] { 0 }, SumList(new IntNode(), new IntNode()).ToArray());
//        }
//    }
//}
