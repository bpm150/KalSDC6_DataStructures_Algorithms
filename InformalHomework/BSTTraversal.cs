using System;
using System.Collections.Generic;
using System.Text;

// Put this file in a dotnetcore 3.1 console project
// and run BSTDemo.Run()
namespace InformalHomework
{
    public static class BSTDemo
    {
        const string endl = "\n";
        public static void Run()
        {
            var bst = new BST<int>();

            var items = new int[] { 6, 4, 8, 2, 7, 10, 3, 9, 7, 3 };

            bst.Insert(items);

            Console.WriteLine(bst.StringifyInOrder_Recursive());
            Console.WriteLine(endl);

            Console.WriteLine(bst.StringifyInOrder_Iterative());
            Console.WriteLine(endl);

            // Lol, haven't got StringifyAsTree working yet
            //var treePrint = bst.StringifyAsTree();
            //Console.WriteLine(treePrint);
        }
    }

    // No rebalancing of this simple BST.
    // Value type for now (where T : struct)
    // Need to understand how using comparison operators works when T
    // could be a reference type (and thus could be null)
    // We can't explicitly check for null since T might be a value type
    // and you can't check a value type for null
    // Does that one null check syntax (?) somehow handle this?
    // (Figured it was only a syntactical convenience)
    public class BST<T> where T : struct, IEquatable<T>, IComparable
    {
        public BST()
        {
            root = null;
        }

 
        private enum TraversalMode
        {
            TryGoLeftToSmaller = 0,
            TryGoRight = 1,
            TryGoUpToGreater = 2,

        }
        // Reminder to self that enum values are seperated with a comma
        // not a semicolon :O

        public string StringifyInOrder_Iterative()
        {
            var sb = new StringBuilder();

            var curr = root;

            T? lastStringified = null;

            var mode = TraversalMode.TryGoLeftToSmaller;

            var stringDone = false;

            while (!stringDone)
            {
                switch (mode)
                {
                    case TraversalMode.TryGoLeftToSmaller:
                        if (curr.left != null)
                        {
                            curr = curr.left;
                            // Left mode repeatedly until no longer possible
                            mode = TraversalMode.TryGoLeftToSmaller;
                        }
                        else
                        {
                            StringifyNodeDataAndDupes(curr, sb);
                            lastStringified = curr.data;
                            // Try right if we must
                            mode = TraversalMode.TryGoRight;
                        }
                        break;

                    case TraversalMode.TryGoRight:
                        // (Notice we only try right mode for one loop cycle at a time)
                        if (curr.right != null)
                        {
                            curr = curr.right;
                            mode = TraversalMode.TryGoLeftToSmaller;
                        }
                        else
                        {
                            mode = TraversalMode.TryGoUpToGreater;
                        }
                        break;

                    case TraversalMode.TryGoUpToGreater:
                        if (curr.parent == null)
                        {
                            throw new NullReferenceException(
                                "Problem with logic or problem with tree. This should not be possible.");
                        }
                        else
                        {
                            curr = curr.parent;

                            if (!lastStringified.HasValue)
                                throw new NullReferenceException(
                                    "Find error in switch case logic. This should not be possible");

                            if (curr.data.CompareTo(lastStringified) > 0)
                            {
                                // Found something larger, which means something
                                // we haven't stringified yet
                                StringifyNodeDataAndDupes(curr, sb);
                                lastStringified = curr.data;

                                // Maybe there's something even larger down to the right
                                mode = TraversalMode.TryGoRight;
                            }
                            else
                            {
                                // Recall that in this BST, duplicates (equals)
                                // are handled within a single Node (as dupeCount)
                                // So, this else corresponds to curr.data strictly less than
                                // lastStringified

                                // We went up and couldn't find something larger
                                // If we are simultaneously also back at the root, we're done
                                if (curr == root)
                                {
                                    stringDone = true;
                                }

                                // If we went up and found something smaller, then
                                // we've already printed it. Keep going up
                                // until we find something larger or we find root
                                mode = TraversalMode.TryGoUpToGreater;
                            }
                        }
                        break;
                }
            }

            return sb.ToString();
        }


        public string StringifyInOrder_Recursive()
        {
            var sb = new StringBuilder();

            StringifyInOrder_Recursive_Helper(root, sb);

            return sb.ToString();
        }

        private static void StringifyInOrder_Recursive_Helper(Node<T> curr, StringBuilder sb)
        {
            if (curr == null)
                return;

            StringifyInOrder_Recursive_Helper(curr.left, sb);

            StringifyNodeDataAndDupes(curr, sb);

            StringifyInOrder_Recursive_Helper(curr.right, sb);
        }

