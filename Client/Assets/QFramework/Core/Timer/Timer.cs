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
    [QMonoSingletonAttribute("[Tools]/Timer")]
    public class Timer : QMonoSingleton<Timer>
    {
        BinaryHeap<TimeItem>        m_UnScaleTimeHeap = new BinaryHeap<TimeItem>(128, BinaryHeapSortMode.kMin);
        BinaryHeap<TimeItem>        m_ScaleTimeHeap = new BinaryHeap<TimeItem>(128, BinaryHeapSortMode.kMin);
        private float               m_CurrentUnScaleTime = -1;
        private float               m_CurrentScaleTime = -1;

        public float currentScaleTime
        {
            get { return m_CurrentScaleTime; }
        }

        public float currentUnScaleTime
        {
            get { return m_CurrentUnScaleTime; }
        }

        public override void OnSingletonInit()
        {
            m_UnScaleTimeHeap.Clear();
            m_ScaleTimeHeap.Clear();

            m_CurrentUnScaleTime = Time.unscaledTime;
            m_CurrentScaleTime = Time.time;
        }

        public void ResetMgr()
        {
            m_UnScaleTimeHeap.Clear();
            m_ScaleTimeHeap.Clear();
        }

        public void StartMgr()
        {
            m_CurrentUnScaleTime = Time.unscaledTime;
            m_CurrentScaleTime = Time.time;
        }

        #region 投递受缩放影响定时器
		public TimeItem Post2Scale(QVoidDelegate.WithGeneric<int> callback, float delay, int repeat)
        {
            TimeItem item = TimeItem.Allocate(callback, delay, repeat);
            Post2Scale(item);
            return item;
        }

		public TimeItem Post2Scale(QVoidDelegate.WithGeneric<int> callback, float delay)
        {
            TimeItem item = TimeItem.Allocate(callback, delay);
            Post2Scale(item);
            return item;
        }

        public void Post2Scale(TimeItem item)
        {
            item.SortScore = m_CurrentScaleTime + item.DelayTime();
            m_ScaleTimeHeap.Insert(item);
        }
        #endregion

        #region 投递真实时间定时器

        //投递指定时间计时器：只支持标准时间
		public TimeItem Post2Really(QVoidDelegate.WithGeneric<int> callback, DateTime toTime)
        {
            float passTick = (toTime.Ticks - DateTime.Now.Ticks) / 10000000;
            if (passTick < 0)
            {
                Log.w("Timer Set Pass Time...");
                passTick = 0;
            }
            return Post2Really(callback, passTick);
        }

		public TimeItem Post2Really(QVoidDelegate.WithGeneric<int> callback, float delay, int repeat)
        {
            TimeItem item = TimeItem.Allocate(callback, delay, repeat);
            Post2Really(item);
            return item;
        }

		public TimeItem Post2Really(QVoidDelegate.WithGeneric<int> callback, float delay)
        {
            TimeItem item = TimeItem.Allocate(callback, delay);
            Post2Really(item);
            return item;
        }

        public void Post2Really(TimeItem item)
        {
            item.SortScore = m_CurrentUnScaleTime + item.DelayTime();
            m_UnScaleTimeHeap.Insert(item);
        }
        #endregion

        public void Update()
        {
            UpdateMgr();
        }

        public void UpdateMgr()
        {
            TimeItem item = null;
            m_CurrentUnScaleTime = Time.unscaledTime;
            m_CurrentScaleTime = Time.time;

            #region 不受缩放影响定时器更新
            while ((item = m_UnScaleTimeHeap.Top()) != null)
            {
                if (!item.isEnable)
                {
                    m_UnScaleTimeHeap.Pop();
                    item.Recycle2Cache();
                    continue;
                }

                if (item.SortScore < m_CurrentUnScaleTime)
                {
                    m_UnScaleTimeHeap.Pop();

                    item.OnTimeTick();

                    if (item.isEnable && item.NeedRepeat())
                    {
                        Post2Really(item);
                    }
                    else
                    {
                        item.Recycle2Cache();
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion

            #region 受缩放影响定时器更新
            while ((item = m_ScaleTimeHeap.Top()) != null)
            {
                if (!item.isEnable)
                {
                    m_ScaleTimeHeap.Pop();
                    item.Recycle2Cache();
                    continue;
                }

                if (item.SortScore < m_CurrentScaleTime)
                {
                    m_ScaleTimeHeap.Pop();

                    item.OnTimeTick();

                    if (item.isEnable && item.NeedRepeat())
                    {
                        Post2Scale(item);
                    }
                    else
                    {
                        item.Recycle2Cache();
                    }
                }
                else
                {
                    break;
                }
            }
            #endregion
        }

        public void Dump()
        {

        }
    }
    
}
