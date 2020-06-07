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
            private readonly Stack<T> s1_left;
            private readonly Stack<T> s2_right;

            private readonly Queue<T> debug_queue;


            private bool whenEvenPushAndPopLeft;

            //private bool popLeftNext;

            // TODO: FIGURE OUT WHAT'S GOING ON WITH THIS ENUM NOT WORKING

            //TODO: UPDATE THESE WITH FINAL STACK NAMES
            //public enum StackSide
            //{
            //    never_popped = 0,
            //    s1_left = 1,
            //    s2_right = 2,
            //}
            //// For specifying the front of the queue ONLY when Count >= 3
            //public StackSide lastPoppedSide = StackSide.never_popped;

            public MyQueue()
            {
                s1_left = new Stack<T>();
                s2_right = new Stack<T>();

                //lastPoppedSide = StackSide.never_popped;

                //popLeftNext = true;


                whenEvenPushAndPopLeft = true; ;

                debug_queue = new Queue<T>();
            }



            public string Debug_View
            {
                get
                {
                    var tempStack = new Stack<T>();

                    // Using screen coordinates convention for
                    // the visualization: 0,0 is at upper left


                    var tallestStackHeight =
                        s1_left.Count > s2_right.Count ? s1_left.Count : s2_right.Count;

                    // +1 since we want a row below to label the stacks and queue
                    var sbArr = new StringBuilder[tallestStackHeight + 1];
                    // For simplicity, put the SB objects in first
                    for (var i = 0; i < sbArr.Length; ++i)
                        sbArr[i] = new StringBuilder();



                    // Stringify all the items from s1_left_odd
                    // put them in the array of SBs to render
                    // BUG: BE CAREFUL OF COMPARING TWO THINGS WHEN THEY ARE BOTH CHANGING
                    // ARE YOU SURE THAT'S WHAT YOU MEAN?

                    // Get the items ready to read
                    while (s1_left.Count > 0)
                        tempStack.Push(s1_left.Pop());

                    for (var i = sbArr.Length - 2; tempStack.Count > 0; --i)
                    {
                        var item = tempStack.Pop();

                        sbArr[i].Append($"{item}");

                        // Put the items back
                        s1_left.Push(item);
                    }
                    sbArr[^1].Append("s1_left_odd");


                    PadWithWhitespace(sbArr);

                    // Build bottom to top
                    // Adding to the sb during step of pushing the items
                    // back on their original stack

                    // Now add on all the items from s2_right_even
                    // This time bottom to top

                    while(s2_right.Count > 0)
                        tempStack.Push(s2_right.Pop());

                    sbArr[^1].Append("s2_right_odd");
                    for (var k = sbArr.Length - 2; tempStack.Count > 0; --k)
                    {
                        var item = tempStack.Pop();

                        sbArr[k].Append($"{item}");

                        // Put them back
                        s2_right.Push(item); 
                    }



                    PadWithWhitespace(sbArr);

                    // Don't need a temp queue, since we can simply
                    // make the items "go around and get back in"

                    // Now the queue for comparison:
                    for (var m = 1; m <= debug_queue.Count; ++m)
                    {
                        var item = debug_queue.Dequeue();

                        sbArr[^2].Append($"{item} ");

                        debug_queue.Enqueue(item);
                    }
                    sbArr[^1].Append("debug_queue");


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
                if (Count == 0)
                    throw new InvalidOperationException("Queue is empty.");


                T item;

                if (s1_left.Count > s2_right.Count)
                {
                    item = s1_left.Pop();
                    whenEvenPushAndPopLeft = false;
                }
                else if(s1_left.Count < s2_right.Count)
                {
                    item = s2_right.Pop();
                    whenEvenPushAndPopLeft = true;
                }
                else if (whenEvenPushAndPopLeft)
                    item = s1_left.Pop();
                else
                    item = s2_right.Pop();




                //if (popLeftNext == true)
                //{
                //    item = s1_left.Pop();
                //    popLeftNext = false;
                //}
                //else
                //{
                //    item = s2_right.Pop();
                //    popLeftNext = true;
                //}


                // Alternating works fine for Count >= 4
                // For Count <= 3, we need special handling:
                //if (Count <= 3)
                //    popLeftNext = true;


                //// After popping

                //// If right stack is taller
                //// Fixup: Move the top item of the right stack to the left stack
                //// Thus making the stacks equal height
                //if (s1_left.Count < s2_right.Count)
                //    s1_left.Push(s2_right.Pop());
                //// Now the next item to Dequeue is on the top of the left stack
                //// and the item-after-that to Dequeue is on the top of the right
                //// stack


                //// My guess is that if the stacks are the same height, that sometimes
                //// we should swap the top ones and sometimes we should not (based on
                //// something! maybe we do need an additional variable) Let's experiment...
                //else if (s1_left.Count == s2_right.Count && s1_left.Count > 0 && whenEvenPushAndPopLeft)
                //{
                //    // Let's try swapping them all of the time and see what happen
                //    var temp = s1_left.Pop();
                //    s1_left.Push(s2_right.Pop());
                //    s2_right.Push(temp);

                //    whenEvenPushAndPopLeft = false;
                //}

                //// Maybe it is a simple alternation



                // Basically, at any given moment, we want to be able to Enqueue
                // as though nothing has ever been Dequeued, since Enqueue is so
                // much more complicated anyway, wanting to shift some of the overall
                // complexity to here in Dequque


                var itemFromRealQueue = debug_queue.Dequeue();

                if (item.Equals(itemFromRealQueue) == false)
                    throw new Exception("Item Dequeued from MyQueue != item Dequeued from real Queue.");
                    


                //if (lastPoppedSide == StackSide.s2_right || lastPoppedSide == StackSide.never_popped)
                //{
                //    item = s1_left.Pop();
                //    lastPoppedSide = StackSide.s1_left;
                //}
                //else if (lastPoppedSide == StackSide.s1_left)
                //{
                //    item = s2_right.Pop();
                //    lastPoppedSide = StackSide.s2_right;
                //}
                //else
                //    throw new Exception("Stack side alternation in a bad state.");


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

                // When odd, push onto the shorter stack (evening it up)
                // Bookkeep where to push/pop next (since now even)

                // When even, check bookkeeping to see where to push
                // Note that no bookkeeping is necessary, since now the stacks
                // are odd (and you always pop from the taller stack and
                // push onto the shorter stack to maintain the zig-zag ordering)


                    if (s1_left.Count > s2_right.Count)
                    {
                        // Push onto the right stack
                        while (s2_right.Count > 0)
                            s1_left.Push(s2_right.Pop());

                        s2_right.Push(item);

                        while (s1_left.Count != s2_right.Count)
                            s2_right.Push(s1_left.Pop());

                        // Remember that the left stack still has the top
                        whenEvenPushAndPopLeft = true;
                    }
                    else if (s1_left.Count < s2_right.Count)
                    {
                        // Push onto the left stack
                        while (s1_left.Count > 0)
                            s2_right.Push(s1_left.Pop());

                        s1_left.Push(item);

                        while (s1_left.Count != s2_right.Count)
                            s1_left.Push(s2_right.Pop());

                        // Remember that the right stack now has the top
                        whenEvenPushAndPopLeft = false;
                    }
                    else if (whenEvenPushAndPopLeft)
                    {
                        // REFACTOR OUT THIS COPYPASTA FROM ABOVE

                        // Push onto the left stack
                        while (s1_left.Count > 0)
                            s2_right.Push(s1_left.Pop());

                        s1_left.Push(item);

                        while (s1_left.Count <= s2_right.Count)
                            s1_left.Push(s2_right.Pop());

                        // No bookkeeping necessary...now the left stack
                        // is taller and obviously the top
                    }
                    else
                    {
                        // REFACTOR OUT THIS COPYPASTA FROM ABOVE

                        // Push onto the right stack
                        while (s2_right.Count > 0)
                            s1_left.Push(s2_right.Pop());

                        s2_right.Push(item);

                        while (s1_left.Count >= s2_right.Count)
                            s2_right.Push(s1_left.Pop());

                        // No bookkeeping necessary...now the right stack
                        // is taller and obviously the top
                    }



                debug_queue.Enqueue(item);
            }


            public int Count
            {
                get
                {
                    return s1_left.Count + s2_right.Count;
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
