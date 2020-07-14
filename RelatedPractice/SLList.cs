// Brillan Morgan 6/21/20
// Reversing a linked list in three flavors (using a stack, using recursion and using a pair of temp pointers)
// Look for these:
// Reverse_StackVersion
// Reverse_RecursionVersion
// Reverse_TwoPointerTechnique

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

// HOW TO USE:
// Call RelatedPractice.LinkedListDemo.RunDemo(); from your Program.Main method

namespace RelatedPractice
{
    // For a minute, I confuse and thought I had to create an entire new class
    // to make the GetLetters method
    // GetLetters and its enclosing class don't need to be generic
    // (on char or any other type)
    // It can still return a generic enumerator IEnumerator<char>
    public class LetterMaker : IEnumerable<char>
    {
        public static IEnumerator<char> GetLetters(int count)
        {
            for (var i = 'A'; i < count; ++i)
                yield return i;
        }

        // Implicit implementation
        public IEnumerator<char> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // Implicit implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        // 
        private LetterMaker() { }
    }

    public static class LinkedListDemo
    {

        public static IEnumerator<char> Letters(int count)
        {
            // BUG:
            // for for (var i = 'A'; i < count; ++i)
            //yield return i;
            for (var i = 0; i < count; ++i)
            {
                yield return (char)('A' + i);
                // Have to add these as integers, then cast down to char
            }

            // Ah! yield without return possible for when the method does
            // something when the enumerator is advanced, but there
            // the user of the enumerator is not expecting a value
            // from advancement?

            // Evidently there is no char + char operator
            // Must cast. Why.
            //for (char i = (char)0; i < count; ++i)
            //{
            //    char v = i + 'A';
            //    yield return v;
            //}
        }

        public static void RunDemo()
        {
            var letters = Letters(4);

            // foreach statement cannot operate on variables of type IEnumerator<char>
            // because IEnumerator<char> does not contain a public instance
            // definition for GetEnumerator
            //foreach (var letter in letters)
            //{
            //}
            // QUESTION:
            // How much trouble is to use this yield approach for generating a
            // sequence of values without storing them in a collection?
            // Thought about how that could have been used to serve up the
            // 'A' - 'Z', 'a' - 'z' and '0' - '9' that I used in one of the Assn 6
            // problems. Mostly cute and not useful since it doesn't reduce the
            // time complexity of the solution?

            Console.WriteLine("Build LinkedList<char> letterList:\n");
            var letterList = new SLList<char>();
            Console.WriteLine(letterList);

            // Evidently enumerators start pointing to the "element before the
            // first one in the collection"
            // This seemed confusing at first, but is useful in this way:
            while (letters.MoveNext())
            {
                letterList.Add(letters.Current);
                Console.WriteLine(letterList);
            }


            // nameof() does not involve reflection, correct?

            Console.WriteLine($"\n\n{nameof(letterList)}.{nameof(letterList.Reverse_StackVersion)}():\n");
            letterList.Reverse_StackVersion();
            Console.WriteLine(letterList);
            Console.WriteLine($"{nameof(letterList)}.{nameof(letterList.Reverse_StackVersion)}():\n");
            letterList.Reverse_StackVersion();
            Console.WriteLine(letterList);


            Console.WriteLine($"\n\n{nameof(letterList)}.{nameof(letterList.Reverse_RecursionVersion)}():\n");
            letterList.Reverse_RecursionVersion();
            Console.WriteLine(letterList);
            Console.WriteLine($"{nameof(letterList)}.{nameof(letterList.Reverse_RecursionVersion)}():\n");
            letterList.Reverse_RecursionVersion();
            Console.WriteLine(letterList);



            Console.WriteLine($"\n\n{nameof(letterList)}.{nameof(letterList.Reverse_TwoPointerTechnique)}():\n");
            letterList.Reverse_TwoPointerTechnique();
            Console.WriteLine(letterList);
            Console.WriteLine($"{nameof(letterList)}.{nameof(letterList.Reverse_TwoPointerTechnique)}():\n");
            letterList.Reverse_RecursionVersion();
            Console.WriteLine(letterList);

            // TODO:
            // Do a test with zero letters to see what happen
            // Seems like the behavior of MoveNext (see above)
            // takes care of that
        }
    }

    public class SLList<T> : IEnumerable<T>
    {
        private class Node
        {
            public T Data { get; set; }
            public Node Next { get; set; }
        }

        private Node head;
        private Node tail;

