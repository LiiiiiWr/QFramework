/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * https://github.com/SnowCold/SCFramework_Engine
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
****************************************************************************/

namespace QFramework
{
	using System;
	
	public class BinarySearchTree<T> : Iteratable<T> where T : IBinarySearchTreeElement
	{	
		public enum NodeType
		{
			Left = 0,
			Right = 1,
			Root = 2,
		}

		public class Node
		{
			public Node LeftChild;
			public Node RightChild;
			private Node mParent;
			protected NodeType mNodeType;

			public bool IsLeaf()
			{
				if (LeftChild == null && RightChild == null)
				{
					return true;
				}
				return false;
			}

			public Node Parent
			{
				get { return mParent; }
			}

			public NodeType NodeType
			{
				get { return mNodeType; }
			}

			public void SetParent(Node parent, NodeType nodeType)
			{
				mParent = parent;
				mNodeType = nodeType;
			}

			private T mData;

			public float SortScore
			{
				get { return mData.SortScore; }
			}


			public T Data
			{
				get { return mData; }
			}

			public Node(T data)
			{
				mData = data;
			}
		}

		protected Node mHeapNode;

		public BinarySearchTree()
		{
			
		}

		public void Insert(T[] dataArray)
		{
			if (null == dataArray)
			{
				throw new NullReferenceException("BinarySearchTree Not Support Insert Null Object");
			}

			for (int i = 0; i < dataArray.Length; i++)
			{
				Insert(dataArray[i]);
			}
		}

		public void Insert(T data)
		{
			if (null == data)
			{
				throw new NullReferenceException("BinarySearchTree Not Support Null Object");
			}

			if (null == mHeapNode)
			{
				mHeapNode = new Node(data);
				mHeapNode.SetParent(null,NodeType.Root);
				return;
			}

			Node newNode = new Node(data);

			float score = newNode.SortScore;

			Node preNode = null;
			Node currentNode = mHeapNode;

			while (null != currentNode)
			{
				preNode = currentNode;
				if (score < currentNode.SortScore)
				{
					currentNode = currentNode.LeftChild;
					if (null == currentNode)
					{
						newNode.SetParent(preNode,NodeType.Left);
						preNode.LeftChild = newNode;
						break;
					}
				}
				else
				{
					currentNode = currentNode.RightChild;
					if (currentNode == null)
					{
						newNode.SetParent(preNode,NodeType.Right);
						preNode.RightChild = newNode;
						break;
					}
				}
			}
		}

		protected Node Find(Node head, T data)
		{
			if (null == data)
			{
				return null;
			}

			float score = data.SortScore;
			Node currentNode = head;
			while (null != currentNode)
			{
				if (data.Equals(currentNode.Data))
				{
					break;
				}
				if (score < currentNode.SortScore)
				{
					currentNode = currentNode.LeftChild;
				}
				else
				{
					currentNode = currentNode.RightChild;
				}
			}

			return currentNode;
		}

		public void Remove(T data)
		{
			if (null == data)
			{
				return;
			}

			Node currentNode = Find(mHeapNode, data);

			if (null == currentNode)
			{
				Console.WriteLine("Not Find DeleteNode");
			}

			if (null == currentNode.LeftChild && null == currentNode.RightChild)
			{
				switch (currentNode.NodeType)
				{
					case NodeType.Left:
					{
						currentNode.Parent.LeftChild = null;
					}
						break;
					case NodeType.Right:
					{
						currentNode.Parent.RightChild = null;
					}
						break;
					case NodeType.Root:
					{
						mHeapNode = null;
					}
						break;
					default:
						break;
				}
				return;
			}

			if (null != currentNode.RightChild)
			{
				var rightChild = currentNode.RightChild;
				switch (currentNode.NodeType)
				{
					case NodeType.Left:
					{
						currentNode.Parent.LeftChild = rightChild;
						rightChild.SetParent(currentNode.Parent,NodeType.Left);
					}
						break;
					case NodeType.Right:
					{
						currentNode.Parent.RightChild = rightChild;
						rightChild.SetParent(currentNode.Parent,NodeType.Right);
					}
						break;
					case NodeType.Root:
					{
						mHeapNode = rightChild;
						rightChild.SetParent(null,NodeType.Root);
					}
						break;
					default:
						break;
				}

				Node minLeftNode = GetMinNode(rightChild);

				if (null != currentNode.LeftChild)
				{
					minLeftNode.LeftChild = currentNode.LeftChild;
					currentNode.LeftChild.SetParent(minLeftNode,NodeType.Left);
				}
				
				return;
			}

			var leftNode = currentNode.LeftChild;
			switch (currentNode.NodeType)
			{
				case NodeType.Left:
					currentNode.Parent.LeftChild = leftNode;
					leftNode.SetParent(currentNode.Parent, NodeType.Left);
					break;
				case NodeType.Right:
					currentNode.Parent.RightChild = leftNode;
					leftNode.SetParent(currentNode.Parent,NodeType.Right);
					break;
				case NodeType.Root:
					mHeapNode = leftNode;
					leftNode.SetParent(null,NodeType.Root);
					break;
				default:
					break;
			}
		}

		public delegate void DataVisitor(T data);
		// 遍历 通过队列实现
		public void Accept(DataVisitor visitor)
		{
			if (null == mHeapNode)
			{
				return;
			}
			
			QStack<Node> stack = new QStack<Node>();
			Node current = mHeapNode;
			while (null != current || !stack.IsEmpty)
			{
				while (null != current)
				{
					stack.Push(current);
					current = current.LeftChild;
				}

				if (!stack.IsEmpty)
				{
					current = stack.Pop();
					visitor(current.Data);

					current = current.RightChild;
				}
			}
		}

		public Iterator<T> Iterator()
		{
			return new BinarySearchTreeIterator(mHeapNode);
		}
		
		public class BinarySearchTreeIterator : Iterator<T>
		{
			private Node mHeadNode;
			private Node mCurrent;
			QStack<Node> mStack = new QStack<Node>();

			public BinarySearchTreeIterator(Node headNode)
			{
				mHeadNode = headNode;
				mCurrent = mHeadNode;
			}

			public bool HasNext
			{
				get
				{
					if (null != mCurrent || !mStack.IsEmpty)
					{
						return true;
					}
					return false;
				}
			}

			public T Next
			{
				get
				{
					while (null != mCurrent)
					{
						mStack.Push(mCurrent);
						;
						mCurrent = mCurrent.LeftChild;
					}

					if (!mStack.IsEmpty)
					{
						mCurrent = mStack.Pop();
						T result = mCurrent.Data;
						mCurrent = mCurrent.RightChild;
						return result;
					}
					return default(T);
				}
			}
		}

		protected Node GetMinNode(Node head)
		{
			Node current = head;
			while (current.LeftChild != null)
			{
				current = current.LeftChild;
			}
			return current;
		}
	}
}