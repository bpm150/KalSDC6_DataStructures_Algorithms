using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assignment5
{
    class Problem3
    {
        public static void RunInteractiveTesting()
        {
            string intro =
                "==============\n" +
                "= Problem #3 =\n" +
                "==============\n" +
                "\n" +
                "Implement a Stack using two queues, q1 and q2.\n";

            Console.WriteLine(intro);

            var stack = new MyStack<string>();

            string input = "default";

            while (input != "done")
            {
                Console.WriteLine(stack.Debug_View);

                Console.WriteLine("\nEnter Stack command or \"done\"\n");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "push")
                {
                    var item = commands[1];
                    stack.Push(item);
                    Console.WriteLine($"Enqueued: {item}\n");
                }
                else if (commands[0] == "pop")
                {
                    Console.WriteLine($"Dequeued: {stack.Pop()}\n");
                }
            }
        }

    }

    public class MyStack<T> where T : IEquatable<T>
    {
        private readonly Queue<T> q1_above;
        private readonly Queue<T> q2_below;

        private readonly Stack<T> debug_stack;


        public MyStack()
        {
            q1_above = new Queue<T>();
            q2_below = new Queue<T>();

            debug_stack = new Stack<T>();
        }

        public T Pop()
        {
            if (q1_above.Count + q2_below.Count == 0)
                throw new InvalidOperationException("Stack is empty.");

            // TODO: ACTUALL IMPLEMENT POP
            var item = q1_above.Dequeue();



            var itemPoppedFromActualStack = debug_stack.Pop();

            if (item.Equals(itemPoppedFromActualStack) == false)
                throw new Exception("Item popped from actual stack was" +
                    $" {itemPoppedFromActualStack}, item you popped was {item}");

            return default;
        }

        public void Push(T item)
        {
            // TODO: ACTUALLY IMPLEMENT PUSH


            debug_stack.Push(item);
        }


        public string Debug_View
        {
            get
            {
                // Using screen coordinates convention for
                // the visualization: 0,0 is at upper left

                // Above the baseline
                const int VERT_QUEUE_LAYOUT_LINES = 4;

                var heightOfTallestSection =
                    debug_stack.Count > VERT_QUEUE_LAYOUT_LINES ?
                        debug_stack.Count : VERT_QUEUE_LAYOUT_LINES;

                // +1 since we want a row below to label the queues and stack
                var sbArr = new StringBuilder[heightOfTallestSection + 1];
                // For simplicity, put the SB objects in first
                for (var i = 0; i < sbArr.Length; ++i)
                    sbArr[i] = new StringBuilder();


                var sb_q1_above = new StringBuilder();

                for (var i = 1; i <= q1_above.Count; ++i)
                {
                    var item = q1_above.Dequeue();

                    sb_q1_above.Insert(0, $" {item}");

                    q1_above.Enqueue(item);
                }

                var sb_q2_below = new StringBuilder();

                for (var i = 1; i <= q2_below.Count; ++i)
                {
                    var item = q2_below.Dequeue();

                    sb_q2_below.Insert(0, $" {item}");

                    q2_below.Enqueue(item);
                }


                sbArr[^5].Append(sb_q1_above);
                sbArr[^4].Append(nameof(q1_above));
                //sbArr[^3] is a blank line
                sbArr[^2].Append(sb_q2_below);
                sbArr[^1].Append(nameof(q2_below));


                const int IN_BETWEEN_PADDING = 5;

                // Because the queue strings all have a leading space on them
                const int LEADING_PADDING = IN_BETWEEN_PADDING - 1;

                var lengthOfLongestSB = sbArr.Max(sb => sb.Length);

                foreach (var sb in sbArr)
                {
                    sb.Insert(0, " ", LEADING_PADDING + lengthOfLongestSB - sb.Length);
                    sb.Append(' ', IN_BETWEEN_PADDING);
                }
                




                var tempStack = new Stack<T>();

                // Get the items ready to read
                while (debug_stack.Count > 0)
                    tempStack.Push(debug_stack.Pop());

                for (var i = sbArr.Length - 2; tempStack.Count > 0; --i)
                {
                    var item = tempStack.Pop();

                    sbArr[i].Append($"{item}");

                    // Put the items back
                    debug_stack.Push(item);
                }
                sbArr[^1].Append(nameof(debug_stack));




                var finalSB = new StringBuilder();
                // Must disambiguate the call to AppendJoin
                finalSB.AppendJoin("\n", sbArr as IEnumerable<StringBuilder>);

                return finalSB.ToString();
            }
        }



    }


}
