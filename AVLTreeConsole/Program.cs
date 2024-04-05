using AVLTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AVLTreeConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 10000;
            int[] array = new int[n];
            AVLTree<int, int> bintree = new AVLTree<int, int>();

            Random randNum = new Random();
            for (int i = 0; i < array.Length; i++)
            {
                bool flag = true;
                while (flag)
                {
                    int randInt = randNum.Next(0, 3 * n);
                    if (!array.Contains(randInt))
                    {
                        array[i] = randInt;
                        flag = false;
                    }
                }
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            foreach (var t in array)
            {
                bintree.Add(t, 0);
            }
            for (int i = 5000; i < 7000; i++)
            {
                if (!bintree.Remove(array[i])) Console.WriteLine("No Remove Element");
            }
            for (int i = 0; i < 5000; i++)
            {
                if (!bintree.ContainsKey(array[i])) Console.WriteLine("No Find");
            }
            for (int i = 7000; i < 10000; i++)
            {
                if (!bintree.ContainsKey(array[i])) Console.WriteLine("No Find");
            }
            stopWatch.Stop();
            Console.WriteLine("Binary Tree: {0}", stopWatch.ElapsedMilliseconds);

            SortedDictionary<int, int> sortdict = new SortedDictionary<int, int>();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            foreach (var t in array)
            {
                sortdict.Add(t, 0);
            }
            for (int i = 5000; i < 7000; i++)
            {
                if (!sortdict.Remove(array[i])) Console.WriteLine("No Remove Element");
            }
            for (int i = 0; i < 5000; i++)
            {
                if (!sortdict.ContainsKey(array[i])) Console.WriteLine("No Find");
            }
            for (int i = 7000; i < 10000; i++)
            {
                if (!sortdict.ContainsKey(array[i])) Console.WriteLine("No Find");
            }
            watch.Stop();
            Console.WriteLine("Sorted Dictionary: {0}", watch.ElapsedMilliseconds);

            Console.ReadKey();
        }
    }
}

