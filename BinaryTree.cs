using System;
using System.Text;
using System.Collections.Generic;

namespace FreeTime
{
    public abstract class BinaryTree<T> : Tree<T> where T : IComparable
    {
        #region Constructors and Destructor

        public BinaryTree() : base() { }
        public BinaryTree(BinaryTreeNode<T> root) : base(root) { }

        ~BinaryTree() { }

        #endregion

        #region Balance
        
        public override void RecalculateHeight(TreeNode<T> current)
        {
            if (current == null)
                return;
            RecalculateHeight(((BinaryTreeNode<T>)current).Left);
            RecalculateHeight(((BinaryTreeNode<T>)current).Right);
            int left_height = (((BinaryTreeNode<T>)current).Left == null) ? -1 : ((BinaryTreeNode<T>)current).Left.Height;
            int right_height = (((BinaryTreeNode<T>)current).Right == null) ? -1 : ((BinaryTreeNode<T>)current).Right.Height;
            current.Height = Math.Max(left_height, right_height) + 1;
        }

        #endregion

        #region Validation

        // recursive check, can also be done with inorder traversal
        /// <summary>
        /// Recursively checks if the BinaryTree is valid.
        /// Verifies that all nodes to the left of a given
        /// node have lesser values and that all nodes to
        /// the right of a given node have greater values.
        /// </summary>
        /// <param name="current">The node that is currently being checked.</param>
        /// <param name="min">The lowest value found before reaching the current node.</param>
        /// <param name="max">The highest value found before reaching the current node.</param>
        /// <returns></returns>
        public bool IsValid(BinaryTreeNode<T> current, T min, T max)
        {
            if (current == null)
                return true;
            return (current.Value.CompareTo(min) > 0 &&
                    current.Value.CompareTo(max) < 0 &&
                    IsValid(current.Left, min, current.Value) &&
                    IsValid(current.Right, current.Value, max));
        }

        /// <summary>
        /// States whether or not the current node is a left
        /// child of its parent.
        /// </summary>
        /// <param name="current">The node to look at.</param>
        /// <returns>True if the node is the parent's left child, false otherwise.</returns>
        public bool IsLeftChild(BinaryTreeNode<T> current)
        {
            if (current == null || current.Parent == null)
                return false;

            return current.Value.CompareTo(current.Parent.Value) < 0;
        }

        /// <summary>
        /// States whether or not the current node is a right
        /// child of its parent.
        /// </summary>
        /// <param name="current">The node to look at.</param>
        /// <returns>True if the node is the parent's right child, false otherwise.</returns>
        public bool IsRightChild(BinaryTreeNode<T> current)
        {
            if (current == null || current.Parent == null)
                return false;

            return current.Value.CompareTo(current.Parent.Value) > 0;
        }

        // C++ implementation
        //bool recursive_check(Node* current, int min, int max)
        //{
        //    if (current == NULL)
        //        return true;
        //    return (current->data > min &&
        //            current->data < max &&
        //            recursive_check(current->left, min, current->data) &&
        //            recursive_check(current->right, current->data, max));
        //}

        #endregion

        #region Visualization

        public override string Print(TreeNode<T> current, Enumerations.PrintTreeMode expression, int MAX_NODE_VALUE_LENGTH = 4)
        {
            if (current == null)
                return string.Empty;
            
            if (expression == Enumerations.PrintTreeMode.BinaryPrefix ||
                expression == Enumerations.PrintTreeMode.BinaryInfix ||
                expression == Enumerations.PrintTreeMode.BinaryPostfix)
            {
                return PrintBinary_Recursive((BinaryTreeNode<T>)current, expression);
            }
            else if (expression == Enumerations.PrintTreeMode.ZigZagStartLeft ||
                    expression == Enumerations.PrintTreeMode.ZigZagStartRight)
            {
                return PrintZigZag_Iterative(current, expression);
            }
            else if (expression == Enumerations.PrintTreeMode.LeftToRight ||
                    expression == Enumerations.PrintTreeMode.RightToLeft)
            {
                //base.Print(current, expression);
                List<BinaryTreeNode<T>> list1 = new List<BinaryTreeNode<T>>();
                List<BinaryTreeNode<T>> list2 = new List<BinaryTreeNode<T>>();
                list1.Add((BinaryTreeNode<T>)current);
                return PrintByLevel_Recursive(list1, list2, expression);
            }
            else if (expression == Enumerations.PrintTreeMode.Entire)
            {
                //base.Print(current, expression, MAX_NODE_VALUE_LENGTH);
                List<BinaryTreeNode<T>> list1 = new List<BinaryTreeNode<T>>();
                List<BinaryTreeNode<T>> list2 = new List<BinaryTreeNode<T>>();
                list1.Add((BinaryTreeNode<T>)current);
                return PrintEntire_Recursive(list1, list2, current.Height, MAX_NODE_VALUE_LENGTH);
            }

            return string.Empty;
        }

