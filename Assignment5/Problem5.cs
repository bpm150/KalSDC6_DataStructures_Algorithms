using DataStructuresAndAlgos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment5
{
    class Problem5
    {
        public static void RunTests()
        {
            var testCases = new List<TestCase>
            {
                new TestCase
                {
                    InputIntArray = new int[]{ 4, 5, 2, 25 },
                    CorrectNGEString = "{ {4, 5}, {5, 25}, {2, 25}, {25, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 13, 7, 6, 12 },
                    CorrectNGEString = "{ {13, -1}, {7, 12}, {6, 12}, {12, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 50, 2, 15, 0, 5, -3, -8, 7, 4, 1, 10, 6, 5, 12, 8, 2, 20 },
                    CorrectNGEString = "{ {50, -1}, {2, 15}, {15, 20}, {0, 5}, {5, 7}, {-3, 7}, {-8, 7}, {7, 10}, {4, 10}, {1, 10}, {10, 12}, {6, 12}, {5, 12}, {12, 20}, {8, 20}, {2, 20}, {20, -1} }",
                },
                new TestCase
                {
                    // Same principle as using String.Empty instead of ""
                    InputIntArray = Array.Empty<int>(),
                    //InputIntArray = new int[]{},
                    CorrectNGEString = "{}",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 4 },
                    CorrectNGEString = "{ {4, -1} }",
                },
                new TestCase
                {
                    InputIntArray = new int[]{ 12, 8, 2, 20, 10, 5, 3, -4 },
                    CorrectNGEString = "{ {12, 20}, {8, 20}, {2, 20}, {20, -1}, {10, -1}, {5, -1}, {3, -1}, {-4, -1} }",
                },
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 12, 9, 20, 10, 8, 0, 6, 9, 5, 10, 11},
                //    CorrectLeadersString = $"leaders are 20 and 11",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 0, -6, -10, -12, -20, -8, -5},
                //    CorrectLeadersString = $"leaders are 0 and -5",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ -20, -5, -20, -10, -5, -10},
                //    CorrectLeadersString = $"leaders are -5 and -10",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{0},
                //    CorrectLeadersString = $"leader is 0",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{0, 1, 2, 3, 4, 5, 6},
                //    CorrectLeadersString = $"leader is 6",
                //},
                //new TestCase
                //{
                //    InputIntArray = new int[]{ 4, 3, 1, 2},
                //    CorrectLeadersString = $"leaders are 4, 3 and 2",
                //},
            };


            string intro =
                "==============\n" +
                "= Problem #5 =\n" +
                "==============\n" +
                "\n" +
                "The task is to design and implement methods of an LRU cache. " +
                "The class has two methods get and set which are defined as follows. \n" +
                "get(x):\n" +
                "Gets the value of the key x if the key exists in the cache otherwise returns -1\n\n" +
                "set(x, y):\n" +
                "inserts the value if the key x is not already present.\n" +
                "If the cache reaches its capacity it should invalidate the least recently used item before inserting the new item." +
                "In the constructor of the class the size of the cache should be initialized.";

            Console.WriteLine(intro);

            int testOopsCount = 0;

            //for (var i = 0; i < testCases.Count; ++i)
            //{
            //    Console.WriteLine($"\nTest #{i + 1}:");


            //    Console.WriteLine($"For the array: {Utility.CollectionToString(testCases[i].InputIntArray)}");
            //    Console.WriteLine($"The correct result is: {testCases[i].CorrectNGEString}");

            //    var testCaseResult = PrintElementsWithNextGreaterElements(testCases[i].InputIntArray);

            //    string resultMessage;

            //    if (testCaseResult == testCases[i].CorrectNGEString)
            //    {
            //        resultMessage = "SUCCESS";
            //    }
            //    else
            //    {
            //        ++testOopsCount;
            //        resultMessage = "OOPS";
            //    }

            //    Console.WriteLine($"{resultMessage}!");
            //    Console.WriteLine($"Your answer is:        {testCaseResult}");
            //}

            //var testCount = testCases.Count;
            //var testSuccessCount = testCount - testOopsCount;

            //Console.WriteLine($"\n\nOut of {testCount} tests total,\n");
            //Console.WriteLine($"{testSuccessCount}/{testCount} tests succeeded, and");
            //Console.WriteLine($"{testOopsCount}/{testCount} tests oopsed.\n");

            //if (testOopsCount == 0)
            //{
            //    Console.WriteLine($"YAY! All tests succeeded! :D\n");
            //}
        }





        private class TestCase
        {
            public int[] InputIntArray { get; set; }

            public string CorrectNGEString { get; set; }
        }



        // TODO:
        // STILL NEED TO DO NULL CHECKS
        // BEFORE DEREF NEXT, PREV, HEAD, TAIL PTRS
        // WHAT IS THAT CUTE SYNTAX?
        // FIRST THINK ABOUT WHAT I WANT TO DO IF ANY OF THESE ARE NULL

        public class LRUCache<TKey, TValue>
        {
            public LRUCache(int size = 10)
            {
                if (size == 0)
                    throw new ArgumentOutOfRangeException("LRUCache must have a size of at least 1.");

                // GHOST STORIES ABOUT CONSTRUCTORS THROWING EXCEPTIONS?    

                capacity = size;
                count = 0;

                dict = new Dictionary<TKey, Node<TKey, TValue>>(capacity);

                // BUG: FORGOT TO UPDATE THESE NAMES
                head = null;
                tail = null;
            }

            public string Debug_Dict
            {
                get
                {
                    var sb = new StringBuilder();

                    foreach (var kvp in dict)
                    {
                        var x = kvp.Key;
                        var y = kvp.Value.y;

                        sb.Append($"dict[{x}]: y == {y}\n");
                    }

                    return sb.ToString();
                }
            }

            public string Debug_List
            {
                get
                {
                    var sb = new StringBuilder();

                    if (head != null)
                    {
                        var curr = head;
                        // TODO: CLEANER WAY TO DO THIS LOOP?
                        for(var i = 0; ; ++i)
                        {
                            sb.Append($"list[{i}]: x == {curr.x} y == {curr.y}\n");

                            if (curr.next != null)
                                curr = curr.next;
                            else
                                break;
                        }
                    }

                    return sb.ToString();
                }
            }

            public TValue Get(TKey x)
            {
                // Key must not be null
                if (x == null)
                    throw new ArgumentNullException();

                if (dict.ContainsKey(x))
                    // BUG: THE MOST SIGNIFICANT ONE
                    // Forgot that the values stored in the dictionary are
                    // Node<TKey, TValue> objects, whereas Get returns a TValue
                    return dict[x].y;
                else
                    throw new KeyNotFoundException();
            }

            public void Set(TKey x, TValue y)
            {
                // Key must not be null
                if (x == null)
                    throw new ArgumentNullException();

                // Null value is ok, though

                if (dict.ContainsKey(x))
                {
                    // Look up the Node for that key
                    var lookupNode = dict[x];

                    // Overwrite current value
                    lookupNode.y = y;

                    // Don't need to modify key of lookupNode, the key is not changing

                    // Bump up that Node as most recently used in two steps

                    if (lookupNode != head)
                    {
                        // Step 1: Take lookupNode out of its current spot in the list
                        if (lookupNode == tail)
                        {
                            tail = lookupNode.prev;
                            lookupNode.prev.next = null;
                        }
                        else
                        {
                            lookupNode.next.prev = lookupNode.prev;
                            lookupNode.prev.next = lookupNode.next;
                        }

                        // Step 2: Put lookupNode at the front of the list
                        lookupNode.next = head;
                        lookupNode.prev = null;
                        lookupNode.next.prev = lookupNode;
                        head = lookupNode;

                    }

                    // Note also that count doesn't change
                    // Same number of elements before as after
                }
                else // key not found in dict
                {
                    // Min capacity of 1, so tail should never be null here
                    if (count == capacity)
                    {
                        // Drop the last element
                        dict.Remove(tail.x);

                        if(tail.prev != null)
                            tail.prev.next = null;
                        tail = tail.prev;
                        // Boop! Now we've dropped both of the references
                        // that we had to the least recently used element

                        // Don't have to decrement count
                        // We added an element, then dropped one
                        // Count has not changed
                    }
                    else // count < capacity
                    {
                        ++count;
                    }

                    //BUG: NEED TO SPECIFY THE TYPE PARAMS FOR THE NEW NODE
                    // Easily missed by not having constructor parens
                    var newNode = new Node<TKey, TValue>
                    {
                        x = x,
                        y = y,
                        // Will be first in the list by definition
                        next = head,
                        prev = null,
                    };
                    head = newNode;

                    // BUG: PREV WAS NEVER GETTING SET FOR NODES THAT
                    // WERE AT THE FRONT OF THE LIST WHEN A NEW (NON-OVERWRITE)
                    // KVP WAS BEING INSERTED
                    // CAUSED THE LRU ELEMENT TO NOT EFFECTIVELY GET DROPPED
                    // FROM THE LIST AFTER IT WAS DROPPED FROM THE DICT
                    // A NULL CHECK IN THE DROPPING PROLLY KEPT IT FROM HAPPEN
                    if (newNode.next != null)
                        newNode.next.prev = newNode;

                    if (tail == null)
                        tail = newNode;

                    dict[x] = newNode;
                }
            }

            private int capacity;

            private int count;

            private readonly Dictionary<TKey, Node<TKey, TValue>> dict;

            // Most recent Node
            private Node<TKey, TValue> head;

            // Least recent Node
            private Node<TKey, TValue> tail;

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
            // I know what I do
            private class Node<TKey, TValue>
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
            {
                public Node<TKey, TValue> prev;

                public Node<TKey, TValue> next;

                public TKey x;

                public TValue y;
            }
        }

    }
}
