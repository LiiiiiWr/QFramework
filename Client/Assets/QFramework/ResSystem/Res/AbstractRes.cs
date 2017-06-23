﻿/****************************************************************************
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

namespace QFramework
{
    public class AbstractRes : RefCounter, IRes, ICacheAble
    {
        protected string                    m_Name;
        private short                       m_ResState = eResState.kWaiting;
        private bool                        m_CacheFlag = false;
        protected UnityEngine.Object        m_Asset;
        private event Action<bool, IRes>    m_ResListener;

        public string name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public short resState
        {
            get { return m_ResState; }
            set
            {
                m_ResState = value;
                if (m_ResState == eResState.kReady)
                {
                    NotifyResEvent(true);
                }
            }
        }

        public float progress
        {
            get
            {
                if (m_ResState == eResState.kLoading)
                {
                    return CalculateProgress();
                }

                if (m_ResState == eResState.kReady)
                {
                    return 1;
                }

                return 0;
            }
        }

        protected virtual float CalculateProgress()
        {
            return 0;
        }

        public UnityEngine.Object asset
        {
            get { return m_Asset; }
            set { m_Asset = value; }
        }

        public virtual object rawAsset
        {
            get { return null; }
        }

        public bool CacheFlag
        {
            get
            {
                return m_CacheFlag;
            }

            set
            {
                m_CacheFlag = value;
            }
        }

        public virtual void AcceptLoaderStrategySync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnSyncLoadFinish(loader, this);
        }

        public virtual void AcceptLoaderStrategyAsync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnAsyncLoadFinish(loader, this);
        }

        public void RegisteResListener(Action<bool, IRes> listener)
        {
            if (listener == null)
            {
                return;
            }

            if (m_ResState == eResState.kReady)
            {
                listener(true, this);
                return;
            }

            m_ResListener += listener;
        }

        public void UnRegisteResListener(Action<bool, IRes> listener)
        {
            if (listener == null)
            {
                return;
            }

            if (m_ResListener == null)
            {
                return;
            }

            m_ResListener -= listener;
        }

        protected void OnResLoadFaild()
        {
            m_ResState = eResState.kWaiting;
            NotifyResEvent(false);
        }

        private void NotifyResEvent(bool result)
        {
            if (m_ResListener != null)
            {
                m_ResListener(result, this);
                m_ResListener = null;
            }
        }

        protected AbstractRes(string name)
        {
            m_Name = name;
        }

        public AbstractRes()
        {

        }

        protected bool CheckLoadAble()
        {
            if (m_ResState == eResState.kWaiting)
            {
                return true;
            }

            return false;
        }

        protected void HoldDependRes()
        {
            string[] depends = GetDependResList();
            if (depends == null || depends.Length == 0)
            {
                return;
            }

            for (int i = depends.Length - 1; i >= 0; --i)
            {
                var res = ResMgr.Instance.GetRes(depends[i], false);
                if (res != null)
                {
                    res.AddRef();
                }
            }
        }

        protected void UnHoldDependRes()
        {
            string[] depends = GetDependResList();
            if (depends == null || depends.Length == 0)
            {
                return;
            }

            for (int i = depends.Length - 1; i >= 0; --i)
            {
                var res = ResMgr.Instance.GetRes(depends[i], false);
                if (res != null)
                {
                    res.SubRef();
                }
            }
        }

        #region 子类实现
        public virtual bool LoadSync()
        {
            return false;
        }

        public virtual void LoadAsync()
        {
        }

        public virtual string[] GetDependResList()
        {
            return null;
        }

        public bool IsDependResLoadFinish()
        {
            string[] depends = GetDependResList();
            if (depends == null || depends.Length == 0)
            {
                return true;
            }

            for (int i = depends.Length - 1; i >= 0; --i)
            {
                var res = ResMgr.Instance.GetRes(depends[i], false);
                if (res == null || res.resState != eResState.kReady)
                {
                    return false;
                }
            }

            return true;
        }

        public virtual bool UnloadImage(bool flag)
        {
            return false;
        }

        public bool ReleaseRes()
        {
            if (m_ResState == eResState.kLoading)
            {
                return false;
            }

            if (m_ResState != eResState.kReady)
            {
                return true;
            }

            //Log.i("Release Res:" + m_Name);

            OnReleaseRes();

            m_ResState = eResState.kWaiting;
            m_ResListener = null;
            return true;
        }

        protected virtual void OnReleaseRes()
        {

        }

        protected override void OnZeroRef()
        {
            if (m_ResState == eResState.kLoading)
            {
                return;
            }

            ReleaseRes();
        }

        public virtual void Recycle2Cache()
        {
            
        }

        public virtual void OnCacheReset()
        {
            m_Name = null;
            m_ResListener = null;
        }

        public virtual IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            finishCallback();
            yield break;
        }
        #endregion
    }
}