        #region Recursive Print Methods

        private string PrintBinary_Recursive(BinaryTreeNode<T> current, Enumerations.PrintTreeMode expression)
        {
            if (current == null)
                return string.Empty;

            StringBuilder representation = new StringBuilder();

            if (expression == Enumerations.PrintTreeMode.BinaryPrefix)
            {
                // print parent
                representation.AppendLine("Value: " + Convert.ToString(current.Value) + ", Height: " + current.Height);

                // print left child
                string result = PrintBinary_Recursive(current.Left, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);

                // print right child
                result = PrintBinary_Recursive(current.Right, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);
            }
            else if (expression == Enumerations.PrintTreeMode.BinaryInfix)
            {
                // print left child
                string result = PrintBinary_Recursive(current.Left, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);

                // print parent
                representation.AppendLine("Value: " + Convert.ToString(current.Value) + ", Height: " + current.Height);

                // print right child
                result = PrintBinary_Recursive(current.Right, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);
            }
            else if (expression == Enumerations.PrintTreeMode.BinaryPostfix)
            {
                // print left child
                string result = PrintBinary_Recursive(current.Left, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);

                // print right child
                result = PrintBinary_Recursive(current.Right, expression);
                if (!string.IsNullOrEmpty(result))
                    representation.AppendLine(result);

                // print parent
                representation.AppendLine("Value: " + Convert.ToString(current.Value) + ", Height: " + current.Height);
            }

            return representation.ToString();
        }

        private string PrintZigZag_Recursive(Stack<BinaryTreeNode<T>> s1, Stack<BinaryTreeNode<T>> s2, int level)
        {
            // First, get the children of all of the nodes in stack 1
            string representation = string.Empty;
            BinaryTreeNode<T> node;
            while (s1.Count > 0)
            {
                node = (BinaryTreeNode<T>)s1.Pop();
                if (node == null)
                    continue;
                representation += node.Value + " ";
                if (level % 2 == 0) // left to right when popped
                {
                    s2.Push(node.Right);
                    s2.Push(node.Left);
                }
                else // right to left when popped
                {
                    s2.Push(node.Left);
                    s2.Push(node.Right);
                }
            }

            // If stack 2 received any nodes, print them
            if (s2.Count > 0)
                return representation + "\n" + PrintZigZag_Recursive(s2, s1, level + 1);
            return representation;
        }
        
        private string PrintByLevel_Recursive(List<BinaryTreeNode<T>> l1, List<BinaryTreeNode<T>> l2, Enumerations.PrintTreeMode left_right)
        {
            // First, get the children of all of the nodes in list 1
            string representation = string.Empty;
            BinaryTreeNode<T> node;
            while (l1.Count > 0)
            {
                node = (BinaryTreeNode<T>)l1[0];
                l1.RemoveAt(0);
                if (node == null)
                    continue;
                representation += node.Value + " ";
                if (left_right == Enumerations.PrintTreeMode.LeftToRight)
                {
                    l2.Add(node.Left);
                    l2.Add(node.Right);
                }
                else
                {
                    l2.Add(node.Right);
                    l2.Add(node.Left);
                }
            }

            // If list 2 received any nodes, print them
            if (l2.Count > 0)
                return representation + "\n" + PrintByLevel_Recursive(l2, l1, left_right);
            return representation;
        }

