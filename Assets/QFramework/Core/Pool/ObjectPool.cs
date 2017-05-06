using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QFramework;

namespace QFramework
{
    //主动提供缓存管理的类型
    public interface ICacheType
    {
        void Recycle2Cache();
    }

    public interface ICacheAble
    {
        void OnCacheReset();
        bool CacheFlag
        {
            get;
            set;
        }
    }

    public interface CountObserverAble
    {
        int currentCount
        {
            get;
        }
    }

    public class ObjectPool<T> : QSingleton<ObjectPool<T>>, CountObserverAble where T : ICacheAble, new()
    {
		private int         mMaxCount = 0;
		private Stack<T>    mCacheStack;

		private ObjectPool() {}

        public void Init(int maxCount, int initCount)
        {
            if (maxCount > 0)
            {
                initCount = Mathf.Min(maxCount, initCount);
            }

            if (currentCount < initCount)
            {
                for (int i = currentCount; i < initCount; ++i)
                {
                    Recycle(new T());
                }
            }
        }

        public int currentCount
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

        public int maxCacheCount
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

        public void Recycle(T t)
        {
            if (t == null || t.CacheFlag)
            {
                return;
            }

            if (mCacheStack == null)
            {
                mCacheStack = new System.Collections.Generic.Stack<T>();
            }
            else if (mMaxCount > 0)
            {
                if (mCacheStack.Count >= mMaxCount)
                {
                    t.OnCacheReset();
                    return;
                }
            }

            t.CacheFlag = true;
            t.OnCacheReset();
            mCacheStack.Push(t);
        }
    }
}
