/****************************************************************************
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
 * 
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;


namespace QFramework
{
    public class TimeItem : IBinaryHeapElement, ICacheAble, ICacheType
    {
        /*
         * tick:当前第几次
         * 
         */

        private float                   mDelayTime;
        private bool                    mIsEnable = true;
        private int                     mRepeatCount;
        private float                   mSortScore;
		private QVoidDelegate.WithGeneric<int>                mCallback;
        private int                     mCallbackTick;
        private int                     mHeapIndex;
        private bool                    mIsCache;

		public static TimeItem Allocate(QVoidDelegate.WithGeneric<int> callback, float delayTime, int repeatCount = 1)
        {
            TimeItem item = ObjectPool<TimeItem>.Instance.Allocate();
            item.Set(callback, delayTime, repeatCount);
            return item;
        }

		public void Set(QVoidDelegate.WithGeneric<int> callback, float delayTime, int repeatCount)
        {
            mCallbackTick = 0;
            mCallback = callback;
            mDelayTime = delayTime;
            mRepeatCount = repeatCount;
        }

        public void OnTimeTick()
        {
            if (mCallback != null)
            {
                mCallback(++mCallbackTick);
            }

            if (mRepeatCount > 0)
            {
                --mRepeatCount;
            }
        }

		public QVoidDelegate.WithGeneric<int> callback
        {
            get { return mCallback; }
        }

        public float SortScore
        {
            get { return mSortScore; }
            set { mSortScore = value; }
        }

        public int HeapIndex
        {
            get { return mHeapIndex; }
            set { mHeapIndex = value; }
        }

        public bool isEnable
        {
            get { return mIsEnable; }
        }

        public bool CacheFlag
        {
            get
            {
                return mIsCache;
            }

            set
            {
                mIsCache = value;
            }
        }

        public void Cancel()
        {
            if (mIsEnable)
            {
                mIsEnable = false;
                mCallback = null;
            }
        }

        public bool NeedRepeat()
        {
            if (mRepeatCount == 0)
            {
                return false;
            }
            return true;
        }

        public float DelayTime()
        {
            return mDelayTime;
        }

        public void RebuildHeap<T>(BinaryHeap<T> heap) where T : IBinaryHeapElement
        {
            heap.RebuildAtIndex(mHeapIndex);
        }

        public void OnCacheReset()
        {
            mCallbackTick = 0;
            mCallback = null;
            mIsEnable = true;
            mHeapIndex = 0;
    }

        public void Recycle2Cache()
        {
            //超出缓存最大值
			ObjectPool<TimeItem>.Instance.Recycle(this);
        }
    }
}
