using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTime
{
    public class TreeNode<T>
    {
        private T _value;
        private int _height;
        private TreeNode<T> _parent;
        private CustomLinkedList<TreeNode<T>> _neighbors;

        #region Constructors and Destructor

        public TreeNode() : this(default(T)) { }
        public TreeNode(T Value) : this(Value, null) { }
        public TreeNode(T Value, TreeNode<T> Parent) : this(Value, Parent, null) { }
        public TreeNode(T Value, TreeNode<T> Parent, CustomLinkedList<TreeNode<T>> Neighbors)
        {
            this._value = Value;
            this._parent = Parent;
            this._height = 0;
            this._neighbors = Neighbors ?? new CustomLinkedList<TreeNode<T>>();
        }

        ~TreeNode()
        {
            _value = default(T);
            _height = 0;
            _parent = null;
            _neighbors = null;
        }

        #endregion

        #region Fields

        public T Value { get { return _value; } set { _value = value; } }
        public int Height { get { return _height; } set { _height = value; } }
        public TreeNode<T> Parent { get { return _parent; } set { _parent = value; } }
        public CustomLinkedList<TreeNode<T>> Neighbors { get { return _neighbors; } set { _neighbors = value; } }

        #endregion
    }
}
