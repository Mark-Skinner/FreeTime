using System;

namespace FreeTime
{
    public class AVLTree<T> : BinarySearchTree<T> where T : IComparable
    {
        #region Constructors and Destructor

        public AVLTree() : base() { }
        public AVLTree(BinaryTreeNode<T> root) : base(root) { }

        ~AVLTree() { }

        #endregion

        #region CRUD

        public override void Add(T value)
        {
            base.Add(value);
            Balance((BinaryTreeNode<T>)Root);
        }

        public override void Delete(T value)
        {
            base.Delete(value);
        }

        #endregion

        #region Balance

        public void Balance(BinaryTreeNode<T> current)
        {
            if (current.Left != null)
                Balance(current.Left);
            if (current.Right != null)
                Balance(current.Right);
            BinaryTreeNode<T> right_child = current.Right;
            BinaryTreeNode<T> left_child = current.Left;
            int left_height = (left_child == null) ? -1 : left_child.Height;
            int right_height = (right_child == null) ? -1 : right_child.Height;
            if (Math.Abs(left_height - right_height) > 1)
            {
                if (right_height > left_height) // Heavier on the right, perform left rotation
                {
                    left_height = (right_child.Left == null) ? -1 : right_child.Left.Height;
                    right_height = (right_child.Right == null) ? -1 : right_child.Right.Height;
                    if (left_height > right_height) // Heavier on the left, perform LR, or "Double Left"
                        RightRotation(right_child);
                    LeftRotation(current);
                    //else // Heavier on the right (or equal), perform LL, or "Single Left"
                    //{
                    //    LeftRotation(current);
                    //}
                }
                else // Heavier on the left, perform right rotation
                {
                    left_height = (left_child.Left == null) ? -1 : left_child.Left.Height;
                    right_height = (left_child.Right == null) ? -1 : left_child.Right.Height;
                    if (right_height > left_height) // Heavier on the right, perform RL, or "Double Right"
                        LeftRotation(left_child);
                    RightRotation(current);
                    //else // Heavier on the left (or equal), perform RR, or "Single Right"
                    //{

                    //}
                }
                RecalculateHeight(Root);
            }
        }

        private void RightRotation(BinaryTreeNode<T> pivot_node)
        {
            // get left child - we assume it isn't null if we made it here
            BinaryTreeNode<T> left_child = pivot_node.Left;

            // Left subtree of the right child will replace right subtree
            pivot_node.Left = left_child.Right;
            if (left_child.Right != null)
                left_child.Right.Parent = pivot_node;
            // Right child moves to current node's position
            left_child.Parent = pivot_node.Parent;
            pivot_node.Parent = left_child;
            left_child.Right = pivot_node;
            if (left_child.Parent == null) // root
                Root = left_child;
            else
            {
                if (left_child.Value.CompareTo(left_child.Parent.Value) > 0)
                    ((BinaryTreeNode<T>)left_child.Parent).Right = left_child;
                else
                    ((BinaryTreeNode<T>)left_child.Parent).Left = left_child;
            }
        }

        private void LeftRotation(BinaryTreeNode<T> pivot_node)
        {
            // get right child - we assume it isn't null if we made it here
            BinaryTreeNode<T> right_child = pivot_node.Right;

            // Left subtree of the right child will replace right subtree
            pivot_node.Right = right_child.Left;
            if (right_child.Left != null)
                right_child.Left.Parent = pivot_node;
            // Right child moves to current node's position
            right_child.Parent = pivot_node.Parent;
            pivot_node.Parent = right_child;
            right_child.Left = pivot_node;
            if (right_child.Parent == null) // root
                Root = right_child;
            else
            {
                if (right_child.Value.CompareTo(right_child.Parent.Value) > 0)
                    ((BinaryTreeNode<T>)right_child.Parent).Right = right_child;
                else
                    ((BinaryTreeNode<T>)right_child.Parent).Left = right_child;
            }
        }

        #endregion
    }
}
