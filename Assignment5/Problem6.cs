using DataStructuresAndAlgos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace Assignment5
{
    class Problem6
    {
        public static void RunTests()
        {

            // Can't cast from it to char because truncation?
            // unchecked would work, but doing a proper conversion
            // with the Convert class is better
                var testCases = new List<TestCase>
                {
                    new TestCase
                    {
                        InputAsString = "sstnhtn",
                        CorrectNREAfterEachRead = new int[]{ 's', -1, 't', 't', 't', 'n', 'h' },
                    },
                };

            string intro =
                "==============\n" +
                "= Problem #6 =\n" +
                "==============\n" +
                "\n" +
                "Given an input stream of n characters consisting only of " +
                "small case alphabets the task is to find the first non " +
                "repeating character each time a character is inserted to " +
                "the stream. If no non repeating element is found print -1.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            for (var i = 0; i < testCases.Count; ++i)
            {
                Console.WriteLine($"\nTest #{i + 1}:");


                Console.WriteLine($"For the stream: {testCases[i].InputAsString}");
                //Console.WriteLine($"The correct result is: {testCases[i].CorrectNREAfterEachRead}");

                var testCaseResult = PrintFirstNRE(testCases[i].InputAsStream);

                string resultMessage;

                if (Enumerable.SequenceEqual(testCaseResult, testCases[i].CorrectNREAfterEachRead))
                {
                    resultMessage = "SUCCESS";
                }
                else
                {
                    ++testOopsCount;
                    resultMessage = "OOPS";
                }

                Console.WriteLine($"{resultMessage}!");
                //Console.WriteLine($"Your answer is:        {testCaseResult}");
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


        private class LetterTracker
        {
            const int LETTER_COUNT = 'z' - 'a';
            private readonly OrderedLetter[] orderedLetters;

            public LetterTracker()
            {
                orderedLetters = new OrderedLetter[LETTER_COUNT];
                for (var i = 0; i < LETTER_COUNT; ++i)
                {
                    orderedLetters[i] = new OrderedLetter();
                }
            }

            public int GetNRE()
            {
                var queryNRE = orderedLetters
                    .Where(ol => ol.HaveSeen == true)
                    .Where(ol => ol.DoesRepeat == false)
                    .OrderBy(ol => ol.FirstSeenOrder);

                if (queryNRE.Count() == 0)
                {
                    return NO_NRE;
                }

                var newNRE = queryNRE.First();

                // Reverse lookup to figure out what letter newNRE is
                // An alternative to this is for each OrderedLetter to know what letter
                // it represents. I like this way better.
                for (var i = 0; i < orderedLetters.Length; ++i)
                {
                    if (ReferenceEquals(orderedLetters[i], newNRE))
                        return 'a' + i;
                }

                throw new ArgumentOutOfRangeException(
                    "An element in orderedLetters met the query criteria," +
                    "but that element isn't in orderedLetters? Wat.");
            }

            // Do we need HaveSeen?
            // Maybe we only need to know if we've seen it when seeing it
            // with SeeLetter
            public bool HaveSeen(int letter)
            {
                ValidateLetter(letter);

                return orderedLetters[letter - 'a'].HaveSeen;
                // Add 'a' before returning letter value back to the user
                // subtract 'a' after getting letter value from the user,
                // before using it
            }

            // Returns true if this is a new sighting of the letter
            public bool SeeLetter(int letter)
            {
                ValidateLetter(letter);

                return orderedLetters[letter - 'a'].SeeThisLetter();
            }

            public bool DoesRepeat(int letter)
            {
                ValidateLetter(letter);

                return orderedLetters[letter - 'a'].DoesRepeat;
            }

            private class OrderedLetter
            {
                private static int nextSeenOrderValue = 0;

                const int NOT_SEEN = -2;

                public OrderedLetter()
                {
                    FirstSeenOrder = NOT_SEEN;
                    DoesRepeat = false;
                }

                // Returns true if this is a new sighting of the letter
                // Returns false if letter has been seen before
                public bool SeeThisLetter()
                {
                    if (HaveSeen == false)
                    {
                        FirstSeenOrder = nextSeenOrderValue++;

                        if (nextSeenOrderValue > ('z' - 'a' + 1))
                            throw new ArgumentOutOfRangeException(
                                "Somehow we've seen more letters then there exist letters.");

                        // New sighting of this letter
                        return true;
                    }
                    else
                    {
                        if (DoesRepeat == false)
                        {
                            DoesRepeat = true;
                        }

                        // Not first sighting of this letter
                        return false;
                    }
                }

                public int FirstSeenOrder { get; private set; }

                // Maybe don't really need have seen...only do this once
                public bool HaveSeen
                {
                    get
                    {
                        return FirstSeenOrder >= 0 ? true : false;
                    }
                }
                public bool DoesRepeat { get; private set; }
            }
        }


        public static void ValidateLetter(int letter)
        {
            if (letter < 'a' || letter > 'z')
                throw new ArgumentException("Must be a letter between 'a' and 'z'.");
        }

        const int NO_NRE = -1;

        // "Prints" first non-repeating element from the stream
        // to the result List<int> for each letter in the stream
        public static List<int> PrintFirstNRE(StreamReader sr)
        {
            if (sr == null)
                throw new ArgumentNullException("Parameter StreamReader sr is null.");

            //// Debug to see what we got in that thar stream
            //while (sr.EndOfStream == false)
            //{
            //    Console.WriteLine(Convert.ToChar(sr.Read()));
            //}

            var letters = new LetterTracker();
            var firstNREList = new List<int>();

            var currFirstNRE = sr.Read();
            ValidateLetter(currFirstNRE);
            letters.SeeLetter(currFirstNRE);
            firstNREList.Add(currFirstNRE);

            while (sr.EndOfStream == false)
            {
                var currLetter = sr.Read();
                ValidateLetter(currLetter);
                letters.SeeLetter(currLetter);

                if (currFirstNRE == NO_NRE || currLetter == currFirstNRE)
                {
                    currFirstNRE = letters.GetNRE();
                    firstNREList.Add(currFirstNRE);
                }
                else // Same first NRE as last loop
                {
                    firstNREList.Add(currFirstNRE);
                }
                // Could add a bailout logic feature for the LetterTracker class
                // to whittle the count of remaining NRE down to zero
                // so doen't have to look for an NRE if we know that there aren't any

                // if currLetter != firstNRE, we're done:
                // firstNRE can only change when currLetter == firstNRE
                // the sighting of currLetter has been logged
                // That is, currLetter's sighting order (if this is the first sighting)
                // also flagging currLetter as repeating if this is 2nd or later sighting
                // The LetterTracker keeps track of all of this
            }

            return firstNREList;
        }


        //public static char[] FirstNonRepeatingCharPerChar(StreamReader sr)
        //{
        //    char NO_NRE = Convert.ToChar(-1);

        //    if (sr == null)
        //        throw new ArgumentNullException("Parameter StreamReader sr is null.");

        //    var firstNRE = Convert.ToChar(sr.Read());

        //    // Older class type with no generic version (keys and values are object)
        //    // Key type is char
        //    // Value type is SeenChar, which contains bool DoesRepeat
        //    var seenChars = new OrderedDictionary();

        //    seenChars[firstNRE] = new SeenChar
        //    {
        //        DoesRepeat = false,
        //    };

        //    while (sr.EndOfStream == false)
        //    {
        //        var curr = Convert.ToChar(sr.Read());

        //        if (seenChars.Contains(curr))
        //        {   //Second sighting of this char in the stream

        //            var currChar = ((SeenChar)seenChars[curr]);
        //            if ( currChar.DoesRepeat == false )
        //            {   // And just became a repeat
        //                currChar.DoesRepeat = true;

        //                if (curr == firstNRE)
        //                {
        //                    // curr is now repeating, so must find a new NRE
        //                    foreach (var entry in seenChars)
        //                    {
        //                        // Does this give us the value only, or a kvp?
        //                        var seenChar = ((SeenChar)((DictionaryEntry)entry).Value);

        //                    }

        //                }
        //                else
        //                {


        //                }


        //                // Wait. This stream is guaranteed to only consist of lowercase letters
        //                // Don't use a freaking (non-generic) OrderedDictionary
        //                // Maybe this solution for if the things in the stream could be
        //                // or represent arbitrary objects
        //                // just do an array of (26) bools instead
        //            }
        //        }
        //    }



        //    throw new NotImplementedException();
        //}

        private class TestCase
        {
            public string InputAsString { get; set; }
            public StreamReader InputAsStream
            {
                get
                {
                    return new StreamReader(InputAsString.ToStream());
                }
            }

            public int[] CorrectNREAfterEachRead { get; set; }
        }
    }


    // Borrowed static string utility class
    // used for testing only
    // https://stackoverflow.com/a/38399723/13587176
    public static class StringExtensions
    {
        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s ?? ""));
        }


        //// Huh. I think that we don't actually have to reverse the string
        //// before converting it into a stream

        //// A basic solution that isn't robust in the general case:
        //// https://stackoverflow.com/questions/228038/best-way-to-reverse-a-string
        //public static string Reverse(this string s)
        //{
        //    var charArray = s.ToCharArray();
        //    Array.Reverse(charArray);
        //    return new string(charArray);
        //    // not: return charArray.ToString();
        //}
    }

}
