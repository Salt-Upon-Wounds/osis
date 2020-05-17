using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class Tree<T> where T: IComparable<T>
    {        
        public Node<T> Root; // корень
        public Node<T> current;
        public Tree()
        {
            Root = null;
            
        }
        // поворот влево
        public Node<T> RotaeLeft(Node<T> q)
        {
            Node<T> p = q.Right;
            if(p != null)
            {
                p.Parent = q.Parent;
            }
            q.Right = p.Left;
            p.Left = q;
            q.Parent = p;
            if(q.Right != null)
                q.Right.Parent = q;
            fixeheight(ref q);
            fixeheight(ref p);
            return p;
        } 
        // поворот вправо
        public Node<T> RotaeRight(Node<T> q)
        {
            Node<T> p = q.Left;
            if(p != null)
            {
                p.Parent = q.Parent;
            }
            q.Left = p.Right;
            p.Right = q;
            q.Parent = p;
            if(q.Left != null)
            {
                q.Left.Parent = q;
            }
            fixeheight(ref q);
            fixeheight(ref p);
            return p;
        }
        // возвращает высоту поддерева
        public int height(Node<T> p)
        {
            if (p == null) return 0;
            else return max(height(p.Left) + 1, height(p.Right) + 1);
        }
        // показывает на сколько правое поддерево выше левого
        public int bfactor(Node<T> p)
        {
            return height(p.Right) - height(p.Left);
        }
        // исправляет высоту дерева (после поворота)
        public void fixeheight(ref Node<T> p)
        {
            int h1 = height(p.Left);
            int h2 = height(p.Right);
            p.Height = max(h1, h2) + 1;
        }
        // балансирует дерево
        public Node<T> balance(ref Node<T> p)
        {
            fixeheight(ref p);
            if(bfactor(p) == 2)
            {
                if(bfactor(p.Right) < 0)
                {
                    p.Right = RotaeRight(p.Right);
                }
                return RotaeLeft(p);
            }
            if(bfactor(p) == -2)
            {
                if(bfactor(p.Left) > 0)
                {
                    p.Left = RotaeLeft(p.Left);
                }
                return RotaeRight(p);
            }
            return p;
        }
        // вставка
        public Node<T> Insert(ref Node<T> p, T k, Node<T> par=null)
        {
            if (p == null) return new Node<T>(k, par);
            
            if (k.CompareTo(p.Value) < 0)
            {
                p.Left = Insert(ref p.Left, k, p);
            }
            else
            {
                 p.Right = Insert(ref p.Right, k, p);
            }
            return balance(ref p);
        }
        public Node<T> findmin(Node<T> p)
        {
            if (p.Left == null)
                return p;
            else return findmin(p.Left);
        }
        public Node<T> findmax(Node<T> p)
        {
            if (p.Right == null) return p;
            else return findmax(p.Right);
        }
        public Node<T> removemin(Node<T> p)
        {
            if(p.Left == null)
            {
                return p.Right;
            }
            p.Left = removemin(p.Left);
            return balance(ref p);
        }
        public Node<T> remove(ref Node<T> p, T k)
        {
            if (p == null)
                return null;
            if (k.CompareTo(p.Value) < 0)
            {
                p.Left = remove(ref p.Left, k);
                if (p.Left != null)
                {
                    p.Left.Parent = p;
                }
            }
            else if (k.CompareTo(p.Value) > 0)
            {
                p.Right = remove(ref p.Right, k);
                if (p.Right != null)
                    p.Right.Parent = p;
            }
            else
            {
                Node<T> q = p.Left;
                Node<T> r = p.Right;
                if (r == null)
                {
                    return q;
                }
                Node<T> min = findmin(r);
                min.Right = removemin(r);
                min.Left = q;
                return balance(ref min);
            }
            return balance(ref p);
        }
        public Node<T> search(Node<T> root, T k)
        {
            if (root == null)
                return null;
            if (k.CompareTo(root.Value) < 0)
                return search(root.Left, k);
            else if (k.CompareTo(root.Value) > 0)
                return search(root.Right, k);
            else return root;
        }

        // обходы
        public void PreOrder(Node<T> root)
        {
            if(root != null)
            {
                Console.Write(root.Value + " ");
                PreOrder(root.Left);
                PreOrder(root.Right);
            }
        }

        public void PostOrder(Node<T> root)
        {
            if(root != null)
            {
                PostOrder(root.Left);
                PostOrder(root.Right);
                Console.Write(root.Value + " ");
            }
        }

        public void InOrder(Node<T> root)
        {
            if(root != null)
            {
                InOrder(root.Left);
                Console.Write(root.Value + " ");
                InOrder(root.Right);
            }
        }
        
        public void BFS(Node<T> root)
        {
            if (root != null)
            {
                List<Node<T>> nodes = new List<Node<T>>();
                nodes.Add(root);
                while (nodes.Count != 0)
                {
                    Node<T> next = nodes.First();
                    Console.Write(next.Value + " ");
                    nodes.Remove(next);
                    if (next.Left != null)
                    {
                        nodes.Add(next.Left);
                    }
                    if (next.Right != null)
                    {
                        nodes.Add(next.Right);
                    }
                }
            }
        }
        public Node<T> next(Node<T> current)
        {
            if (current == null)
                return findmin(Root);
            if (current.Right != null)
                return findmin(current.Right);
            Node<T> tmp = current.Parent;
            while(tmp != null && current == tmp.Right)
            {
                current = tmp;
                tmp = tmp.Parent;
            }
            return tmp;
        }

        public Node<T> prev(Node<T> current)
        {
            if (current == null)
                return findmax(Root);
            if (current.Left != null)
                return findmax(current.Left);
            Node<T> tmp = current.Parent;
            while (tmp != null && current == tmp.Left)
            {
                current = tmp;
                tmp = tmp.Parent;
            }
            return tmp;
        }
        //public void makeTree()
        //{
        //    Root = Insert(ref Root, 5);
        //    Root = Insert(ref Root, 2);
        //    Root = Insert(ref Root, 4);
        //    Root = Insert(ref Root, 6);
        //    Root = Insert(ref Root, 7);
        //    Root = Insert(ref Root, 9);
        //    Root = Insert(ref Root, 3);
        //    Root = Insert(ref Root, 8);
        //    Root = Insert(ref Root, 10);
        //    Root = remove(ref Root, 3);
        //    Root = remove(ref Root, 4);
        //    Root = remove(ref Root, 9);
        //    Root = remove(ref Root, 5);
        //    Root = remove(ref Root, 8);
        //    Root = remove(ref Root, 7);
        //    Root = remove(ref Root, 10);
        //    Root = remove(ref Root, 6);
        //    Root = remove(ref Root, 2);
        //}

        int max(int n1, int n2)
        {
            return n1 > n2 ? n1 : n2;
        }

        void clear()
        {
            while(Root != null)
            {
                remove(ref Root, Root.Value);
            }
        }
    }
}
