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
	
	// 链表
	public class QLinkedList<T> : IList<T>, Iteratable<T>
	{
		private ListNode<T> mHeadNode;

		public QLinkedList()
		{
			
		}

		protected ListNode<T> HeadNode
		{
			get { return mHeadNode; }
		}
		
		// 获取队尾
		protected ListNode<T> TailNode
		{
			get
			{
				if (null == mHeadNode)
				{
					return null;
				}

				ListNode<T> nextNode = mHeadNode;
				while (null != nextNode.Next)
				{
					nextNode = nextNode.Next;
				}

				return nextNode;
			}
		}

		public void InsertHead(T data)
		{
			var preHead = mHeadNode;
			
			mHeadNode = new ListNode<T>();
			mHeadNode.Data = data;

			mHeadNode.Next = preHead;
		}
		
		// 插入队列尾
		public void InsertTail(T data)
		{
			var preTail = TailNode;
			
			ListNode<T> tail = new ListNode<T>();
			tail.Data = data;

			if (null == preTail)
			{
				mHeadNode = tail;
			}
			else
			{
				preTail.Next = tail;
			}
		}

		public void RemoveHead()
		{
			if (null == mHeadNode)
			{
				return;
			}

			mHeadNode = mHeadNode.Next;
		}

		public bool RemoveAt(int index)
		{
			if (null == mHeadNode)
			{
				return false;
			}

			ListNode<T> preNode = null;
			ListNode<T> currentNode = mHeadNode;

			while (index-- > 0 && null != currentNode)
			{
				preNode = currentNode;
				currentNode = preNode.Next;
			}

			if (currentNode == null)
			{
				return false;
			}

			if (null == preNode)
			{
				mHeadNode = currentNode.Next;
			}
			else
			{
				preNode.Next = currentNode.Next;
			}
			return true;
		}

		public bool Remove(T data)
		{
			if (mHeadNode == null)
			{
				return false;
			}

			ListNode<T> preNode = null;
			ListNode<T> currentNode = mHeadNode;
			bool hasFind = false;

			while (null != currentNode)
			{
				if (currentNode.Data.Equals(data))
				{
					hasFind = true;
					break;
				}

				preNode = currentNode;
				currentNode = currentNode.Next;
			}

			if (!hasFind)
			{
				return false;
			}
			
			// 删除的是头结点
			if (null == preNode)
			{
				mHeadNode.Next = currentNode.Next;
			}
			else
			{
				preNode.Next = currentNode.Next;
			}
			return true;
		}
		
		// 查询方法，返回索引
		public int Query(T data)
		{
			if (null == mHeadNode)
			{
				return -1;
			}

			ListNode<T> currentNode = mHeadNode;
			int index = 0;
			while (null != currentNode)
			{
				if (currentNode.Data.Equals(data))
				{
					return index;
				}
				currentNode = currentNode.Next;
				++index;
			}

			return -1;
		}

		public T HeadData
		{
			get
			{
				if (null == mHeadNode)
				{
					return default(T);
				}
				return mHeadNode.Data;
			}
		}

		public T TailData
		{
			get
			{
				var tailHead = TailNode;
				if (null == tailHead)
				{
					return default(T);
				}
				return tailHead.Data;
			}
		}

		public bool IsEmpty
		{
			get { return null == mHeadNode; }
		}

		public void Accept(IListVisitor<T> visitor)
		{
			var it = Iterator();
			while (it.HasNext)
			{
				visitor.Visit(it.Next);
			}
		}

		public void Accept(ListVisitorDelegate<T> visitor)
		{
			var it = Iterator();
			while (it.HasNext)
			{
				visitor(it.Next);
			}
		}
		
		public class LinkedListIterator : Iterator<T>
		{
			private ListNode<T> mHeadNode;
			private ListNode<T> mCurrentNode;

			public LinkedListIterator(ListNode<T> head)
			{
				mHeadNode = head;
				if (null != mHeadNode)
				{
					mCurrentNode = new ListNode<T>();
					mCurrentNode.Next = mHeadNode;
				}
			}

			public bool HasNext
			{
				get { return mCurrentNode.Next != null; }
			}

			public T Next
			{
				get
				{
					T r = mCurrentNode.Next.Data;
					mCurrentNode = mCurrentNode.Next;
					return r;
				}
			}
		}

		public Iterator<T> Iterator()
		{
			return new LinkedListIterator(mHeadNode);
		}
	}	
}