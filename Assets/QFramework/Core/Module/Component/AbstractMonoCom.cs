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
    using UnityEngine;
    
    public class AbstractMonoCom : MonoBehaviour, ICom
    {
        private AbstractActor mActor;

        public AbstractActor Actor
        {
            get { return mActor; }
        }

        public virtual int ComOrder
        {
            get { return ComOrderDefine.DEFAULT; }
        }

        public void AwakeCom(AbstractActor actor)
        {
            mActor = actor;

            OnActorBind(actor);

            OnAwakeCom();
        }

        public void OnComDisable()
        {

        }

        public void OnComEnable()
        {

        }

        public virtual void OnComLateUpdate(float dt)
        {

        }

        public virtual void OnComStart()
        {

        }

        public virtual void OnComUpdate(float dt)
        {

        }

        public void DestroyCom()
        {
            OnDestroyCom();
            mActor = null;
            Destroy(this);
        }

#region 子类继承
        protected virtual void OnActorBind(AbstractActor actor)
        {

        }

        protected virtual void OnAwakeCom()
        {

        }
        protected virtual void OnDestroyCom()
        {

        }
#endregion
    }
}
