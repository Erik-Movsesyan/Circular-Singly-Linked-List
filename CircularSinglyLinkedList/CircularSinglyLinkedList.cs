using System;
using SinglyLinkedList;
using System.Collections;
using System.Collections.Generic;

namespace MyCollections.CircularSinglyLinkedList
{
    public class CircularSinglyLinkedList<T>: IEnumerable<T>
    {
        public CircularSinglyLinkedListNode<T>? First { get; private set; }
        public CircularSinglyLinkedListNode<T>? Last { get; private set; }
        public int Length { get; private set; }

        public CircularSinglyLinkedList() { }

        public CircularSinglyLinkedList(IEnumerable<T>? collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            foreach (T item in collection)
            {
                AddLast(item);
            }
        }

        public CircularSinglyLinkedListNode<T> AddLast(T? value)
        {
            var newNode = new CircularSinglyLinkedListNode<T>(this, value);
            return AddLastInternal(newNode);
        }

        public CircularSinglyLinkedListNode<T> AddLast(CircularSinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddLastInternal(newNode);
        }

        public CircularSinglyLinkedListNode<T> AddFirst(T? value)
        {
            var newNode = new CircularSinglyLinkedListNode<T>(this, value);
            return AddFirstInternal(newNode);
        }

        public CircularSinglyLinkedListNode<T> AddFirst(CircularSinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddFirstInternal(newNode);
        }

        public CircularSinglyLinkedListNode<T> AddAfter(CircularSinglyLinkedListNode<T>? node, T? value)
        {
            ValidateNode(node);
            var newNode = new CircularSinglyLinkedListNode<T>(this, value);

            return AddAfterInternal(node!, newNode);
        }

        public CircularSinglyLinkedListNode<T> AddAfter(CircularSinglyLinkedListNode<T>? node, CircularSinglyLinkedListNode<T>? newNode)
        {
            ValidateNode(node);
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddAfterInternal(node!, newNode);
        }

        public CircularSinglyLinkedListNode<T> AddBefore(CircularSinglyLinkedListNode<T>? node, T? value)
        {
            var newNode = new CircularSinglyLinkedListNode<T>(this, value);

            return AddBeforeInternal(node!, newNode);
        }

        public CircularSinglyLinkedListNode<T> AddBefore(CircularSinglyLinkedListNode<T>? node, CircularSinglyLinkedListNode<T>? newNode)
        {
            ValidateNewNode(newNode);
            newNode!.List = this;

            return AddBeforeInternal(node!, newNode);  
        }

        public bool Remove(T? value)
        {
            var nodeToRemove = Find(value);
            if(nodeToRemove != null)
            {
                RemoveNodeInternal(nodeToRemove);
                return true;
            }

            return false;
        }

        public void Remove(CircularSinglyLinkedListNode<T>? nodeToRemove)
        {
            ValidateNode(nodeToRemove);
            RemoveNodeInternal(nodeToRemove);
        }

        public void RemoveFirst()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(First);
        }

        public void RemoveLast()
        {
            if (First == null)
                throw new InvalidOperationException("The SinglyLinkedList is empty");

            RemoveNodeInternal(Last);
        }

        public void Clear()
        {
            CircularSinglyLinkedListNode<T>? current = First;
            while (current != null)
            {
                CircularSinglyLinkedListNode<T> temp = current;
                current = current.Next;
                temp.Invalidate();
            }

            First = null;
            Last = null;
            Length = 0;
        }

        public bool Contains(T? value) => Find(value) != null;

