using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using DataStructuresAndAlgos;

namespace Assignment6
{
    class Problem9
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    CorrectOutput = "to",
                    InputStrArr = new List<string>
                    {
                        "TOP",
                        "TO",
                    },
                },
                new TestCase
                {
                    CorrectOutput = "f",
                    InputStrArr = new List<string>
                    {
                        "FOR",
                        "FAN"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "f",
                    InputStrArr = new List<string>
                    {
                        "FOX",
                        "FOR",
                        "FAN"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        "HUG",
                        "ME"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "ap",
                    InputStrArr = new List<string>
                    {
                        "apple",
                        "ape",
                        "April"
                    },
                },
                new TestCase
                {
                    CorrectOutput = "foo",
                    InputStrArr = new List<string>
                    {
                        "foo",
                        "food"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        String.Empty,
                        "food"
                    },
                },
                new TestCase
                {
                    CorrectOutput = String.Empty,
                    InputStrArr = new List<string>
                    {
                        String.Empty,
                        String.Empty
                    },
                },
                //new TestCase
                //{
                //    CorrectOutput = String.Empty,
                //    InputStrArr = new List<string>
                //    {},
                //},
                new TestCase
                {
                    CorrectOutput = "sweet",
                    InputStrArr = new List<string>
                    {
                        "SWEET",
                        "SWEET"
                    },
                },
                //new TestCase
                //{
                //    CorrectOutput = "sweet",
                //    InputStrArr = new List<string>
                //    {
                //        "SWEET",
                //        null,
                //        "SWEET"
                //    },
                //},
            };

            string intro =
                "==============\n" +
                "= Problem #9 =\n" +
                "==============\n" +
                "\n" +
                "Given an array of n strings, find the longest common prefix.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");

                Console.WriteLine($"Input: \"{ Utility.CollectionToString(testCases[i].InputStrArr) }\"");
                Console.WriteLine($"Output: \"{ testCases[i].CorrectOutput }\"");

                var testCaseResult = LongestCommonPrefix(testCases[i].InputStrArr.ToArray());

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
            public List<string> InputStrArr { get; set; }
            public string CorrectOutput { get; set; }
        }

        // BUG:
        // Forgot to put the return type on the method
        public static string LongestCommonPrefix(string[] strArr)
        {
            if (strArr == null)
                throw new ArgumentNullException(
                    "parameter array is null");

            if(strArr.Length == 0)
                throw new ArgumentException(
                    "parameter array is empty");

            // Fixed more elegantly with FirstOrDefault() below
            // BUG:
            // Can't call First() on an empty sequence/collection
            // The for loops are safe, we reach for shortestStr first
            //if (strArr.Length == 0)
            //    return String.Empty;

            // TODO:
            // OH! We still need to check and make sure each individual
            // string in the array is not null
            // Executive decision is to skip over null strings
            // Give the correct answer for all the non-null strings
            // in the array



            // Nah...as cute as the linq is, it is turning out to be a hassle
            // More trouble than it is worth, etc.
            // Ask Linq to skip over any null strings
            // We will throw for null strings, but can't do that inside a linq
            // query...and even if we could...that sounds ugly, so let's not

            //var shortestStr = String.Empty;
            var shortestStr = String.Empty;
            for (var k = 0; k < strArr.Length; ++k)
            {
                if (strArr[k] == null)
                    throw new ArgumentNullException(
                        "param array contains a null string");

                if (k == 0)
                    shortestStr = strArr[k];
                else if (strArr[k].Length < shortestStr.Length)
                    shortestStr = strArr[k];
            }

            //var shortestStr = String.Empty;
            //for(var k = 0; k < strArr.Length; ++k)
            //{ 
            //    if (strArr[k] == null)
            //        throw new ArgumentNullException(
            //            "param array contains a null string");

            //    if (k == 0)
            //        shortestStr = strArr[k];
            //    else if (strArr[k].Length < shortestStr.Length)
            //        shortestStr = strArr[k];
            //}

            //var shortestStr = String.Empty;
            //foreach (var str in strArr)
            //{
            //    if(str == null)
            //        throw new ArgumentNullException(
            //            "param array contains a null string");

            //    shortestStr = str.Length < shortestStr.Length ? str : shortestStr;
            //}
            //var shortestStr = strArr.Where(str => str != null).
            //                            OrderBy(str => str.Length).
            //                            FirstOrDefault();
            // FIX: First() -> FirstOrDefault()
            // If there strArr is empty, or contains only nulls
            // Get String.Empty

            for (var j = 0; j < shortestStr.Length; ++j)
            {
                // NVM:
                // compCh will get properly assigned a value before being read from
                // Compiler's static analysis suggests that this doesn't happen
                // so have to assign something to it, even though that value
                // never gets read
                // Well, if strArr.Length == 0, the inner for loop would not
                // execute, but then compCh would not get read from either,
                // so idk
                var compCh = Char.ToLower(strArr[0][j]);
                for (var i = 1; i < strArr.Length; ++i)
                {
                // Normally would do this inside the outer loop
                // and outside the inner loop,
                // But need to check the strings in the array for null
                // and right inside the inner loop is the right place
                // to do that if you want to do it only once...like I do
                    var currCh = Char.ToLower(strArr[i][j]);
                    if (compCh != currCh)
                        // BUG:
                        // Index/Range syntax is:
                        // ".." (two dots) not "..." (three dots)
                        // I think it was Ruby that had both two dots and
                        // three dots, where each meant something different
                        return strArr[0][..j].ToLower();
                }
            }
            return shortestStr.ToLower();
        }

    }
}
