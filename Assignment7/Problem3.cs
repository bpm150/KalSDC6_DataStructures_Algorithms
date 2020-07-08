using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Assignment7
{
    class Problem3
    {


        public static IntNode ReverseEveryKNodes(IntNode head, int k)
        {
            if (head == null)
                throw new ArgumentNullException("head is null");

            if (k < 0)
                throw new ArgumentException("k must be non-neg");

            if (k == 0 || k == 1)
                return head;

            IntNode newHead = null;

            var nextLeftHead = ReverseSublist(head, k, out newHead);
            IntNode revSLHead = null;

            while (nextLeftHead != null)
            {
                // TODO:
                // Stitching of the groups together

                nextLeftHead = ReverseSublist(nextLeftHead, k, out revSLHead);
            }



            // TODO
            return default;
        }

        // Returns next nextLeftHead of next group or null if no next group exists
        private static IntNode ReverseSublist(IntNode inHead, int k, out IntNode outHead)
        {
            if (inHead.Next == null)
            {
                outHead = inHead;
                return null;
            }

            var prev = inHead;
            var curr = prev.Next;
            IntNode temp = null;

            for (var i = k - 1; i >= 1 && curr != null; --i)
            {
                temp = curr.Next;
                curr.Next = prev;
                prev = curr;
            }

            if (curr != null)
            {
                // Either just processed last group of n % k == 0,
                // or we're not at last group yet, so don't know yet about n % k

                outHead = curr;

                // Will be null iff this was the last group
                return curr.Next;
            }

            // if curr == null, then this was the last group
            // and n % k != 0
            outHead = prev;
            return null;
        }

        public static IntNode CreateIntNodeList(IEnumerable<int> ints)
        {
            if (ints.Count() == 0)
                return null;

            // DONE WITH DUMMY HEAD 
            var dummyHead = new IntNode();
            //{
            //    // Cannot apply indexing with [] to an expression of type
            //    // IEnumerable<int>
            //    // However, can use ElementAt
            //    //Data = ints[0],
            //    Data = ints.ElementAt(0),
            //};

            var curr = dummyHead;
            foreach (var i in ints)
            {
                curr.Next = new IntNode();
                curr = curr.Next;
                curr.Data = i;
            }

            return dummyHead.Next;

            // DONE WITH PREV   
            //var head = new IntNode();
            ////{
            ////    // Cannot apply indexing with [] to an expression of type
            ////    // IEnumerable<int>
            ////    // However, can use ElementAt
            ////    //Data = ints[0],
            ////    Data = ints.ElementAt(0),
            ////};

            //var curr = head;
            //IntNode prev = null;
            //foreach (var i in ints)
            //{
            //    curr.Data = i;
            //    curr.Next = new IntNode();
            //    prev = curr;
            //    curr = curr.Next;
            //}
            //prev.Next = null;
        }

        public class IntNode
        {
            public int Data;
            public IntNode Next;

            public override string ToString()
            {
                var sb = new StringBuilder();
                var curr = this;

                do
                {
                    sb.Append($"{curr.Data} -> ");
                    curr = curr.Next;
                } while (curr != null);

                sb.Append("null");

                return sb.ToString();
            }
        }

        //Hrm...what need happen to use Node<T> here?
        // Looks like it is probably as simple as declaring the method itself to be generic on T
        //private static Node<T> ReverseSublist<T>(Node<T> inHead, int k, out Node<T> outHead)
        //{
        //    outHead = default;

        //    return default;
        //}

        //private class Node<T>
        //{
        //    public T Data;
        //    public Node<T> Next;
        //}
    }
}
