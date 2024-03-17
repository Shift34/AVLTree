using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using AVLTree;
using System.Collections.Generic;

namespace UnitTestAVLTree
{
    [TestClass]
    public class UnitTest1
    {
        private AVLTree<int, int> avltree;
        [TestMethod]
        public void Count()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            Assert.AreEqual(1, avltree.Count);
        }
        [TestMethod]
        public void FindExestingKey()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            Assert.AreEqual(true, avltree.ContainsKey(n));
        }
        [TestMethod]
        public void FindNoExestingKey()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { 2, 2 }
            };
            Assert.AreEqual(false, avltree.ContainsKey(n));
        }
        [TestMethod]
        public void RemoveExestingKey()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            avltree.Remove(n);
            Assert.AreEqual(0,avltree.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void RemoveNoExestingKey()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            avltree.Remove(2);
        }
        [TestMethod]
        public void AddAndContains()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            avltree.Add(n + 1, n + 1);
            Assert.AreEqual(true,avltree.ContainsKey(n + 1));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddIfKeyExesting()
        {
            int n = 1;
            avltree = new AVLTree<int, int>
            {
                { n, n }
            };
            avltree.Add(n,n);
        }
    }
}
