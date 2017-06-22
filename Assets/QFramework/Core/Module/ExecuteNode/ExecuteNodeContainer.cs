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
****************************************************************************/

namespace QFramework
{
    using System.Collections.Generic;

    public class ExecuteNodeContainer
    {
        #region Event
        public QVoidDelegate.WithGeneric<float>     OnExecuteScheduleEvent;
        public QVoidDelegate.WithGeneric<string>    OnExecuteTipsEvent;
        public QVoidDelegate.WithVoid               OnExecuteContainerBeginEvent;
        public QVoidDelegate.WithVoid               OnExecuteContainerEndEvent;
        #endregion

        #region 属性&字段
		private List<ExecuteNode>   mNodeList;
		private int                 mCurrentIndex;
		private ExecuteNode         mCurrentNode;

		private float               mTotalSchedule = 0;
        #endregion

        public float TotalSchedule
        {
            get { return mTotalSchedule; }
        }

        public ExecuteNode CurrentNode
        {
            get
            {
                return mCurrentNode;
            }
        }

        public void Append(ExecuteNode item)
        {
            if (mNodeList == null)
            {
                mNodeList = new List<ExecuteNode>();
                mCurrentIndex = -1;
            }

            mNodeList.Add(item);
        }

        public void Start()
        {
            mCurrentIndex = -1;
            MoveToNextUpdateFunc();
        }

        public void Update()
        {
            if (mCurrentNode != null)
            {
                mCurrentNode.OnExecute();

                float schedule = mCurrentNode.Progress;

                mTotalSchedule = mCurrentIndex * (1.0f / mNodeList.Count) + schedule / mNodeList.Count;

                if (OnExecuteScheduleEvent != null)
                {
                    OnExecuteScheduleEvent(mTotalSchedule);
                }

                if (mCurrentNode.Finished)
                {
                    MoveToNextUpdateFunc();
                }
            }
        }

        private void MoveToNextUpdateFunc()
        {
            if (mCurrentNode != null)
            {
                mCurrentNode.OnEnd();
            }

            ++mCurrentIndex;
            if (mCurrentIndex >= mNodeList.Count)
            {
                mTotalSchedule = 1.0f;
                mCurrentNode = null;

                if (OnExecuteContainerEndEvent != null)
                {
                    OnExecuteContainerEndEvent();

                    OnExecuteContainerEndEvent = null;
                }
            }
            else
            {
                mCurrentNode = mNodeList[mCurrentIndex];
                mCurrentNode.OnBegin();

                if (mCurrentIndex == 0)
                {
                    if (OnExecuteContainerBeginEvent != null)
                    {
                        OnExecuteContainerBeginEvent();
                    }
                }

                if (OnExecuteTipsEvent != null)
                {
                    OnExecuteTipsEvent(mCurrentNode.Tips);
                }
            }
        }
    }
}