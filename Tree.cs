using System;
using System.Collections.Generic;

namespace FreeTime
{
    public abstract class Tree<T> //where T : TreeNode<T>
    {
        private TreeNode<T> _root;

        #region Constructors and Destructor

        public Tree() : this(default(TreeNode<T>)) { }
        public Tree(TreeNode<T> root) { _root = root; }

        ~Tree() { Clear(); }

        #endregion

        #region Fields

        public TreeNode<T> Root { get { return _root; } set { _root = value; } }

        #endregion
        
        #region Balance

        public virtual void RecalculateHeight(TreeNode<T> current)
        {
            if (current == null)
                return;
            int max_height = -1;
            TreeNode<T> child;
            for (int i = 0; i < current.Neighbors.Length; i++)
            {
                child = current.Neighbors[i];
                RecalculateHeight(child);
                if (child != null && child.Height > max_height)
                    max_height = child.Height;
            }
            current.Height = max_height + 1;
        }

        #endregion
        
        #region Visualization

        /// <summary>
        /// This method will return the contents of the Tree.
        /// 
        /// NOTE:
        /// This method only supports printing using one of the
        /// following values:
        /// Enumerations.PrintTreeMode.ZigZagStartLeft
        /// Enumerations.PrintTreeMode.ZigZagStartRight
        /// Enumerations.PrintTreeMode.LeftToRight
        /// Enumerations.PrintTreeMode.RightToLeft
        /// Enumerations.PrintTreeMode.Entire
        /// </summary>
        /// <param name="current">The current node in the recursive call.</param>
        /// <param name="expression">The method of printing the Tree.</param>
        /// <param name="MAX_NODE_VALUE_LENGTH">The maximum length of the node values (optional) - used when expression = Enumerations.PrintTreeMode.Entire</param>
        public virtual string Print(TreeNode<T> current, Enumerations.PrintTreeMode expression, int MAX_NODE_VALUE_LENGTH = 4)
        {
            if (current == null)
                return string.Empty;
            if (expression == Enumerations.PrintTreeMode.ZigZagStartLeft || expression == Enumerations.PrintTreeMode.ZigZagStartRight)
            {
                Stack<TreeNode<T>> stack1 = new Stack<TreeNode<T>>();
                Stack<TreeNode<T>> stack2 = new Stack<TreeNode<T>>();
                stack1.Push(current);
                return PrintZigZag_Recursive(stack1, stack2, (expression == Enumerations.PrintTreeMode.ZigZagStartLeft) ? 0 : 1);
            }
            else if (expression == Enumerations.PrintTreeMode.LeftToRight || expression == Enumerations.PrintTreeMode.RightToLeft)
            {
                List<TreeNode<T>> list1 = new List<TreeNode<T>>();
                List<TreeNode<T>> list2 = new List<TreeNode<T>>();
                list1.Add(current);
                return PrintByLevel_Recursive(list1, list2, expression);
            }
            else if (expression == Enumerations.PrintTreeMode.Entire)
            {
                List<TreeNode<T>> list1 = new List<TreeNode<T>>();
                List<TreeNode<T>> list2 = new List<TreeNode<T>>();
                list1.Add(current);
                return PrintEntire_Recursive(list1, list2, current.Height, MAX_NODE_VALUE_LENGTH);
            }
            else
                throw new ArgumentException("The provided expression is not supported by this Print method.");
        }

        #region Recursive Print Methods

        private string PrintZigZag_Recursive(Stack<TreeNode<T>> s1, Stack<TreeNode<T>> s2, int level)
        {
            string representation = string.Empty;
            TreeNode<T> node;
            while (s1.Count > 0)
            {
                node = s1.Pop();
                if (node == null)
                    continue;
                representation += node.Value + " ";
                if (level % 2 == 0) // left to right when popped
                    for (int i = node.Neighbors.Length - 1; i >= 0; i--)
                        s2.Push(node.Neighbors[i]);
                else // right to left when popped
                    for (int i = 0; i < node.Neighbors.Length; i++)
                        s2.Push(node.Neighbors[i]);
            }
            if (s2.Count > 0)
                return representation + "\n" + PrintZigZag_Recursive(s2, s1, level + 1);
            return representation;
        }

        private string PrintByLevel_Recursive(List<TreeNode<T>> l1, List<TreeNode<T>> l2, Enumerations.PrintTreeMode left_right)
        {
            // First, get the children of all of the nodes in list 1
            string representation = string.Empty;
            TreeNode<T> node;
            while (l1.Count > 0)
            {
                node = l1[0];
                l1.RemoveAt(0);
                if (node == null)
                    continue;
                representation += node.Value + " ";
                if (left_right == Enumerations.PrintTreeMode.LeftToRight)
                    for (int i = 0; i < node.Neighbors.Length; i++)
                        l2.Add(node.Neighbors[i]);
                else
                    for (int i = 0; i < node.Neighbors.Length; i++)
                        l2.Add(node.Neighbors[i]);
            }

            // If list 2 received any nodes, print them
            if (l2.Count > 0)
                return representation + "\n" + PrintByLevel_Recursive(l2, l1, left_right);
            return representation;
        }

        private string PrintEntire_Recursive(List<TreeNode<T>> l1, List<TreeNode<T>> l2, int height, int space)
        {
            string representation = string.Empty;
            if (height > 0)
                representation += string.Empty.PadRight((int)(Math.Pow(2, height) - 1) * space);
            // First, get the children of all of the nodes in list 1
            TreeNode<T> node;
            while (l1.Count > 0)
            {
                node = l1[0];
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
                for (int i = 0; i < node.Neighbors.Length; i++)
                    l2.Add(node.Neighbors[i]);
            }

            // If list 2 received any nodes, print them
            if (l2.Count > 0)
                return representation + "\n" + PrintEntire_Recursive(l2, l1, height - 1, space);
            return representation;
        }

        #endregion

        #endregion

        #region Clean up

        public virtual void Clear() { _root = default(TreeNode<T>); }

        #endregion
    }
}
