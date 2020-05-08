using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Assignment4
{
    public static class Utility
    {
        public static string CollectionToString<T>(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if (collection.Count() == 0)
            {
                return "{ }";
                // Avoids awkwardly removing an extra space later
            }

            var builder = new StringBuilder("{ ");

            foreach (var item in collection)
            {
                builder.Append($"{item}, ");
            }

            var length = builder.Length;
            builder.Remove(length - 2, 2).Append(" }");

            return builder.ToString();

        }
        private static void TestCollectionToString()
        {
            // Test CollectionToString for
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
            Console.WriteLine(CollectionToString(intArray));

            intArray = new int[] { 42 };
            Console.WriteLine(CollectionToString(intArray));

            intArray = new int[] { 7, 11 };
            Console.WriteLine(CollectionToString(intArray));

            intArray = null;
            //Console.WriteLine(CollectionToString(intArray));

        }
    }
}
