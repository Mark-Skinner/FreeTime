using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTime
{
    public class CustomLinkedListNode<T>
    {
        private T _value;
        private CustomLinkedListNode<T> _next, _previous;

        #region Constructors and Destructor

        public CustomLinkedListNode() : this(default(T)) { }
        public CustomLinkedListNode(T value)
        {
            _value = value;
        }

        ~CustomLinkedListNode()
        {
            _value = default(T);
            _next = null;
            _previous = null;
        }

        #endregion

        #region Fields

        public T Value { get { return _value; } set { _value = value; } }
        public CustomLinkedListNode<T> Next { get { return _next; } set { _next = value; } }
        public CustomLinkedListNode<T> Previous { get { return _previous; } set { _previous = value; } }

        #endregion
        
        #region Visualization

        public override string ToString()
        {
            return Convert.ToString(_value);
        }

        #endregion
    }
}
