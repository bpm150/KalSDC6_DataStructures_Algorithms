using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment5
{
    class Problem4
    {
        public static void RunInteractiveTesting()
        {
            string intro =
                "==============\n" +
                "= Problem #4 =\n" +
                "==============\n" +
                "\n" +
                "Implement a stack for which you can get the min element in O(1) time and O(1) space.\n";

            Console.WriteLine(intro);

            var stack = new MinEleIntStack();

            string input = "default";

            while (input != "done")
            {
                Console.WriteLine("\nEnter Stack command or \"done\"");
                input = Console.ReadLine();
                string[] commands = input.Split(' ');

                if (commands[0] == "push")
                {
                    var item = commands[1];
                    stack.Push(int.Parse(item));
                    Console.WriteLine($"\nPushed: {item}\n");
                    Console.WriteLine($"Also, the min is: {stack.GetMinEle()}");
                }
                else if (commands[0] == "pop")
                {
                    Console.WriteLine($"\nPopped: {stack.Pop()}\n");
                    Console.WriteLine($"Also, the min is: {stack.GetMinEle()}");
                }
            }
        }



        public class MinEleIntStack
        {
            private readonly Stack<int> stack;

            // minEle has no meaning if the stack is empty
            private int minEle;

            public MinEleIntStack()
            {
                stack = new Stack<int>();
            }

            public int Pop()
            {
                if (stack.Count == 0)
                    throw new InvalidOperationException("Stack is empty.");

                var topOfStackVal = stack.Pop();

                if (topOfStackVal >= minEle)
                    return topOfStackVal;
                else
                {
                    // The thing being popped actually represents the minEle on the stack

                    var minEleActuallyOnTopOfStack = minEle;

                    minEle = minEleActuallyOnTopOfStack + minEleActuallyOnTopOfStack - topOfStackVal;

                    return minEleActuallyOnTopOfStack;
                }
            }

            public void Push(int actualItem)
            {
                if (stack.Count == 0)
                {
                    stack.Push(actualItem);
                    minEle = actualItem;
                }
                else if (actualItem >= minEle)
                    stack.Push(actualItem);
                else
                {
                    // Don't push actualItem, since it is the new minEle.
                    // Insetad push "2x - minEle"
                    stack.Push(actualItem+actualItem-minEle);
                    minEle = actualItem;
                }
            }

            public int GetMinEle()
            {
                if (stack.Count == 0)
                    throw new InvalidOperationException("Stack is empty.");

                return minEle;
            }
        }
    }
}
