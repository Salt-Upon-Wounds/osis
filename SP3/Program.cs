using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTree
{
    class Program
    {
        static Tree<int> tree = new Tree<int>();
        static void choose()
        {
            int a = 0;
            while (a < 10)
            {
                Console.Write("1. Insert\n" +
                    "2. Remove\n" +
                    "3. PostOrder\n" +
                    "4. PreOrder\n" +
                    "5. InOrder\n" +
                    "6. BFS\n" +
                    "7. Search\n" +
                    "8. Next\n" +
                    "9. Prev\n" +
                    "10. Exit\n");
                a = Convert.ToInt32(Console.ReadLine());
                int b = 0;
                switch (a)
                {
                    case 1:
                        Console.WriteLine("Input inserted number: ");
                        b = Convert.ToInt32(Console.ReadLine());
                        tree.Root = tree.Insert(ref tree.Root, b);
                        break;
                    case 2:
                        Console.WriteLine("Input removed number: ");
                        b = Convert.ToInt32(Console.ReadLine());
                        tree.Root = tree.remove(ref tree.Root, b);
                        break;
                    case 3:
                        Console.WriteLine("PostOrder");
                        tree.PostOrder(tree.Root);
                        Console.WriteLine();
                        break;
                    case 4:
                        Console.WriteLine("\nPreOrder");

                        tree.PreOrder(tree.Root);
                        Console.WriteLine();
                        break;
                    case 5:
                        Console.WriteLine("\nInOrder");

                        tree.InOrder(tree.Root);
                        Console.WriteLine();
                        break;
                    case 6:
                        Console.WriteLine("\nBFS");

                        tree.BFS(tree.Root);
                        Console.WriteLine();
                        break;
                    case 7:
                        Console.WriteLine("Input removed number: ");
                        b = Convert.ToInt32(Console.ReadLine());
                        Node<int> n = tree.search(tree.Root, b);
                        if (n == null)
                            Console.WriteLine("null");
                        else Console.WriteLine(n.Value + " are founded");
                        break;
                    case 8:
                        tree.current = tree.next(tree.current);
                        if(tree.current != null)
                            Console.WriteLine(tree.current.Value);
                        else
                            Console.WriteLine("NULL");                        
                        break;
                    case 9:
                        tree.current = tree.prev(tree.current);
                        if(tree.current != null)
                            Console.WriteLine(tree.current.Value);                        
                        else
                            Console.WriteLine("NULL");                        
                        break;

                }
            }
        }
        static void Main(string[] args)
        {
            Tree<int> tree = new Tree<int>();
            choose();
            System.Console.ReadKey();
        }
    }
}
