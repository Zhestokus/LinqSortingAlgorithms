using System;
using System.Collections;
using System.Collections.Generic;

namespace SortingConsoleApp.Common.Sorters
{
    public class RbTreeSorter<TElement> : SorterBase<TElement>
    {
        public override IEnumerable<TElement> Sort(IEnumerable<TElement> collection, IComparer<TElement> comparer, bool parallel)
        {
            var tree = new RbTree(comparer);

            foreach (var element in collection)
                tree.Add(element);

            return tree;
        }

        private class RbTree : IEnumerable<TElement>
        {
            private int _count;

            private RbNode _root;

            private readonly IComparer<TElement> _comparer;

            public RbTree(IComparer<TElement> comparer)
            {
                _comparer = comparer;
            }

            public IEnumerator<TElement> GetEnumerator()
            {
                foreach (var node in InOrderTravers(_root))
                    yield return node.Item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(TElement item)
            {
                if (_root == null)
                {
                    _root = new RbNode(item, false);
                    return;
                }

                var current = _root;
                var parent = (RbNode)null;
                var grandParent = (RbNode)null;
                var greatGrandParent = (RbNode)null;

                var order = 0;

                while (current != null)
                {
                    order = CompareItems(item, current.Item);
                    if (order == 0)
                    {
                        _root.IsRed = false;
                        _count++;

                        return;
                    }

                    if (Is4Node(current))
                    {
                        Split4Node(current);

                        if (IsRed(parent))
                        {
                            parent = InsertionBalance(current, parent, grandParent, greatGrandParent);
                        }
                    }

                    greatGrandParent = grandParent;
                    grandParent = parent;

                    parent = current;
                    current = (order < 0) ? current.Left : current.Right;
                }

                var node = new RbNode(item);
                if (order > 0)
                {
                    parent.Right = node;
                }
                else
                {
                    parent.Left = node;
                }

                if (parent.IsRed)
                {
                    parent = InsertionBalance(node, parent, grandParent, greatGrandParent);
                }

                _root.IsRed = false;
                _count++;
            }

            private IEnumerable<RbNode> InOrderTravers(RbNode node)
            {
                /*
                iterativeInorder(node)
                  parentStack = empty stack
                  while (not parentStack.isEmpty() or node ≠ null)
                    if (node ≠ null)
                      parentStack.push(node)
                      node = node.left
                    else
                      node = parentStack.pop()
                      visit(node)
                      node = node.right
                */

                if (node == null)
                    yield break;

                var capacity = 2 * (int)Math.Log(_count + 1, 2);
                var parentStack = new Stack<RbNode>(capacity);

                while (parentStack.Count > 0 || node != null)
                {
                    if (node != null)
                    {
                        parentStack.Push(node);
                        node = node.Left;
                    }
                    else
                    {
                        node = parentStack.Pop();
                        yield return node;
                        node = node.Right;
                    }
                }
            }

            private void ReplaceChildOrRoot(RbNode parent, RbNode child, RbNode newChild)
            {
                if (parent != null)
                {
                    if (parent.Left == child)
                    {
                        parent.Left = newChild;
                    }
                    else
                    {
                        parent.Right = newChild;
                    }
                }
                else
                {
                    _root = newChild;
                }
            }

            private RbNode InsertionBalance(RbNode current, RbNode parent, RbNode grandParent, RbNode greatGrandParent)
            {
                var parentIsOnRight = (grandParent.Right == parent);
                var currentIsOnRight = (parent.Right == current);

                RbNode newChildOfGreatGrandParent;
                if (parentIsOnRight == currentIsOnRight)
                {
                    // same orientation, single rotation
                    newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
                }
                else
                {
                    // different orientaton, double rotation
                    newChildOfGreatGrandParent = (currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent));

                    // current node now becomes the child of greatgrandparent 
                    parent = greatGrandParent;
                }
                // grand parent will become a child of either parent of current.
                grandParent.IsRed = true;
                newChildOfGreatGrandParent.IsRed = false;

                ReplaceChildOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);

                return parent;
            }

            private bool IsRed(RbNode node)
            {
                return (node != null && node.IsRed);
            }
            private bool Is4Node(RbNode node)
            {
                return IsRed(node.Left) && IsRed(node.Right);
            }

            private void Split4Node(RbNode node)
            {
                node.IsRed = true;
                node.Left.IsRed = false;
                node.Right.IsRed = false;
            }

            private RbNode RotateLeft(RbNode node)
            {
                var x = node.Right;
                node.Right = x.Left;
                x.Left = node;
                return x;
            }

            private RbNode RotateLeftRight(RbNode node)
            {
                var child = node.Left;
                var grandChild = child.Right;

                node.Left = grandChild.Right;
                grandChild.Right = node;

                child.Right = grandChild.Left;
                grandChild.Left = child;

                return grandChild;
            }

            private RbNode RotateRight(RbNode node)
            {
                var x = node.Left;
                node.Left = x.Right;

                x.Right = node;
                return x;
            }

            private RbNode RotateRightLeft(RbNode node)
            {
                var child = node.Right;
                var grandChild = child.Left;

                node.Right = grandChild.Left;
                grandChild.Left = node;

                child.Left = grandChild.Right;
                grandChild.Right = child;

                return grandChild;
            }

            private int CompareItems(TElement x, TElement y)
            {
                var order = _comparer.Compare(x, y);

                if (order == 0)
                    order = -1;

                return order;
            }
        }

        private class RbNode
        {
            public TElement Item;
            public bool IsRed;

            public RbNode Left;
            public RbNode Right;

            public RbNode(TElement item)
            {
                Item = item;
                IsRed = true;
            }
            public RbNode(TElement item, bool isRed)
            {
                Item = item;
                IsRed = isRed;
            }
        }
    }
}