        public CircularSinglyLinkedListNode<T>? Find(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;

            if (node != null)
            {
                if (value != null)
                {
                    CircularSinglyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            return node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != First);
                }
                else
                {
                    do
                    {
                        if (node!.Value == null)
                        {
                            return node;
                        }
                        node = node.Next;

                    } while (node != First);
                }
            }
            return null;
        }

        public CircularSinglyLinkedListNode<T>? FindLast(T? value)
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;
            CircularSinglyLinkedListNode<T>? resultNode = null;

            if (node != null)
            {
                if (value != null)
                {
                    CircularSinglyLinkedListNode<T>? nextNode;
                    do
                    {
                        if (comparer.Equals(node!.Value, value))
                        {
                            resultNode = node;
                        }
                        nextNode = node.Next;
                        node = nextNode;

                    } while (nextNode != First);

                    return resultNode;
                }
                else
                {
                    do
                    {
                        if (node!.Value == null)
                        {
                            resultNode = node;
                        }
                        node = node.Next;

                    } while (node != First);

                    return resultNode;
                }
            }
            return null;
        }

        public CircularSinglyLinkedListNode<T>? FindBefore(CircularSinglyLinkedListNode<T>? node)
        {
            ValidateNode(node);
            if (node!.List!.Length < 2)
                return null;

            var currentNode = First!.Next;

            CircularSinglyLinkedListNode<T>? previousNode = First;
            CircularSinglyLinkedListNode<T>? resultNode = null;
            do
            {
                if (currentNode == node)
                {
                    resultNode = previousNode;
                    break;
                }

                previousNode = currentNode;
                currentNode = currentNode?.Next;

            } while (currentNode != First);

            return resultNode;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CircularSinglyLinkedListEnumerator<T>(this);
        }

        private CircularSinglyLinkedListNode<T> AddFirstInternal(CircularSinglyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                First = newNode;
                Last = newNode;
                Last.Next = First;
            }
            else
            {
                newNode.Next = First;
                First = newNode;
            }

            Length++;
            return newNode;
        }

        private CircularSinglyLinkedListNode<T> AddLastInternal(CircularSinglyLinkedListNode<T> newNode)
        {
            if (First == null)
            {
                First = newNode;
            }
            else
            {
                Last!.Next = newNode;
            }

            Last = newNode;
            Last.Next = First;

            Length++;
            return newNode;
        }

        private CircularSinglyLinkedListNode<T> AddAfterInternal(CircularSinglyLinkedListNode<T> node, CircularSinglyLinkedListNode<T> newNode)
        {
            if (Last == node)
            {
                newNode.Next = First;
                Last = newNode;
            }
            else
            {
                newNode.Next = node.Next;
            }
            node.Next = newNode;

            Length++;
            return newNode;
        }

        private CircularSinglyLinkedListNode<T> AddBeforeInternal(CircularSinglyLinkedListNode<T> node, CircularSinglyLinkedListNode<T> newNode)
        {
            var precedingNode = FindBefore(node);

            if (precedingNode == null)
            {
                First = newNode;
            }
            else
            {
                precedingNode.Next = newNode;
            }
            newNode.Next = node;

            Length++;
            return newNode;
        }

        private void RemoveNodeInternal(CircularSinglyLinkedListNode<T>? nodeToRemove)
        {
            var previousNode = FindBefore(nodeToRemove);
            if (previousNode != null)
            {
                previousNode.Next = nodeToRemove!.Next;

                if (nodeToRemove == Last)
                    Last = previousNode;
            }
            else
            {
                if(nodeToRemove!.Next != First)
                {
                    First = nodeToRemove.Next;
                    Last!.Next = First;
                }
                else
                {
                    First = null;
                    Last = null;
                }
            }

            nodeToRemove.Invalidate();
            Length--;
        }

        private static void ValidateNewNode(CircularSinglyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != null)
            {
                throw new InvalidOperationException("Node is already part of a different SinglyLinkedList");
            }
        }

        private void ValidateNode(CircularSinglyLinkedListNode<T>? node)
        {
            ArgumentNullException.ThrowIfNull(node);

            if (node.List != this)
            {
                throw new InvalidOperationException("The node does not belong to the current SinglyLinkedList");
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public sealed class CircularSinglyLinkedListNode<T>
    {
        public CircularSinglyLinkedList<T>? List { get; internal set; }
        public CircularSinglyLinkedListNode<T>? Next { get; internal set; }
        public T? Value { get; }

        internal CircularSinglyLinkedListNode(CircularSinglyLinkedList<T> list, T? value)
        {
            List = list;
            Value = value;
        }

        public CircularSinglyLinkedListNode(T? value)
        {
            Value = value;
        }

        public void Invalidate()
        {
            List = null;
            Next = null;
        }
    }
}