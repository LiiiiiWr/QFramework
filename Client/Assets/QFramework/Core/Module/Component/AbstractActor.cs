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
    using UnityEngine;

    using System.Collections.Generic;
    using SCFramework;
    
    public class AbstractActor : MonoBehaviour
    {
        [SerializeField]
		private List<string>    mComsNameList = new List<string>();
        private bool            mHasAwake = false;
        private bool            mHasStart = false;
        private List<ICom>      mComponentList = new List<ICom>();
        private QEventSystem     mEventSystem;

#region MonoBehaviour
        private void Awake()
        {
            OnActorAwake();

            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                AwakeCom(mComponentList[i]);
            }

            mHasAwake = true;
        }
        
        private void Start()
        {
            OnActorStart();
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                StartCom(mComponentList[i]);
            }
			mHasStart = true;
        }

        //关于Update的优化方案，可以后续再做
        private void Update()
        {
            UpdateAllComs();
        }

        private void LateUpdate()
        {
            LateUpdateAllComs();
        }

        private void OnDestroy()
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                DestroyCom(mComponentList[i]);
            }

            mComponentList.Clear();
            mComsNameList.Clear();

            OnActorDestroy();
        }
#endregion

#region Public

        public QEventSystem eventSystem
        {
            get
            {
                if (mEventSystem == null)
                {
                    mEventSystem = ObjectPool<QEventSystem>.Instance.Allocate();
                }
                return mEventSystem;
            }
        }

        public void AddCom(ICom com)
        {
            if (com == null)
            {
                return;
            }

            if (GetCom(com.GetType()) != null)
            {
                Log.w("Already Add Component:" + name);
                return;
            }

            //ComWrap wrap = new ComWrap(com);

            mComponentList.Add(com);

            mComsNameList.Add(com.GetType().Name);

            mComponentList.Sort(ComWrapComparison);

            OnAddCom(com);

            if (mHasAwake)
            {
                AwakeCom(com);
            }

            if (mHasStart)
            {
                StartCom(com);
            }
        }

        public T AddCom<T>() where T : ICom, new()
        {
            T com = new T();
            AddCom(com);
            return com;
        }

        public T AddMonoCom<T>() where T : MonoBehaviour, ICom
        {
            T com = gameObject.AddComponent<T>();
            AddCom(com);
            return com;
        }

        public void RemoveCom<T>() where T : ICom
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                if (mComponentList[i] is T)
                {
                    ICom com = mComponentList[i];

                    mComponentList.RemoveAt(i);
                    mComsNameList.RemoveAt(i);
                    OnRemoveCom(com);

                    DestroyCom(com);
                    return;
                }
            }
        }

        public void RemoveCom(ICom com)
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                if (mComponentList[i] == com)
                {
                    mComponentList.RemoveAt(i);
                    mComsNameList.RemoveAt(i);
                    OnRemoveCom(com);

                    DestroyCom(com);
                    return;
                }
            }
        }

        public T GetCom<T>() where T : ICom
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                if (mComponentList[i] is T)
                {
                    return (T)mComponentList[i];
                }
            }

            return default(T);
        }

#endregion

#region Private

        private ICom GetCom(Type t)
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                if (mComponentList[i].GetType() == t)
                {
                    return mComponentList[i];
                }
            }
            return null;
        }

        //这玩意会产生alloac
        protected void ProcessAllCom(Action<ICom> process)
        {
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                process(mComponentList[i]);
            }
        }

        protected void LateUpdateAllComs()
        {
            float dt = Time.deltaTime;
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                mComponentList[i].OnComLateUpdate(dt);
            }
        }

        protected void UpdateAllComs()
        {
            float dt = Time.deltaTime;
            for (int i = mComponentList.Count - 1; i >= 0; --i)
            {
                mComponentList[i].OnComUpdate(dt);
            }
        }

        protected void AwakeCom(ICom wrap)
        {
            wrap.AwakeCom(this);
        }

        protected void StartCom(ICom wrap)
        {
            wrap.OnComStart();
        }

        protected void DestroyCom(ICom wrap)
        {
            wrap.DestroyCom();
        }

        private int ComWrapComparison(ICom a, ICom b)
        {
            return a.ComOrder - b.ComOrder;
        }

        protected virtual void OnActorAwake()
        {

        }

        protected virtual void OnActorStart()
        {

        }

        protected virtual void OnActorDestroy()
        {

        }

        protected virtual void OnAddCom(ICom com)
        {

        }

        protected virtual void OnRemoveCom(ICom com)
        {

        }
#endregion
    }
}