        // For a generic class, ctors don't have the type param <T>
        // between their name and their param list
        public SLList()
        {
            tail = head = null;
        }

        // Added for Assignment7.Problem1
        // Inspired by the similar constructor for the List type
        public SLList(IEnumerable<T> collection)
        {
            tail = head = null;
            foreach (var item in collection)
                Add(item);
        }


        // Added for Assignment7.Problem1
        public T GetMiddle()
        {
            if (head == null )
                throw new ArgumentNullException(
                    "no objects in list");

            var currEnd = head;
            var currMid = head;

            var advMid = true;

            while (currEnd.Next != null)
            {
                currEnd = currEnd.Next;
                if (advMid)
                {
                    currMid = currMid.Next;
                    advMid = false;
                }
                // BUG: advMid starts as true, gets set to false on its first advance,
                // then stayed false forever after that since I never set it back to
                // true. This is why it worked for short lists, but not for longer ones
                // lists of size >= 4, the smallest size for which advMid not being set
                // back to true would cause currMid to not advance properly
                else
                    advMid = true;
            }

            return currMid.Data;
        }

        public void Add(T item)
        {
            var newNode = new Node
            {
                Data = item,
                Next = null,
            };

            if (head == null)
                head = tail = newNode;
            else
            {
                // When adding to the end of the list,
                // remember to update the tail pointer
                tail.Next = newNode;
                tail = newNode;
            }

        }

        public void Reverse_StackVersion()
        {
            var stk = new Stack<Node>();

            for (var node = head; node != tail; node = node.Next)
                stk.Push(node);

            var curr = head = tail;

            while (stk.Count > 0)
            {
                curr.Next = stk.Pop();
                curr = curr.Next;
            }

            // Note typo on whiteboard:
            // was: tail.curr;
            tail = curr;
            tail.Next = null;
        }

        public void Reverse_RecursionVersion()
        {
            tail = head;
            ReverseHelper(head, null);
        }

        private void ReverseHelper(Node curr, Node prev)
        {
            if (curr.Next == null)
                head = curr;
            else
                ReverseHelper(curr.Next, curr);

            curr.Next = prev;
        }

        public void Reverse_TwoPointerTechnique()
        {
            var rebuild = head;
            var breakdown = head.Next;
            tail = rebuild;
            tail.Next = null;
            while (breakdown != null)
            {
                var curr = breakdown;
                breakdown = breakdown.Next;
                curr.Next = rebuild;
                rebuild = curr;
            }
            head = rebuild;
        }

        public override string ToString()
        {
            // using System.Text; for StringBuilder
            var sb = new StringBuilder();

            sb.Append("LinkedList:\n");
            var i = 0;
            foreach (var item in this)
            {
                sb.Append($"item {i} == {item}\n");
                ++i;
            }

            return sb.ToString();
        }

        public void Remove(T item)
        {
            throw new NotImplementedException();
        }


        // One poster:
        // "Explicit for IEnumerable because weakly typed collections are Bad"
        // https://stackoverflow.com/a/11296888/13587176
        // "uses the strongly typed IEnumerable<T> implementation"

        // Reply to that poster:
        // "But implementing IEnumerable is redundant (IEnumerable<T> already inherits from it). "
        // https://stackoverflow.com/questions/11296810/how-do-i-implement-ienumerablet#comment43530584_11296888

        // For some reason, can't use lambda notation for this method?
        // Maybe because it references instance class data/members?
        IEnumerator IEnumerable.GetEnumerator()
        {
            // First of all, does any of this work?

            // Does this:
            yield return this;

            // Do the same thing as this:
            //foreach(var node in this)
            //{
            //    yield return node;
            //}

            // Don't understand why I can't seem to invoke
            // IEnumerable<T>.GetEnumerator explicitly
            // How to invoke it explicitly?
            //return this.IEnumerable<T>.GetEnumerator();
        }

        // Not really sure why this can't just be:
        // IEnumerator<T> GetEnumerator()
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var curr = head;
            while (curr != null)
            {
                yield return curr.Data;
                curr = curr.Next;
            }

            // Remember "yield" does not involve recursion
            // execution of this method picks right back up where it left off
            // so it should be in a loop
        }


        //IEnumerator<T> IEnumerable<T>.GetEnumerator()
        //{
        //    throw new System.NotImplementedException();
        //}


        //// Implicitly public because it implements an interface
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    throw new System.NotImplementedException();
        //}


        // Cannot be an iterator block because 'void' is not an iterator
        // interface type:
        //public void Fn()
        //{
        //    yield return 
        //}
    }
}
