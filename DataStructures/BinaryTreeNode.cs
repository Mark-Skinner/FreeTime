using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTime
{
    public class BinaryTreeNode<T> : TreeNode<T>
    {
        #region Constructors and Destructor

        public BinaryTreeNode() : base() { }
        public BinaryTreeNode(T data) : base(data) { }
        public BinaryTreeNode(T data, BinaryTreeNode<T> Parent) : base(data, Parent) { }
        public BinaryTreeNode(T data, BinaryTreeNode<T> left, BinaryTreeNode<T> right) : this(data, null, left, right) { }
        public BinaryTreeNode(T data, BinaryTreeNode<T> Parent, BinaryTreeNode<T> left, BinaryTreeNode<T> right) : base(data, Parent)
        {
            base.Neighbors.AddFirst(right);
            base.Neighbors.AddFirst(left);
        }

        ~BinaryTreeNode() { }

        #endregion

        #region Fields

        public BinaryTreeNode<T> Left
        {
            get
            {
                if (base.Neighbors == null || base.Neighbors.Length == 0)
                    return null;
                return (BinaryTreeNode<T>)base.Neighbors[0];
            }
            set
            {
                if (base.Neighbors == null || base.Neighbors.Length == 0)
                    base.Neighbors = new CustomLinkedList<TreeNode<T>>(2);
                base.Neighbors[0] = value;
            }
        }

        public BinaryTreeNode<T> Right
        {
            get
            {
                if (base.Neighbors == null || base.Neighbors.Length == 0)
                    return null;
                return (BinaryTreeNode<T>)base.Neighbors[1];
            }
            set
            {
                if (base.Neighbors == null || base.Neighbors.Length == 0)
                    base.Neighbors = new CustomLinkedList<TreeNode<T>>(2);
                base.Neighbors[1] = value;
            }
        }

        #endregion
    }
}
