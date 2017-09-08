using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class AbstractRes : RefCounter, IRes, ICacheAble
    {
		#if UNITY_EDITOR
		static int mSimulateAssetBundleInEditor = -1;
		const string kSimulateAssetBundles = "SimulateAssetBundles"; //此处跟editor中保持统一，不能随意更改

		// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		public static bool SimulateAssetBundleInEditor {
			get {
				if (mSimulateAssetBundleInEditor == -1)
				{
					mSimulateAssetBundleInEditor = UnityEditor.EditorPrefs.GetBool (kSimulateAssetBundles, true) ? 1 : 0;
				}
				return mSimulateAssetBundleInEditor != 0;
			}
			set {
				int newValue = value ? 1 : 0;
				if (newValue != mSimulateAssetBundleInEditor) 
				{
					mSimulateAssetBundleInEditor = newValue;
					UnityEditor.EditorPrefs.SetBool (kSimulateAssetBundles, value);
				}
			}
		}
		#endif


        protected string                    mAssetName;
        protected string                    mOwnerBundleName;
        private short                       mResState = eResState.kWaiting;
        private bool                        mCacheFlag = false;
        protected UnityEngine.Object        mAsset;
        private event Action<bool, IRes>    mResListener;

        public string AssetName
        {
            get { return mAssetName; }
            set { mAssetName = value; }
        }
        

        public short ResState
        {
            get { return mResState; }
            set
            {
                mResState = value;
                if (mResState == eResState.kReady)
                {
                    NotifyResEvent(true);
                }
            }
        }

        public string OwnerBundleName
        {
            get { return mOwnerBundleName; }
            set { mOwnerBundleName = value; }
        }

        public float Progress
        {
            get
            {
                if (mResState == eResState.kLoading)
                {
                    return CalculateProgress();
                }

                if (mResState == eResState.kReady)
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

        public UnityEngine.Object Asset
        {
            get { return mAsset; }
            set { mAsset = value; }
        }

        public virtual object RawAsset
        {
            get { return null; }
        }

        public bool CacheFlag
        {
            get
            {
                return mCacheFlag;
            }

            set
            {
                mCacheFlag = value;
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

            if (mResState == eResState.kReady)
            {
                listener(true, this);
                return;
            }

            mResListener += listener;
        }

        public void UnRegisteResListener(Action<bool, IRes> listener)
        {
            if (listener == null)
            {
                return;
            }

            if (mResListener == null)
            {
                return;
            }

            mResListener -= listener;
        }

        protected void OnResLoadFaild()
        {
            mResState = eResState.kWaiting;
            NotifyResEvent(false);
        }

        private void NotifyResEvent(bool result)
        {
            if (mResListener != null)
            {
                mResListener(result, this);
                mResListener = null;
            }
        }

        protected AbstractRes(string assetName)
        {
            mAssetName = assetName;
        }

        public AbstractRes()
        {

        }

        protected bool CheckLoadAble()
        {
            if (mResState == eResState.kWaiting)
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
                if (res == null || res.ResState != eResState.kReady)
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
            if (mResState == eResState.kLoading)
            {
                return false;
            }

            if (mResState != eResState.kReady)
            {
                return true;
            }

            //Log.i("Release Res:" + mName);

            OnReleaseRes();

            mResState = eResState.kWaiting;
            mResListener = null;
            return true;
        }

        protected virtual void OnReleaseRes()
        {

        }

        protected override void OnZeroRef()
        {
            if (mResState == eResState.kLoading)
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
            mAssetName = null;
            mResListener = null;
        }

        public virtual IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            finishCallback();
            yield break;
        }
        #endregion
    }
}