        private static void StringifyNodeDataAndDupes(Node<T> curr, StringBuilder sb)
        {
            // Total number of impressions desired is one more than the dupeCount
            for (var i = 1; i <= curr.dupeCount + 1; ++i)
            {
                sb.Append($"{curr.data} ");
            }
        }

        public void Insert(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Insert(item);
            }
        }

        public void Insert(T item)
        {
            var newNode = new Node<T>(item);

            if (root == null)
            {
                root = newNode;
            }
            else
            {
                Insert_Helper(root, newNode);
            }
        }

        private static void Insert_Helper(Node<T> curr, Node<T> newNode)
        {
            if (newNode.data.CompareTo(curr.data) < 0)
            {
                if (curr.left == null)
                {
                    curr.left = newNode;
                    newNode.parent = curr;
                }
                else
                {
                    Insert_Helper(curr.left, newNode);
                }
            }
            else if (newNode.data.CompareTo(curr.data) > 0)
            {
                if (curr.right == null)
                {
                    curr.right = newNode;
                    newNode.parent = curr;
                }
                else
                {
                    Insert_Helper(curr.right, newNode);
                }
            }
            else if (newNode.data.Equals(curr.data))
            {
                ++curr.dupeCount;
            }
            else
            {
                throw new ArgumentException(
                    $"Somehow these objects are neither less than," +
                    $"greater than, nor equal to each other. Amazing!");
            }
        }





        private Node<T> root;

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        private class Node<T>
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            public Node(T item)
            {
                left = right = parent = null;
                data = item;
                dupeCount = 0;
            }

            public Node<T> left;

            public Node<T> right;

            public Node<T> parent;

            public T data;

            public int dupeCount;
        }



        public int GetHeight()
        {
            return GetHeight_Helper(root);
        }

        private static int GetHeight_Helper(Node<T> curr)
        {
            if (curr == null)
            {
                return 0;
            }
            else
            {
                var leftDepth = GetHeight_Helper(curr.left);
                var rightDepth = GetHeight_Helper(curr.right);

                // Remember to count curr
                return 1 + (leftDepth > rightDepth ? leftDepth : rightDepth);
            }
        }


        // Adapted from C++ code found here:
        // https://stackoverflow.com/a/30837061
        public string StringifyAsTree()
        {
            var sb = new StringBuilder();

            var height = GetHeight() * 2;
            for (var i = 0; i < height; ++i)
            {
                StringifyAsTree_StringifyRow(root, height, i, sb);
            }

            return sb.ToString();
        }


        // Adapted from C++ code found here:
        // https://stackoverflow.com/a/30837061
        private static void StringifyAsTree_StringifyRow(Node<T> p, int height, int depth, StringBuilder sb)
        {
            var items = new List<T?>();

            StringifyAsTree_GetLine(p, depth, items);

            var padWidth = (height - depth) * 2; // scale setw with depth

            var toggle = true; // start with left

            if (items.Count > 1)
            {
                foreach (var item in items)
                {
                    if (item.HasValue)
                    {
                        if (toggle)
                        {
                            sb.Append("/".PadRight(padWidth));
                            // But this one is just whitespace, what is it here for?
                            // Maybe for when padWidth is smaller than three spaces
                            sb.Append("   ".PadRight(padWidth));
                        }
                        else
                        {
                            sb.Append("\\".PadRight(padWidth));
                            sb.Append("   ".PadRight(padWidth));
                        }
                    }
                    toggle = !toggle;
                }
                sb.Append("\n");
                // why did they set it again here?
                // seems like they never un-set it
                //cout << setw((height - depth)*2);
            }
            foreach (var item in items)
            {
                if (item.HasValue)
                {
                    sb.Append(item.ToString().PadRight(padWidth));
                    sb.Append("   ".PadRight(padWidth));
                }
            }
            sb.Append("\n");
        }

        // Adapted from C++ code found here:
        // https://stackoverflow.com/a/30837061
        private static void StringifyAsTree_GetLine(Node<T> curr, int depth, List<T?> items)
        {
            if (depth <= 0 && curr != null)
            {
                items.Add(curr.data);
                return;
            }
            if (curr.left != null)
            {
                StringifyAsTree_GetLine(curr.left, depth - 1, items);
            }
            else if (depth - 1 <= 0)
            {
                items.Add(null);
            }
            if (curr.right != null)
            {
                StringifyAsTree_GetLine(curr.right, depth - 1, items);
            }
            else if (depth - 1 <= 0)
            {
                items.Add(null);
            }
        }
    }

}
