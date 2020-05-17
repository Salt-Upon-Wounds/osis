using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class Node<T>
    {
        public int Height { get; set; }
        public T Value { get; set; }
        public Node<T> Left;
        public Node<T> Right;
        public Node<T> Parent;
        public Node(T val, Node<T> _parent)
        {
            Value = val;
            Left = null;
            Right = null;
            Parent = _parent;
            Height = 1;    
        }
        public Node(T val, Node<T> left, Node<T> right)
        {
            Value = val;
            Left = left;
            Right = right;
        }
    }
}
