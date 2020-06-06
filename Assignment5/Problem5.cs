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


            var cache = new LRUCache<string, int>(2);

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
            private readonly int capacity;

            private int count;

            // Clever alias done by inheritacnce to save time writing
            private class Node : Node<TKey, TValue> { }
            // This works mostly like a typedef
            // Can only do using aliases at namespace level
            // Can do this anywhere you can declare a class

            // "I know what I do"
            private class Node<TKey, TValue>
            {
                public Node prev;

                public Node next;

                public TKey x;

                public TValue y;
            }



            private readonly Dictionary<TKey, Node> dict;

            // Most recent Node
            private Node head;

            // Least recent Node
            private Node tail;

                       


            public LRUCache(int size = 10)
            {
                if (size < 1)
                    throw new ArgumentOutOfRangeException("LRUCache must have a size of at least 1.");

                // GHOST STORIES FROM C++ PAST ABOUT CONSTRUCTORS THROWING EXCEPTIONS?    

                capacity = size;
                count = 0;

                dict = new Dictionary<TKey, Node>(capacity);

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
                        for (var i = 0; ; ++i)
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
                {
                    PromoteNodeToMRU(dict[x]);

                    // BUG: THE MOST SIGNIFICANT ONE
                    // Forgot that the values stored in the dictionary are
                    // Node<TKey, TValue> objects, whereas Get returns a TValue
                    return dict[x].y;
                }
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

                    PromoteNodeToMRU(lookupNode);

                    // Note also that count doesn't change
                    // Same number of elements before as after
                }
                else
                    SetWhenKeyNotFound(x, y);
            }

            private void SetWhenKeyNotFound(TKey x, TValue y)
            {
                // Make room for new node element in cache if cache is at capacity

                // Enforced min cache capacity of 1, so tail should never be null here
                if (count == capacity)
                {
                    // Drop the least recently used element from dict
                    dict.Remove(tail.x);

                    // Also drop the least recently used element from the list
                    if (tail.prev != null)
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



                // Create new node

                //BUG: NEED TO SPECIFY THE TYPE PARAMS FOR THE NEW NODE
                // Easily missed because no constructor parens to prompt me
                var newNode = new Node
                {
                    x = x, y = y,
                    // Will be first in the list by definition
                    next = head, // Taking the spot of the element currently at the head
                    prev = null,
                };

                // New node to list
                head = newNode;

                // BUG: PREV WAS NEVER GETTING SET FOR NODES THAT
                // WERE AT THE FRONT OF THE LIST WHEN A NEW (NON-OVERWRITE)
                // KVP WAS BEING INSERTED
                // CAUSED THE LRU ELEMENT TO NOT EFFECTIVELY GET DROPPED
                // FROM THE LIST AFTER IT WAS DROPPED FROM THE DICT
                // A NULL CHECK IN THE DROPPING PROLLY KEPT CRASH FROM HAPPEN
               
                
                // Fix the prev ptr on the next node (the old head)
                if (head.next != null)
                    head.next.prev = newNode;
                // old head is null when set is called when there are currently no elements

                // Fixup tail if we just added the first element
                if (tail == null)
                    tail = newNode;


                // New node to dictionary

                dict[x] = newNode;
            }

            private void PromoteNodeToMRU(Node node)
            {
                if (node == head)
                    return;

                // Step 1: Take node out of its current spot in the list
                if (node == tail)
                {
                    tail = node.prev;
                    node.prev.next = null;
                }
                else
                {
                    node.next.prev = node.prev;
                    node.prev.next = node.next;
                }

                // Step 2: Put node at the front of the list
                node.next = head;
                node.prev = null;
                node.next.prev = node;
                head = node;
            }
        }
    }
}