        private string PrintEntire_Recursive(List<BinaryTreeNode<T>> l1, List<BinaryTreeNode<T>> l2, int height, int space)
        {
            string representation = string.Empty;
            if (height > 0)
                representation += string.Empty.PadRight((int)(Math.Pow(2, height) - 1) * space);
            // First, get the children of all of the nodes in list 1
            BinaryTreeNode<T> node;
            while (l1.Count > 0)
            {
                node = (BinaryTreeNode<T>)l1[0];
                l1.RemoveAt(0);
                if (node == null)
                {
                    for (int i = 0; i < space * 2 * (height + 1); i++)
                        representation += " ";
                    continue;
                }
                string val = Convert.ToString(node.Value);
                val = val.PadLeft((space - val.Length + 2) / 2);
                representation += val + string.Empty.PadRight((int)(space * 2 * Math.Pow(2, height) - val.Length));
                l2.Add(node.Left);
                l2.Add(node.Right);
            }

            // If list 2 received any nodes, print them
            if (l2.Count > 0)
                return representation + "\n" + PrintEntire_Recursive(l2, l1, height - 1, space);
            return representation;
        }

        #endregion

        #region Iterative Print Methods

        private string PrintBinary_Iterative(BinaryTreeNode<T> Root, Enumerations.PrintTreeMode print_mode)
        {
            if (Root == null)
                return string.Empty;
            //representation += "Value: " + Convert.ToString(current.Value) + ", Height: " + current.Height + "\n";
            //representation += Print(((BinaryTreeNode<T>)current).Left, expression);
            //representation += Print(((BinaryTreeNode<T>)current).Right, expression);

            StringBuilder representation = new StringBuilder();
            BinaryTreeNode<T> Current;
            List<BinaryTreeNode<T>> Nodes = new List<BinaryTreeNode<T>>(Root.Height + 1);
            Nodes.Add(Root);
            int current_node_index = 0;

            while (true)
            {
                Current = Nodes[current_node_index];
                if (print_mode == Enumerations.PrintTreeMode.BinaryPrefix)
                {
                    // print out parent
                    representation.AppendLine("Value: " + Convert.ToString(Current.Value) + ", Height: " + Current.Height);

                    // if left child isn't null, move to that child
                    if (Current.Left != null)
                    {
                        Nodes.Add(Current.Left);
                        current_node_index++;
                    }
                    // else if right child isn't null, move to that child
                    else if (Current.Right != null)
                    {
                        Nodes.Add(Current.Right);
                        current_node_index++;
                    }
                    // else if both children are null, check if we can move up tree
                    else if (current_node_index != 0)
                    {
                        // this part is wrong..

                        // if current node is left child of parent
                        if (IsLeftChild(Current))
                        {
                            // move up the tree until a right child is found
                            while (Current.Right == null)
                            {
                                Nodes.Remove(Current);
                                if (Nodes.Count == 0)
                                    break;
                                Current = Nodes[--current_node_index];
                            }

                            // done traversing tree
                            if (Nodes.Count == 0)
                                break;
                        }
                    }
                    // both children are null and the parent is null, the tree is a single node..
                    else
                        break;
                }
                else if (print_mode == Enumerations.PrintTreeMode.BinaryInfix)
                {

                }
                else if (print_mode == Enumerations.PrintTreeMode.BinaryPostfix)
                {

                }
                break;
            }

            return string.Empty;
        }

        private string PrintZigZag_Iterative(TreeNode<T> Root, Enumerations.PrintTreeMode print_mode)
        {
            // push current node on stack to get started
            Stack<TreeNode<T>> s1 = new Stack<TreeNode<T>>(), s2 = new Stack<TreeNode<T>>();
            if (print_mode == Enumerations.PrintTreeMode.ZigZagStartLeft)
                s1.Push(Root);
            else
                s2.Push(Root);

            StringBuilder representation = new StringBuilder();
            BinaryTreeNode<T> node;
            while (true)
            {
                if (s1.Count > 0)
                {
                    while (s1.Count > 0)
                    {
                        node = (BinaryTreeNode<T>)s1.Pop();
                        if (node == null)
                            continue;
                        representation.Append(node.Value + " ");
                        
                        s2.Push(node.Right);
                        s2.Push(node.Left);
                    }

                    if (s2.Count == 0)
                        break;
                }
                else
                {
                    while (s2.Count > 0)
                    {
                        node = (BinaryTreeNode<T>)s2.Pop();
                        if (node == null)
                            continue;
                        representation.Append(node.Value + " ");
                        
                        s1.Push(node.Left);
                        s1.Push(node.Right);
                    }

                    if (s1.Count == 0)
                        break;
                }

                representation.AppendLine();
            }

            return representation.ToString();
        }

        #endregion

        #endregion
    }
}
