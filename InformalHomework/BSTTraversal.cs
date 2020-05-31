using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment4.InformalHomework
{
    // No rebalancing of this simple BST.
    // Value type for now
    // Need to understand how using comparison operators works when T
    // could be a reference type (and thus could be null)
    // We can't explicitly check for null since T might be a value type
    // and you can't check a value type for null
    // Does that one null check syntax (?) somehow handle this?
    // (Figured it was a syntactical convenience, rather than an actual substantive
    // feature addition)
    public class BST<T> where T : struct, IEquatable<T>, IComparable
    {
        public BST()
        {
            root = null;
        }

        public void Insert(T item)
        {
            var newNode = new Node<T>();
            newNode.data = item;

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
                    return;
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
                    return;
                }
                else
                {
                    Insert_Helper(curr.right, newNode);
                }
            }
            else if (newNode.data.Equals(curr.data))
            {
                ++curr.dupeCount;
                return;
            }
            else
            {
                throw new ArgumentException(
                    $"Those are some objects you've got there!\n" +
                    "Somehow they are neither less than, greater than," +
                    "or equal to each other.");
            }

            throw new ArgumentException(
                $"If you made it all the way down here, something bad happened.");
        }

        public int GetHeight()
        {
            return GetHeight_Helper(root);
        }

        private static int GetHeight_Helper(Node<T> root)
        {
            if (root == null)
            {
                return 0;
            }
            else
            {
                var leftDepth = GetHeight_Helper(root.left);
                var rightDepth = GetHeight_Helper(root.right);

                // Count the current node
                return 1 + (leftDepth > rightDepth ? leftDepth : rightDepth);
            }
        }



        // Adapted from:
        // https://stackoverflow.com/a/30837061
        public string PrintAsTree()
        {
            var sb = new StringBuilder();

            var height = GetHeight() * 2;
            for (var i = 0; i < height; ++i)
            {
                PrintAsTree_PrintRow(root, height, i, sb);
            }

            return sb.ToString();
        }


        // Adapted from:
        // https://stackoverflow.com/a/30837061
        private static void PrintAsTree_PrintRow(Node<T> p, int height, int depth, StringBuilder sb)
        {
            var vec = new List<T?>();
            
            PrintAsTree_GetLine(p, depth, vec);
            
            var padWidth = (height - depth)*2; // scale setw with depth
            
            var toggle = true; // start with left
            
            if (vec.Count > 1)
            {
                foreach(var v in vec)
                {
                    if (v.HasValue)
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
            foreach (var v in vec)
            {
                if (v.HasValue)
                {
                    sb.Append(v.ToString().PadRight(padWidth));
                    sb.Append("   ".PadRight(padWidth));
                }
            }
            sb.Append("\n");
        }

        // Adapted from:
        // https://stackoverflow.com/a/30837061
        private static void PrintAsTree_GetLine(Node<T> root, int depth, List<T?> vals)
        {
            if (depth <= 0 && root != null) {
                vals.Add(root.data);
                return;
            }
            if (root.left != null)
            {
                PrintAsTree_GetLine(root.left, depth - 1, vals);
            }
            else if (depth - 1 <= 0)
            {
                vals.Add(null);            
            }
            if (root.right != null)
            {
                PrintAsTree_GetLine(root.right, depth - 1, vals);
            }
            else if (depth - 1 <= 0)
            {
                vals.Add(null);            
            }
        }


        public string PrintInOrder_Recursive()
        {
            var sb = new StringBuilder();

            PrintInOrder_Recursive_Helper(root, sb);

            return sb.ToString();
        }

        private static void PrintInOrder_Recursive_Helper(Node<T> curr, StringBuilder sb)
        {
            if (curr == null)
                return;

            PrintInOrder_Recursive_Helper(curr.left, sb);
            // Total number of impressions desired is one more than the dupeCount
            for (var i = 1; i <= curr.dupeCount + 1; ++i)
            {
                sb.Append($"{curr.data} ");
            }
            PrintInOrder_Recursive_Helper(curr.right, sb);
        }

        public string PrintInOrder_Iterative()
        {
            throw new NotImplementedException();

            //return string.Empty;
        }


        private Node<T> root;

#pragma warning disable CS0693 // Type parameter has the same name as the type parameter from outer type
        private class Node<T>
#pragma warning restore CS0693 // Type parameter has the same name as the type parameter from outer type
        {
            public Node()
            {
                left = right = parent = null;
                dupeCount = 0;
            }

            public Node<T> left;

            public Node<T> right;

            public Node<T> parent;

            public T data;

            public int dupeCount;
        }

    }

}
