using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeTime
{
    public class CustomLinkedList<T>
    {
        private CustomLinkedListNode<T> _head;
        private int _length;

        #region Constructors and Destructor

        public CustomLinkedList() : this(0) { }
        public CustomLinkedList(int initialSize)
        {
            _length = 0;
            _head = null;
            // Add elements to list
            for (int i = 0; i < initialSize; i++)
                AddFirst(new CustomLinkedListNode<T>()); //default(CustomLinkedListNode<T>));
        }

        ~CustomLinkedList() { Clear(); }

        #endregion

        #region Fields

        public CustomLinkedListNode<T> Head { get { return _head; } set { _head = value; } }
        public int Length { get { return _length; } }

        #endregion
        
        #region Add

        public void AddFirst(T value)
        {
            AddFirst(new CustomLinkedListNode<T>(value));
        }

        private void AddFirst(CustomLinkedListNode<T> value)
        {
            if (_length == 0)
                _head = value;
            else
            {
                CustomLinkedListNode<T> temp = _head;
                temp.Previous = value;
                _head = value;
                _head.Next = temp;
            }
            _length++;
        }
        
        public void AddLast(T value)
        {
            AddLast(new CustomLinkedListNode<T>(value));
        }

        private void AddLast(CustomLinkedListNode<T> value)
        {
            if (_length == 0)
                _head = value;
            else
            {
                CustomLinkedListNode<T> last = FindNode(_length - 1);
                last.Next = value;
                last.Next.Previous = last;
            }
            _length++;
        }

        #endregion

        #region Insert

        public void Insert(int index, T value)
        {
            Insert(index, new CustomLinkedListNode<T>(value));
        }

        private void Insert(int index, CustomLinkedListNode<T> value)
        {
            if (index == 0)
                AddFirst(value);
            else
            {
                CustomLinkedListNode<T> temp = FindNode(index);
                value.Previous = temp.Previous;
                value.Next = temp;
                temp.Previous.Next = value;
                temp.Previous = value;
                _length++;
            }
        }

        #endregion

        #region Delete

        public void Delete(int index)
        {
            Delete(FindNode(index));
        }

        public void Delete(T value)
        {
            Delete(FindNode(value));
        }

        private void Delete(CustomLinkedListNode<T> value)
        {
            if (value.Previous == null) // head
            {
                _head = value.Next;
                if (value.Next != null)
                    value.Next.Previous = _head;
            }
            else
            {
                value.Previous.Next = value.Next;
                if (value.Next != null)
                    value.Next.Previous = value.Previous;
            }
            value = null;
        }
        
        #endregion

        #region Search

        public T this[int index]
        {
            get { return Find(index); }
            set
            {
                if (index == 0)
                {
                    if (_head == null)
                        _head = new CustomLinkedListNode<T>();
                    _head.Value = value;
                }
                else
                {
                    CustomLinkedListNode<T> val = FindNode(index) ?? new CustomLinkedListNode<T>();
                    val.Value = value;
                }
            }
        }

        public T this[T val]
        {
            get { return Find(val); }
            set
            {
                CustomLinkedListNode<T> v = FindNode(val);
                v.Value = value;
            }
        }

        private T Find(int index)
        {
            if (index < 0 || index >= _length)
                throw new IndexOutOfRangeException(string.Format("The index you requested was out of range. Requested index: {0}, Length of LinkedList: {1}.", index, _length));
            CustomLinkedListNode<T> temp = _head;
            for (; index > 0; index--)
                temp = temp.Next;
            return temp.Value;
        }

        private T Find(T value)
        {
            CustomLinkedListNode<T> temp = _head;
            for (int i = 0; i < _length; i++)
            {
                if (temp.Value.Equals(value))
                    return temp.Value;
                temp = temp.Next;
            }
            throw new IndexOutOfRangeException("The value you requested did not exist in the list.");
        }

        private CustomLinkedListNode<T> FindNode(int index)
        {
            if (index < 0 || index >= _length)
                throw new IndexOutOfRangeException(string.Format("The index you requested was out of range. Requested index: {0}, Length of LinkedList: {1}.", index, _length));
            CustomLinkedListNode<T> temp = _head;
            for (; index > 0; index--)
                temp = temp.Next;
            return temp;
        }

        private CustomLinkedListNode<T> FindNode(T value)
        {
            CustomLinkedListNode<T> temp = _head;
            for (int i = 0; i < _length; i++)
            {
                if (temp.Value.Equals(value))
                    return temp;
                temp = temp.Next;
            }
            throw new IndexOutOfRangeException("The value you requested did not exist in the list.");
        }

        #endregion

        #region Visualization

        public void Print()
        {
            CustomLinkedListNode<T> temp = _head;
            while (temp != null)
            { Console.WriteLine(temp); temp = temp.Next; }
        }

        #endregion

        #region Clean up

        public void Clear()
        {
            if (_head != null)
            {
                for (; _length > 0; _length--)
                    this[_length - 1] = default(T);
                _head = null;
            }
            _length = 0;
        }

        #endregion
    }
}
