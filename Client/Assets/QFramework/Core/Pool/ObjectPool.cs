﻿/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
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
    using UnityEngine;
    using System.Collections.Generic;
    
    /// <summary>
    /// I cache type.
    /// </summary>
    public interface ICacheType
    {
        void Recycle2Cache();
    }

	/// <summary>
	/// I cache able.
	/// </summary>
    public interface ICacheAble
    {
        void OnCacheReset();
        bool CacheFlag
        {
            get;
            set;
        }
    }

	/// <summary>
	/// Count observer able.
	/// </summary>
    public interface CountObserverAble
    {
        int CurCount
        {
            get;
        }
    }

	/// <summary>
	/// Object pool.
	/// </summary>
	public class ObjectPool<T> : QSingleton<ObjectPool<T>>, CountObserverAble where T : ICacheAble,new()
    {
		private int         mMaxCount = 0;
		private Stack<T>    mCacheStack;

		private ObjectPool() {}

		/// <summary>
		/// Init the specified maxCount and initCount.
		/// </summary>
		/// <param name="maxCount">Max Cache count.</param>
		/// <param name="initCount">Init Cache count.</param>
        public void Init(int maxCount, int initCount)
        {
            if (maxCount > 0)
            {
                initCount = Mathf.Min(maxCount, initCount);
            }

			if (CurCount < initCount)
            {
				for (int i = CurCount; i < initCount; ++i)
                {
					Recycle(new T());
                }
            }
        }

		/// <summary>
		/// Gets the current count.
		/// </summary>
		/// <value>The current count.</value>
        public int CurCount
        {
            get
            {
                if (mCacheStack == null)
                {
                    return 0;
                }

                return mCacheStack.Count;
            }
        }

		/// <summary>
		/// Gets or sets the max cache count.
		/// </summary>
		/// <value>The max cache count.</value>
        public int MaxCacheCount
        {
            get { return mMaxCount; }
            set
            {
                mMaxCount = value;

                if (mCacheStack != null)
                {
                    if (mMaxCount > 0)
                    {
                        if (mMaxCount < mCacheStack.Count)
                        {
                            int removeCount = mMaxCount - mCacheStack.Count;
                            while (removeCount > 0)
                            {
                                mCacheStack.Pop();
                                --removeCount;
                            }
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Allocate T instance.
		/// </summary>
        public T Allocate()
        {
            T result;
            if (mCacheStack == null || mCacheStack.Count == 0)
            {
                result = new T();
            }
            else
            {
                result = mCacheStack.Pop();
            }

            result.CacheFlag = false;
            return result;
        }

		/// <summary>
		/// Recycle the T instance
		/// </summary>
		/// <param name="t">T.</param>
		public bool Recycle(T t)
        {
            if (t == null || t.CacheFlag)
            {
				return false;
            }

            if (mCacheStack == null)
            {
                mCacheStack = new Stack<T>();
            }
            else if (mMaxCount > 0)
            {
                if (mCacheStack.Count >= mMaxCount)
                {
                    t.OnCacheReset();
					return false;
                }
            }

            t.CacheFlag = true;
            t.OnCacheReset();
            mCacheStack.Push(t);

			return true;
        }
    }
}