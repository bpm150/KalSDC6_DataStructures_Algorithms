using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment5
{
    class Problem2
    {

        public static void RunInteractiveTesting()
        {
            string intro =
                "==============\n" +
                "= Problem #2 =\n" +
                "==============\n" +
                "\n" +
                "Implement a Queue using two stacks, s1 and s2.\n";

            Console.WriteLine(intro);

            var queue = new MyQueue<string>();

            string input = "default";

            var queue2 = new Queue<string>();

            queue2.Enqueue("a string");

            queue2.Dequeue();



            while (input != "done")
            {
                Console.WriteLine(queue.Debug_View);

                Console.WriteLine("\nEnter Queue command or \"done\"\n");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "en")
                {
                    var item = commands[1];
                    queue.Enqueue(item);
                    Console.WriteLine($"Enqueued: {item}\n");
                }
                else if (commands[0] == "de")
                {
                    Console.WriteLine($"Dequeued: {queue.Dequeue()}\n");
                }
            }
        }


        // TODO: Debug visualization:
        // Pop all the items of each stack onto a temp stack
        // Reading them as you go to print the contents
        // Then re-load the stack to continue testing

        // Check the number of items in the queue
        // Dequeue them and re-queue them, printing as go
        // Stopping when gotten all the way around


        public class MyQueue<T> where T : IEquatable<T>
        {
            private Stack<T> s1_left_odd;
            private Stack<T> s2_right_even;

            private Queue<T> debug_queue;

            public MyQueue()
            {
                s1_left_odd = new Stack<T>();
                s2_right_even = new Stack<T>();

                debug_queue = new Queue<T>();
            }



            public string Debug_View
            {
                // This approach should work as long as it is executed in-between
                // Enqueue and Dequeue method calls
                // Using this during one of those calls may mess up the formatting
                // May need to return and make more robust if that is the goal
                get
                {
                    var tempStack = new Stack<T>();

                    // Using screen coordinates convention for
                    // the visualization: 0,0 is at upper left

                    // Stringify all the items from s1_left_odd
                    // put them in the array of SBs to render
                    var sbArr = new StringBuilder[s1_left_odd.Count+1];
                    // +1 since we want a row below to label the stacks and queue
                    var i = 0;
                    for (; i < s1_left_odd.Count; ++i)
                    {
                        sbArr[i] = new StringBuilder();

                        var item = s1_left_odd.Pop();

                        sbArr[i].Append($"{item}");
                        
                        tempStack.Push(item);
                    }
                    sbArr[i] = new StringBuilder();
                    sbArr[i].Append("s1_left_odd");

                    // Put them back
                    while (tempStack.Count > 0)
                        s1_left_odd.Push(tempStack.Pop());

                    PadWithWhitespace(sbArr);



                    // Now add on all the items from s2_right_even
                    // This time bottom to top
                    sbArr[^1].Append("s2_right_odd");
                    for (var k = s2_right_even.Count-1; k >= 0; --k)
                    // Running the loop when k == 0 crashes since that is
                    // popping from an empty stack
                    {
                        var item = s2_right_even.Pop();

                        sbArr[k].Append($"{item}");

                        tempStack.Push(item);
                    }

                    // Put them back
                    while (tempStack.Count > 0)
                        s2_right_even.Push(tempStack.Pop());

                    // Until I figure out wtf is up with the regular C# Queue
                    // Try it in the same context with a different type,
                    // maybe a value type
                    //PadWithWhitespace(sbArr);

                    //// Now the queue for comparison:
                    //for (var m = 0; m < debug_queue.Count; ++m)
                    //{
                    //    var item = debug_queue.Dequeue();

                    //    sbArr[^2].Append($"{item} ");
                    //}
                    //sbArr[^1].Append("debug_queue");


                    var sb = new StringBuilder();
                    // Must disambiguate the call to AppendJoin
                    sb.AppendJoin("\n", sbArr as IEnumerable<StringBuilder>);

                    return sb.ToString();
                }
            }

            private void PadWithWhitespace(StringBuilder[] sbArr)
            {
                const int EXTRA_PAD = 5;

                int lengthOfLongestSB = sbArr[0].Length;
                for(var i = 1; i < sbArr.Length; ++i)
                {
                    if (sbArr[i].Length > lengthOfLongestSB)
                        lengthOfLongestSB = sbArr[i].Length;
                }

                for(var k = 0; k < sbArr.Length; ++k)
                {
                    var padCount =
                        lengthOfLongestSB - sbArr[k].Length + EXTRA_PAD;
                    sbArr[k].Append(' ', padCount);
                }
            }


            // TODO: Have to handle pop from empty stack...how does that go with
            // the mod test for left or right?




            // For Dequeue:

            // If queue count (that is, stack1.count + stack1.count) % 2 == 1
            // (that is, number of items in queue is odd)
            // then the "front of the queue" is on the top of the left stack
            // -> Dequeue the front of the queue by popping the "left"/"odd" stack

            // Else, if the number of items in queue is even
            // then the "front of the queue" is on the top of the right stack
            // -> Dequeue the front of the queue by popping the "right"/"even" stack

            public T Dequeue()
            {
                // Let's see what exception we get when we pop from an empty stack
                //if(Count == 0)
                //    throw new Empty

                T item;

                if (Count % 2 == 1)
                    item = s1_left_odd.Pop();
                else
                    item = s2_right_even.Pop();

                //var dequeued = debug_queue.Dequeue();

                //if (item.Equals(dequeued))
                //    throw new Exception($"{item} was popped from the stacks, but {dequeued} was dequeued");

                return item;
            }


            // For Enqueue:

            // If there is an even number of items, time for there to be an odd
            // number of items, with one more item being on the left stack

            // Pop all items from the left stack onto the right stack (count how many)
            // Push the newly "Enqueued" item onto the (now empty) left stack
            // Then Push "count" number of items back onto the left stack

            // Now the stacks should be in a good state for being able to pop all
            // the way down to empty immediately, should the user dequeue all the items


            // If there is an odd number of items, time for there to be an even
            // number of items, with equal number of items being on each stack

            // Pop all items from the right stack onto the left stack (count how many)
            // Push the newly "Enqueued" item onto the (now empty) right stack
            // Then push "count" number of items back onto the right stack

            // Wait. We don't need to count how many.
            // Just put back until the the stacks have the same number

            public void Enqueue(T item)
            {
                if (Count % 2 == 1)
                {
                    while (s2_right_even.Count > 0)
                        s1_left_odd.Push(s2_right_even.Pop());

                    s2_right_even.Push(item);

                    while (s1_left_odd.Count != s2_right_even.Count)
                        s2_right_even.Push(s1_left_odd.Pop());
                }
                else
                {
                    while (s1_left_odd.Count > 0)
                        s2_right_even.Push( s1_left_odd.Pop() );

                    s1_left_odd.Push(item);

                    while (s1_left_odd.Count <= s2_right_even.Count)
                        s1_left_odd.Push(s2_right_even.Pop());
                }
                //T result;

                //var check = debug_queue.TryPeek(out result);

                //debug_queue.Enqueue(item);
            }


            public int Count
            {
                get
                {
                    return s1_left_odd.Count + s2_right_even.Count;
                }
            }
        

            // Enqueue must remove them in the order that they were added,
            // so odd/even will enable alternating between the stacks

            // Maybe can tell which stack has the "front" of the queue
            // based on whether the count of items in the queue is even or odd?

            



           // Always leave stacks ready to pop all the way down after pushing




            // Keep track of which stack currently has the "front" of the queue
            // "Stabilize" the stacks after every operation so that they either
            // contain the same number of items, or they differ by only one





        }
    
    }
}
