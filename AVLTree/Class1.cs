using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AVLTree
{
    public class Node<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
        public Node<TKey, TValue> Left, Right, Parent;
        public Node(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public int Height { get; set; } = 1;
        internal void RecalculateHeight()
        {
            var currentNode = this;

            while (currentNode != null)
            {
                if (currentNode.Left == null && currentNode.Right == null)
                {
                    currentNode.Height = 1;
                }
                else if (currentNode.Left == null)
                {
                    currentNode.Height = currentNode.Right.Height + 1;
                }
                else if (currentNode.Right == null)
                {
                    currentNode.Height = currentNode.Left.Height + 1;
                }
                else
                {
                    currentNode.Height = Math.Max(currentNode.Right.Height, currentNode.Left.Height) + 1;
                }
                currentNode = currentNode.Parent;
            }
        }
    }
    public class AVLTree<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public int Count { get; private set; }
        private IComparer<TKey> _comparer;
        private Node<TKey, TValue> _root; //корень
        public AVLTree() : this(null, Comparer<TKey>.Default)
        { }
        public AVLTree(IComparer<TKey> comparer) : this(null, comparer)
        { }
        public AVLTree(IDictionary<TKey, TValue> dictionary) : this(dictionary, Comparer<TKey>.Default)
        { }
        public AVLTree(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
        {
            _comparer = comparer;
            Count = 0;
            _root = null;
            if (dictionary != null && dictionary.Count > 0)
            {
                foreach (var pair in dictionary)
                {
                    Add(pair.Key, pair.Value);
                }
            }
        }
        private int Height(Node<TKey, TValue> node)
        {
            if (node == null)
            {
                return 0;
            }

            return node.Height;
        }
        private int GetBalance(Node<TKey, TValue> node)
        {
            if (node == null)
            {
                return 0;
            }

            return Height(node.Left) - Height(node.Right);
        }

        private void ReplaceNodes(Node<TKey, TValue> replaceableNode, Node<TKey, TValue> successorNode)
        {
            if (successorNode == null)
            {
                if (replaceableNode == _root)
                {
                    _root = null;
                }
                else if (replaceableNode.Parent.Left != null && replaceableNode.Parent.Left == replaceableNode)
                {
                    replaceableNode.Parent.Left = null;
                }
                else
                {
                    replaceableNode.Parent.Right = null;
                }

                return;
            }

            replaceableNode.Key = successorNode.Key;
            replaceableNode.Value = successorNode.Value;

            if (successorNode.Parent != null)
            {
                if (successorNode.Left != null)
                {
                    successorNode.Left.Parent = successorNode.Parent;
                    if (successorNode.Parent.Left == successorNode)
                    {
                        successorNode.Parent.Left = successorNode.Left;
                    }
                    else
                    {
                        successorNode.Parent.Right = successorNode.Left;
                    }
                }
                else if (successorNode.Right != null)
                {
                    successorNode.Right.Parent = successorNode.Parent;
                    if (successorNode.Parent.Left == successorNode)
                    {
                        successorNode.Parent.Left = successorNode.Right;
                    }
                    else
                    {
                        successorNode.Parent.Right = successorNode.Right;
                    }
                }
                else
                {
                    if (successorNode.Parent.Left == successorNode)
                    {
                        successorNode.Parent.Left = null;
                    }
                    else
                    {
                        successorNode.Parent.Right = null;
                    }
                }
            }
        }
        private void BalanceTree(Node<TKey, TValue> node)
        {
            while (node != null)
            {
                int balance = GetBalance(node);

                if (balance > 1)
                {
                    int leftbalance = GetBalance(node.Left);

                    if (leftbalance <= 0)
                    {
                        RotateLeft(node.Left);
                    }

                    RotateRight(node);

                }
                else if (balance < -1)
                {
                    int rightbalance = GetBalance(node.Right);

                    if (rightbalance >= 0)
                    {
                        RotateRight(node.Right);
                    }

                    RotateLeft(node);
                }

                node = node.Parent;
            }
        }

        private void RotateLeft(Node<TKey, TValue> node)
        {
            Node<TKey, TValue> newNode = node.Right;
            node.Right = newNode.Left;
            if (newNode.Left != null)
            {
                newNode.Left.Parent = node;
            }

            newNode.Parent = node.Parent;

            if (node.Parent == null)
            {
                _root = newNode;
            }
            else if (node == node.Parent.Left)
            {
                node.Parent.Left = newNode;
            }
            else
            {
                node.Parent.Right = newNode;
            }

            newNode.Left = node;
            node.Parent = newNode;

            node.RecalculateHeight();
            newNode.RecalculateHeight();
        }
        internal void RotateRight(Node<TKey, TValue> node)
        {
            Node<TKey, TValue> newNode = node.Left;
            node.Left = newNode.Right;
            if (newNode.Right != null)
            {
                newNode.Right.Parent = node;
            }

            newNode.Parent = node.Parent;

            if (node.Parent == null)
            {
                _root = newNode;
            }
            else if (node == node.Parent.Right)
            {
                node.Parent.Right = newNode;
            }
            else
            {
                node.Parent.Left = newNode;
            }

            newNode.Right = node;
            node.Parent = newNode;

            node.RecalculateHeight();
            newNode.RecalculateHeight();
        }
        public void Remove(TKey key)
        {
            var removableNode = Find(key);
            if(removableNode == null)
            {
                throw new KeyNotFoundException();
            }
            var successorNode = removableNode;
            Node<TKey, TValue> successorNodeParent = null;

            if (successorNode.Left != null)
            {
                successorNode = successorNode.Left;

                while (successorNode.Right != null)
                {
                    successorNode = successorNode.Right;
                }

                successorNodeParent = successorNode.Parent;
            }
            else if (successorNode.Right != null)
            {
                successorNode = successorNode.Right;

                while (successorNode.Left != null)
                {
                    successorNode = successorNode.Left;
                }

                successorNodeParent = successorNode.Parent;
            }
            else
            {
                successorNode = null;
                successorNodeParent = removableNode.Parent != null ? removableNode.Parent : null;
            }
            ReplaceNodes(removableNode, successorNode);
            if (successorNodeParent != null)
            {
                successorNodeParent.RecalculateHeight();
                BalanceTree(successorNodeParent);
            }
            Count--;
        }
        public void Add(TKey key, TValue value)
        {
            var node = new Node<TKey, TValue>(key, value);
            if (_root == null)
            {
                _root = node;
                Count++;
                return;
            }
            var current = _root;
            var parent = _root;
            while (current != null)
            {
                parent = current;
                if (_comparer.Compare(current.Key, node.Key) == 0)
                {
                    throw new ArgumentException("Such key is already added");
                }
                if (_comparer.Compare(current.Key, node.Key) > 0)
                {
                    current = current.Left;
                }
                else if (_comparer.Compare(current.Key, node.Key) < 0)
                {
                    current = current.Right;
                }
            }
            if (_comparer.Compare(parent.Key, node.Key) > 0)
            {
                parent.Left = node;
            }
            if (_comparer.Compare(parent.Key, node.Key) < 0)
            {
                parent.Right = node;
            }
            node.Parent = parent;
            parent.RecalculateHeight();
            BalanceTree(parent);
            Count++;
        }

        public bool ContainsKey(TKey key)
        {
            // Поиск узла осуществляется другим методом.
            return Find(key) != null;
        }
        private Node<TKey, TValue> Find(TKey findKey)
        {
            // Попробуем найти значение в дереве.
            var current = _root;

            // До тех пор, пока не нашли...
            while (current != null)
            {
                int result = _comparer.Compare(current.Key, findKey);
                if (result > 0)
                {
                    // Если искомое значение меньше, идем налево.
                    current = current.Left;
                }
                else if (result < 0)
                {
                    // Если искомое значение больше, идем направо.
                    current = current.Right;
                }
                else
                {
                    // Если равны, то останавливаемся
                    break;
                }
            }
            return current;
        }
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                return node == null ? throw new KeyNotFoundException() : node.Value;
            }
            set
            {
                if (key == null)
                    throw new ArgumentNullException();
                var node = Find(key);
                if (node == null)
                    Add(key, value);
                else node.Value = value;
            }
        }
        public void Clear()
        {
            _root = null;
            Count = 0;
        }

        public bool ContainsValue(TValue value)
        {
            var comparer = EqualityComparer<TValue>.Default;
            foreach (var keyValuePair in Traverse())
            {
                if (comparer.Equals(value, keyValuePair.Value))
                    return true;
            }
            return false;
        }
        IEnumerable<KeyValuePair<TKey, TValue>> Traverse(Node<TKey, TValue> node)
        {
            var nodes = new List<KeyValuePair<TKey, TValue>>();
            if (node != null)
            {
                nodes.AddRange(Traverse(node.Left));
                nodes.Add(new KeyValuePair<TKey, TValue>(node.Key, node.Value));
                nodes.AddRange(Traverse(node.Right));
            }
            return nodes;
        }
        public IEnumerable<KeyValuePair<TKey, TValue>> Traverse()
        {
            return Traverse(_root);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Traverse().GetEnumerator();
        }

    }

}

