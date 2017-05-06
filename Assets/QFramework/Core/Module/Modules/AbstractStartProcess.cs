/****************************************************************************
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

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SCFramework;


namespace QFramework
{
    public class AbstractStartProcess : AbstractMonoModule
    {
        private ExecuteNodeContainer    m_ExecuteContainer;
        private Action                  m_OnProcessFinish;

        public void SetFinishListener(Action listener)
        {
            m_OnProcessFinish = listener;
        }

        public ExecuteNodeContainer executeContainer
        {
            get
            {
                return m_ExecuteContainer;
            }
        }

        public float totalSchedule
        {
            get
            {
                if (m_ExecuteContainer == null)
                {
                    return 0;
                }

                return m_ExecuteContainer.totalSchedule;
            }
        }

        public void Append(ExecuteNode node)
        {
            if (node == null)
            {
                return;
            }

            if (m_ExecuteContainer == null)
            {
                m_ExecuteContainer = new ExecuteNodeContainer();
            }
            m_ExecuteContainer.Append(node);
        }

        protected override void OnAwakeCom()
        {
            InitExecuteContainer();
        }

        public override void OnComStart()
        {
            if (m_ExecuteContainer == null)
            {
                return;
            }

            m_ExecuteContainer.On_ExecuteContainerEndEvent += OnAllExecuteNodeEnd;
            m_ExecuteContainer.Start();
        }

        public override void OnComUpdate(float dt)
        {
            if (m_ExecuteContainer == null)
            {
                return;
            }

            m_ExecuteContainer.Update();
        }

        protected virtual void InitExecuteContainer()
        {

        }

        protected virtual void OnAllExecuteNodeEnd()
        {
            Log.i("#BaseStartProcess: OnAllExecuteNodeEnd");
            m_ExecuteContainer.On_ExecuteContainerEndEvent -= OnAllExecuteNodeEnd;

            if (m_OnProcessFinish != null)
            {
                m_OnProcessFinish();
            }

            m_ExecuteContainer = null;

            actor.RemoveCom(this);
        }
    }
}
