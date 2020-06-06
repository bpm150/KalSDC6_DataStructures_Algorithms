using System;
using System.Collections.Generic;
using System.Linq;
using InformalHomework;

namespace DataStructuresAndAlgos
{
    class Program
    {
        static void Main(string[] args)
        {

            //Problem1.RunTests();

            //Problem2.RunTests();

            //Problem3.RunTests();

            //Problem4.RunTests();

            //Problem5.RunTests();

            //Problem6.RunTests();

            //Problem7.RunTests();
            //Problem7.PrintLeaders(new int[] { 4, 3, 1, 2 });

            //Problem8.RunTests();

            //Assignment4.Problem9.RunTests();

            //Problem10.RunTests();
            //Problem10.PrintByFreq(new int[] { 9, 4, 5, 7, 5, 5, 9 });

            //Assignment5.Problem1.RunTests();


            //BSTDemo.Run();



            //ClockAngleDemo.Run();


            //Assignment5.Problem6.RunTests();



            //Assignment5.Problem5.RunInteractiveTesting();


            Assignment5.Problem2.RunInteractiveTesting();


            // looks like there's no copy constructor for Array
            // have to allocate first then copy

            //var arr = new int[] { };

            //IEnumerable<int> ie = arr;




            // int.MaxValue == 2 billion and change
            // long.MaxValue == 9 quintillion, etc.

            // About four billion int.MaxValue would fit in a long.MaxValue
            // 9223372036854775807 / 2147483647 == 4 billion and change

            // If you are overflowing the long type, you are doing something wrong??






            //var dict = new Dictionary<int, int>() { { 2, 2}, { 5, 2}, { 8, 3}, { 6, 1} };

            //IEnumerable<KeyValuePair<int, int>> dictIE = dict;

            //foreach (var kvp in dictIE)
            //{
            //    Console.WriteLine($"{kvp.Value}");
            //}

            //Console.WriteLine("\n\n");

            //var descendingByValue = dict.OrderByDescending(i => i.Value);

            //foreach (var kvp in descendingByValue)
            //{
            //    Console.WriteLine($"{kvp.Value}");
            //}


        }
    }
}
