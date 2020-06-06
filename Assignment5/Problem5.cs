using DataStructuresAndAlgos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment5
{
    class Problem5
    {
        public static void RunInteractiveTesting()
        {
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


            var cache = new LRUCache<string, int>(3);

            string input = "default";

            while (input != "done")
            {
                Console.WriteLine(cache.Debug_List);
                Console.WriteLine(cache.Debug_Dict);

                Console.WriteLine("Enter cache command or \"done\"\n");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "set")
                {
                    var key = commands[1];
                    var value = int.Parse(commands[2]);
                    cache.Set(key, value);
                }
                else if (commands[0] == "get")
                {
                    var key = commands[1];
                    Console.WriteLine($"Got: {cache.Get(key)}\n");
                }
            }
        }



        // DID NULL CHECKS
        // BEFORE DEREF NEXT, PREV, HEAD, TAIL PTRS
        // WHAT IS THAT CUTE SYNTAX THAT KAL SHOWED US?
        // SEVERAL QUESTION MARKS OR SOMETHING
        // LIKE, IF THIS IS NOT NULL, DO THE THING
        // BUT IF IT IS NULL, JUST DO NOTHING (AND ALSO EVALUATE TO NULL)

        public class LRUCache<TKey, TValue>
        {
            public LRUCache(int size = 10)
            {
                if (size == 0)
                    throw new ArgumentOutOfRangeException("LRUCache must have a size of at least 1.");

                // GHOST STORIES FROM C++ PAST ABOUT CONSTRUCTORS THROWING EXCEPTIONS?    

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
                    // Easily missed because no constructor parens to prompt me
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
                    // A NULL CHECK IN THE DROPPING PROLLY KEPT CRASH FROM HAPPEN
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
            // "I know what I do"
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
