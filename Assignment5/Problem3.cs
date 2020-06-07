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

                Console.WriteLine("\nEnter Stack command or \"done\"");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "push")
                {
                    var item = commands[1];
                    stack.Push(item);
                    Console.WriteLine($"\nPushed: {item}\n");
                }
                else if (commands[0] == "pop")
                {
                    Console.WriteLine($"\nPopped: {stack.Pop()}\n");
                }
            }
        }

    }

    public class MyStack<T> where T : IEquatable<T>
    {
        private readonly Queue<T> QA;
        private readonly Queue<T> QB;

        private readonly Stack<T> debug_stack;

        private bool WhenPopUseQA_WhenPushUseQB;

        public MyStack()
        {
            QA = new Queue<T>();
            QB = new Queue<T>();

            WhenPopUseQA_WhenPushUseQB = true;

            debug_stack = new Stack<T>();
        }

        public T Pop()
        {
            if (QA.Count + QB.Count == 0)
                throw new InvalidOperationException("Stack is empty.");

            T item;

            if (WhenPopUseQA_WhenPushUseQB)
            {
                item = QA.Dequeue();
                WhenPopUseQA_WhenPushUseQB = !WhenPopUseQA_WhenPushUseQB;
            }
            else
            {
                item = QB.Dequeue();
                WhenPopUseQA_WhenPushUseQB = !WhenPopUseQA_WhenPushUseQB;
            }


            //// Uneven. QA leads zig zag
            //if (QA.Count > QB.Count)
            //    item = QA.Dequeue();
            //else if (QA.Count < QB.Count)
            //    item = QB.Dequeue();
            //else if (whenEvenPushAndPopAbove)
            //{
            //    item = QA.Dequeue();
            //    whenEvenPushAndPopAbove = false;
            //}
            //else
            //{
            //    item = QB.Dequeue();
            //    whenEvenPushAndPopAbove = true;
            //}

            var itemPoppedFromActualStack = debug_stack.Pop();

            if (item.Equals(itemPoppedFromActualStack) == false)
                throw new Exception("Item popped from actual stack was" +
                    $" {itemPoppedFromActualStack}, item you popped was {item}");

            return item;
        }

        public void Push(T item)
        {
            if (WhenPopUseQA_WhenPushUseQB)
            {
                QB.Enqueue(item);
                WhenPopUseQA_WhenPushUseQB = !WhenPopUseQA_WhenPushUseQB;

                for (var i = QB.Count - 1; i > 0; --i)
                    QB.Enqueue(QB.Dequeue());
            }
            else
            {
                QA.Enqueue(item);
                WhenPopUseQA_WhenPushUseQB = !WhenPopUseQA_WhenPushUseQB;

                for (var i = QA.Count - 1; i > 0; --i)
                    QA.Enqueue(QA.Dequeue());
            }

            debug_stack.Push(item);
        }


        public string Debug_View
        {
            get
            {
                // Using screen coordinates convention for
                // the visualization: 0,0 is at upper left

                // Above the baseline
                const int VERT_QUEUE_LAYOUT_LINES = 5;

                var heightOfTallestSection =
                    debug_stack.Count > VERT_QUEUE_LAYOUT_LINES ?
                        debug_stack.Count : VERT_QUEUE_LAYOUT_LINES;

                // +1 since we want a row below to label the queues and stack
                var sbArr = new StringBuilder[heightOfTallestSection + 1];
                // For simplicity, put the SB objects in first
                for (var i = 0; i < sbArr.Length; ++i)
                    sbArr[i] = new StringBuilder();


                var sb_q1_above = new StringBuilder();

                for (var i = 1; i <= QA.Count; ++i)
                {
                    var item = QA.Dequeue();

                    sb_q1_above.Insert(0, $" {item}");

                    QA.Enqueue(item);
                }

                var sb_q2_below = new StringBuilder();

                for (var i = 1; i <= QB.Count; ++i)
                {
                    var item = QB.Dequeue();

                    sb_q2_below.Insert(0, $" {item}");

                    QB.Enqueue(item);
                }


                sbArr[^5].Append(sb_q1_above);
                sbArr[^4].Append(nameof(QA));
                //sbArr[^3] is a blank line
                sbArr[^2].Append(sb_q2_below);
                sbArr[^1].Append(nameof(QB));


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
