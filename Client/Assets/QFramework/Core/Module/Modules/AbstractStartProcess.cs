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
    using System;
    using SCFramework;
    
    public class AbstractStartProcess : AbstractMonoModule
    {
        private ExecuteNodeContainer    mExecuteContainer;
        private Action                  mOnProcessFinish;

        public void SetFinishListener(Action listener)
        {
            mOnProcessFinish = listener;
        }

        public ExecuteNodeContainer executeContainer
        {
            get
            {
                return mExecuteContainer;
            }
        }

        public float totalSchedule
        {
            get
            {
                if (mExecuteContainer == null)
                {
                    return 0;
                }

                return mExecuteContainer.TotalSchedule;
            }
        }

        public void Append(ExecuteNode node)
        {
            if (node == null)
            {
                return;
            }

            if (mExecuteContainer == null)
            {
                mExecuteContainer = new ExecuteNodeContainer();
            }
            mExecuteContainer.Append(node);
        }

        protected override void OnAwakeCom()
        {
            InitExecuteContainer();
        }

        public override void OnComStart()
        {
            if (mExecuteContainer == null)
            {
                return;
            }

            mExecuteContainer.OnExecuteContainerEndEvent += OnAllExecuteNodeEnd;
            mExecuteContainer.Start();
        }

        public override void OnComUpdate(float dt)
        {
            if (mExecuteContainer == null)
            {
                return;
            }

            mExecuteContainer.Update();
        }

        protected virtual void InitExecuteContainer()
        {

        }

        protected virtual void OnAllExecuteNodeEnd()
        {
            Log.i("#BaseStartProcess: OnAllExecuteNodeEnd");
            mExecuteContainer.OnExecuteContainerEndEvent -= OnAllExecuteNodeEnd;

            if (mOnProcessFinish != null)
            {
                mOnProcessFinish();
            }

            mExecuteContainer = null;

            Actor.RemoveCom(this);
        }
    }
}
