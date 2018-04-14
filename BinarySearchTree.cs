using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTime
{
    public class BinarySearchTree<T> : BinaryTree<T> where T : IComparable
    {
        #region Constructors and Destructor

        public BinarySearchTree() : base() { }

        public BinarySearchTree(BinaryTreeNode<T> root) : base(root) { }

        ~BinarySearchTree() { }

        #endregion

        #region CRUD

        #region Add

        /// <summary>
        /// Adds a node to the BinarySearchTree. This method also
        /// updates the height of the nodes in the BinarySearchTree.
        /// </summary>
        /// <param name="value">The value to add to the BinaryTree.</param>
        public virtual void Add(T value)
        {
            BinaryTreeNode<T> new_node = new BinaryTreeNode<T>(value);
            if (Root == null) { Root = new_node; return; }

            BinaryTreeNode<T> temp = (BinaryTreeNode<T>)Root;
            int compare;
            while (temp != null)
            {
                compare = value.CompareTo(temp.Value);
                if (compare < 0)
                {
                    if (temp.Left == null)
                    {
                        new_node.Parent = temp;
                        temp.Left = new_node;
                        //FixHeight(new_node);
                        RecalculateHeight((BinaryTreeNode<T>)Root);
                        return;
                    }
                    temp = temp.Left;
                }
                else if (compare > 0)
                {
                    if (temp.Right == null)
                    {
                        new_node.Parent = temp;
                        temp.Right = new_node;
                        //FixHeight(new_node);
                        RecalculateHeight((BinaryTreeNode<T>)Root);
                        return;
                    }
                    temp = temp.Right;
                }
                else
                    return;// throw new ArgumentException("The provided value already exists in the data set.");
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// Searches for the node with the provided value in
        /// the BinaryTree.
        /// </summary>
        /// <param name="value">The value to search for in the BinaryTree.</param>
        /// <returns>The BinaryTreeNode that has a value equal to the one provided.</returns>
        public virtual BinaryTreeNode<T> Search(T value)
        {
            BinaryTreeNode<T> temp = (BinaryTreeNode<T>)Root;
            int compare;
            while (temp != null)
            {
                compare = value.CompareTo(temp.Value);
                if (compare < 0)
                    temp = temp.Left;
                else if (compare > 0)
                    temp = temp.Right;
                else
                    return temp;
            }
            return null;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Deletes a node from the BinaryTree that has
        /// a value equal to the one provided. It also
        /// updates the height after removing the node.
        /// </summary>
        /// <param name="value">The value to delete from the BinaryTree.</param>
        public virtual void Delete(T value)
        {
            if (Root == null) return;

            BinaryTreeNode<T> node_to_delete = Search(value);
            if (node_to_delete == null) return;
            BinaryTreeNode<T> parent = (BinaryTreeNode<T>)node_to_delete.Parent;

            int compare = (parent == null) ? 0 : node_to_delete.Value.CompareTo(parent.Value);
            // check if node_to_delete has any children
            #region NO RIGHT
            if (node_to_delete.Right == null) // no right
            {
                if (parent == null) // root
                    Root = node_to_delete.Left;
                else if (compare < 0)
                    parent.Left = node_to_delete.Left;
                else if (compare > 0)
                    parent.Right = node_to_delete.Left;
                if (node_to_delete.Left != null)
                    node_to_delete.Left.Parent = (parent == null) ? Root : parent;
                node_to_delete = null;
            }
            #endregion
            #region HAS RIGHT, RIGHT HAS NO LEFT
            else if (node_to_delete.Right.Left == null) // has right, right has no left
            {
                if (parent == null) // root
                    Root = node_to_delete.Right;
                else if (compare < 0)
                    parent.Left = node_to_delete.Right;
                else if (compare > 0)
                    parent.Right = node_to_delete.Right;
                node_to_delete.Right.Parent = (parent == null) ? Root : parent;
                node_to_delete.Left.Parent = node_to_delete.Right;
                node_to_delete.Right.Left = node_to_delete.Left;
                node_to_delete = null;
            }
            #endregion
            #region HAS RIGHT, RIGHT HAS LEFT
            else
            {
                // find replacement (smallest value on right subtree)
                BinaryTreeNode<T> replacement = node_to_delete.Right.Left;
                while (replacement.Left != null)
                    replacement = replacement.Left;

                // remove replacement from old parent
                ((BinaryTreeNode<T>)replacement.Parent).Left = replacement.Right;
                if (replacement.Right != null)
                    replacement.Right.Parent = replacement.Parent;

                // move replacement to node_to_delete's location
                //replacement.Parent = parent;
                replacement.Left = node_to_delete.Left;
                replacement.Right = node_to_delete.Right;
                replacement.Right.Parent = replacement;
                if (replacement.Left != null)
                    replacement.Left.Parent = replacement;

                if (parent == null) // root
                    Root = replacement;
                else if (compare < 0)
                    parent.Left = replacement;
                else if (compare > 0)
                    parent.Right = replacement;
                replacement.Parent = parent;

                // delete node_to_delete
                node_to_delete = null;
            }
            #endregion

            RecalculateHeight((BinaryTreeNode<T>)Root);
        }

        #endregion

        #endregion
    }
}
