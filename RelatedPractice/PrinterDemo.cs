using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RelatedPractice
{
    public class PrinterDemo
    {
        
        // These methods are kind of like classes
        // Pros and cons of using this or a class?
        // Could be using both at the same time?
        // When are they the most useful?
        // When do they genuinely improve a solution?
        // Idea to ask Charlie

        // using System.Collection for IEnumerator;
        public static IEnumerator Printer(int count = 10)
        {
            if (count < 1)
                throw new ArgumentException("invalid number of numbers to print");

            // Nope. Remember yield does not mean recursion:
            //var curr = 0;
            //if(curr == 0)
            Console.WriteLine(
                "Hi! I'm Printer.\n" +
                "My job is to print a sequence of numbers.\n" +
               $"I will dutifully print {count} numbers for you.");
            yield return default;

            for (var i = 1; i <= count; ++i)
            {
                Console.WriteLine($"{i}");
                yield return default;
            }

            // Looks like if Printer wants to do something "after" the last
            // iteration, it needs to do it before yielding the final time
            // Looks like the body of Printer does not run at all when
            // MoveNext returns false
            // At some point i > count, do we get to see that part?
        }

        public static void InteractivePrinterDemo()
        {
            var printer = Printer();

            string input = String.Empty;
            while (input != "done")
            {
                Console.WriteLine("\nEnter a printer command, or \"done\"\n");
                input = Console.ReadLine();

                string[] commands = input.Split(' ');

                if (commands[0] == "reset")
                {
                    // System.NotSupportedException:
                    // 'Specified method is not supported'
                    //printer.Reset();
                    printer = Printer();
                    Console.WriteLine($"Reset the printer.\n");
                }
                else if (commands[0] == "set")
                {
                    var numToPrint = commands[1];
                    printer = Printer(int.Parse(numToPrint));
                    Console.WriteLine($"Set the printer to: {numToPrint}\n");
                }
                else if (commands[0] == "next")
                {
                    var result = printer.MoveNext();
                    Console.WriteLine($"The result of the advance was: {result}\n");
                    // Evidenly once MoveNext returns false, it does not resume
                    // the Printer method again
                    // after all, execution reached the very end of that method
                    // At that point, would need to reset printer to new Printer()
                }
            }
        }

        public static void BasicPrinterDemo()
        {
            var printer = Printer();

            Console.WriteLine("Intro happens first:\n");
            printer.MoveNext();

            Console.WriteLine("Let's advance printer five times:\n");
            for (var i = 0; i < 5; ++i)
                printer.MoveNext();

            Console.WriteLine("Printer is le tired and will take a nap.\n");
            System.Threading.Thread.Sleep(1000);
            // https://stackoverflow.com/questions/10458118/wait-one-second-in-running-program
            // Though:
            // "Thread.Sleep locks the UI etc. Timer implementations since it waits then fires:"
            // https://stackoverflow.com/a/20799039/13587176

            Console.WriteLine("Now, let's advance printer five more times:\n");
            for (var i = 0; i < 5; ++i)
                printer.MoveNext();




            //while (printer.MoveNext()) { }

            // MoveNext would need to run the advancement even if the
            // Current value is never accessed with Current, right?
        }
    }
}
