using System;
using System.Text;

namespace Assignment4
{
    public static class Utility
    {
        public static string ArrayToString<T>(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (array.Length == 0)
            {
                return "{ }";
                // Avoids awkwardly removing an extra space later
            }

            var builder = new StringBuilder("{ ");

            foreach (var item in array)
            {
                builder.Append($"{item}, ");
            }

            var length = builder.Length;
            builder.Remove(length - 2, 2).Append(" }");

            return builder.ToString();

        }
        private static void Test_ArrayToString()
        {
            // Test ArrayToString for
            // null
            // length == 0
            // length == 1
            // length == 2(+)



            // (Permanently) empty array
            // Arrays cannot change size
            // Empty array in C# is as close as you can get to an immutable collection
            // in C#
            // Can use the same instance for all purposes, as you can with String.Empty

            var intArray = new int[] { };
            Console.WriteLine(ArrayToString(intArray));

            intArray = new int[] { 42 };
            Console.WriteLine(ArrayToString(intArray));

            intArray = new int[] { 7, 11 };
            Console.WriteLine(ArrayToString(intArray));

            intArray = null;
            //Console.WriteLine(ArrayToString(intArray));

        }
    }
}
