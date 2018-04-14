using System;
using System.Collections.Generic;
using System.Windows;
using MsgBox = System.Windows.Forms.MessageBox;
using Excel = Microsoft.Office.Interop.Excel;

namespace FreeTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AVLTree<double> avl_tree;

        public MainWindow()
        {
            InitializeComponent();

            avl_tree = new AVLTree<double>();
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            //CreateBinaryTree();
        }

        private void btn_AddToTree_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (!double.TryParse(txtbox_NumberToAdd.Text, out value))
                return;
            avl_tree.Add(value);
            //Console.WriteLine("_____________________________________________");
            txtblk_Tree.Text = avl_tree.Print(avl_tree.Root, Enumerations.PrintTreeMode.ZigZagStartLeft);
            //Console.WriteLine("_____________________________________________");
        }

        private void btn_RemoveFromTree_Click(object sender, RoutedEventArgs e)
        {
            double value;
            if (!double.TryParse(txtbox_NumberToAdd.Text, out value))
                return;
            avl_tree.Delete(value);
            //Console.WriteLine("_____________________________________________");
            txtblk_Tree.Text = avl_tree.Print(avl_tree.Root, Enumerations.PrintTreeMode.Entire);
            //Console.WriteLine("_____________________________________________");
        }

        #region Data Structures

        private void LinkedListFun()
        {

        }

        private void CreateBinaryTree()
        {
            try
            {
                //BinaryTree<int> tree = new BinaryTree<int>(new BinaryTreeNode<int>(1));
                //tree.Root.Left = new BinaryTreeNode<int>(2);
                //tree.Root.Right = new BinaryTreeNode<int>(3);
                //tree.Root.Left.Left = new BinaryTreeNode<int>(4);
                //tree.Root.Right.Right = new BinaryTreeNode<int>(5);
                //tree.Root.Left.Left.Right = new BinaryTreeNode<int>(6);
                //tree.Root.Right.Right.Right = new BinaryTreeNode<int>(7);
                //tree.Root.Right.Right.Right.Right = new BinaryTreeNode<int>(8);
                /*
                                1 
                              2   3
                             4      5
                               6     7
                                      8
                */

                //tree.Print(tree.Root, Enumerations.PrintTreeMode.BinaryInfix);

                //BinarySearchTree<int> tree = new BinarySearchTree<int>(new BinaryTreeNode<int>(5));
                //tree.Add(4);
                //tree.Add(2);
                //tree.Add(3);
                //tree.Add(1);
                //tree.Add(7);
                //tree.Add(6);
                //tree.Add(8);
                ///*
                //                5 
                //              4   7
                //             2   6 8
                //            1 3      
                //*/

                //tree.Print(tree.Root, Enumerations.PrintTreeMode.BinaryInfix);

                //BinarySearchTree<int> tree = new BinarySearchTree<int>();
                //for (int i = 0; i < 100; i++)
                //    tree.Add(i);
                //tree.Print(tree.Root, Enumerations.PrintTreeMode.BinaryInfix);

                //BinarySearchTree<int> tree = new BinarySearchTree<int>(new BinaryTreeNode<int>(5));
                //tree.Add(4);
                //tree.Add(2);
                //tree.Add(3);
                //tree.Add(1);
                //tree.Add(6);
                //tree.Add(7);
                //tree.Add(8);
                ///*
                //                5 
                //              4   6
                //             2     7
                //            1 3     8 
                //*/

                //tree.Print(tree.Root, Enumerations.PrintTreeMode.ZigZagStartLeft);
                //tree.Print(tree.Root, Enumerations.PrintTreeMode.ZigZagStartRight);


                //AVLTree<int> tree = new AVLTree<int>(new BinaryTreeNode<int>(10));
                //tree.Add(30);
                ///*
                //                10
                //                  30
                //*/
                //tree.Print(tree.Root, Enumerations.PrintTreeMode.LeftToRight);
                //Console.WriteLine();
                //tree.Add(20);
                ///*
                //                20
                //             10    30
                //*/
                //tree.Print(tree.Root, Enumerations.PrintTreeMode.LeftToRight);
                //Console.WriteLine();
                //tree.Add(40);
                ///*
                //                20
                //             10    30
                //                      40
                //*/
                //tree.Print(tree.Root, Enumerations.PrintTreeMode.LeftToRight);
                //Console.WriteLine();
                //tree.Add(50);
                ///*
                //                 20
                //             10      40
                //                  30    50
                //*/

                //tree.Print(tree.Root, Enumerations.PrintTreeMode.LeftToRight);

                AVLTree<double> tree = new AVLTree<double>();
                for (int i = 1; i <= 15; i++)
                    tree.Add(i);
                for (int i = 0; i <= 15; i++)
                    tree.Add(0.5 + i);
                Console.WriteLine(tree.Print(tree.Root, Enumerations.PrintTreeMode.BinaryPrefix));


                // see how much faster iterative print is..

                //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                //for (int i = 0; i < 100; i++)
                //{
                //    System.Collections.Generic.Stack<TreeNode<double>> s = new System.Collections.Generic.Stack<TreeNode<double>>();
                //    s.Push(tree.Root);
                //    tree.PrintZigZagToString(s, new System.Collections.Generic.Stack<TreeNode<double>>(), 0);
                //}
                //sw.Stop();
                //Console.WriteLine(string.Format("Time: {0}", sw.ElapsedTicks));

                //sw = System.Diagnostics.Stopwatch.StartNew();
                //for (int i = 0; i < 100; i++)
                //    tree.PrintZigZag_Iterative(Enumerations.PrintTreeMode.ZigZagStartLeft);
                //sw.Stop();
                //Console.WriteLine(string.Format("Time: {0}", sw.ElapsedTicks));

            }
            catch (Exception ex) { MsgBox.Show(ex.Message); }
        }

        #region Coding Competition Attempt

        private void LongestIncreasingSubsequence()
        {
            string[] m_n_split;
            int m, n;
            long count;
            short pairs = Convert.ToInt16(Console.ReadLine());
            for (short i = 0; i < pairs; i++)
            {
                m_n_split = Console.ReadLine().Split(' ');
                m = Convert.ToInt32(m_n_split[0]);
                n = Convert.ToInt32(m_n_split[1]);
                count = Combinations(m,n);
                count *= m;
                for (int j = m; j > n-1; j--)
                    count -= (j == m) ? (j - 1) : (j - 1) * (m-j+2);

                // first case: 1-n stay together
                //count += (m - n + 1)*(m - n) * n;
                //// subtract cases where sequence repeats
                //if (m >= n * 2)
                //    count -= (m == n * 2) ? 1 : Convert.ToInt64(Math.Pow(2,m-n-1));
                // second case: 1-n separate
                //if (n > 2)
                //    count += (m - n) * n;
                Console.WriteLine(count);
            }
        }

        private int Combinations(int n, int r)
        {
            return Factorial(n)/(Factorial(r)*Factorial(n-r));
        }

        private int Factorial(int n)
        {
            if (n < 2)
                return 1;
            int num = n;
            for (int i = n - 1; i > 1; i--)
                num *= i;
            return num;
        }

        #endregion

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
