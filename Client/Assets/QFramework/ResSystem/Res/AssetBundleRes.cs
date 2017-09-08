using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{

    public class AssetBundleRes : AbstractRes
    {
        private bool        mUnloadFlag = true;
        private string[]    mDependResList;
        private AssetBundleCreateRequest mAssetBundleCreateRequest;

        public static AssetBundleRes Allocate(string name)
        {
			AssetBundleRes res = ObjectPool<AssetBundleRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
                res.InitAssetBundleName();
            }
            return res;
        }

        public AssetBundle assetBundle
        {
            get
            {
                return (AssetBundle)mAsset;
            }

            set
            {
                mAsset = value;
            }
        }

        public AssetBundleRes(string assetName) : base(assetName)
        {

        }

        public AssetBundleRes()
        {

        }

        public override void AcceptLoaderStrategySync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnSyncLoadFinish(loader, this);
        }

        public override void AcceptLoaderStrategyAsync(IResLoader loader, IResLoaderStrategy strategy)
        {
            strategy.OnAsyncLoadFinish(loader, this);
        }

        public override bool LoadSync()
        {
            if (!CheckLoadAble())
            {
                return false;
            }

            ResState = eResState.kLoading;

            //TimeDebugger timer = ResMgr.Instance.timeDebugger;

			string url = FrameworkConfigData.AssetBundleName2Url(mAssetName);

            //timer.Begin("LoadSync AssetBundle:" + mName);
            AssetBundle bundle = AssetBundle.LoadFromFile(url);
            //timer.End();

            mUnloadFlag = true;

            if (bundle == null)
            {
                Log.e("Failed Load AssetBundle:" + mAssetName);
                OnResLoadFaild();
                return false;
            }

            assetBundle = bundle;
            ResState = eResState.kReady;

            //Log.i(string.Format("Load AssetBundle Success.ID:{0}, Name:{1}", bundle.GetInstanceID(), bundle.name));

            //timer.Dump(-1);
            return true;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            ResState = eResState.kLoading;

            ResMgr.Instance.PostIEnumeratorTask(this);
        }

        public override IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            //开启的时候已经结束了
            if (RefCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

			string url = FrameworkConfigData.AssetBundleName2Url(mAssetName);
            AssetBundleCreateRequest abcR = AssetBundle.LoadFromFileAsync(url);

            mAssetBundleCreateRequest = abcR;
            yield return abcR;
            mAssetBundleCreateRequest = null;

            if (!abcR.isDone)
            {
                Log.e("AssetBundleCreateRequest Not Done! Path:" + mAssetName);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            assetBundle = abcR.assetBundle;

            ResState = eResState.kReady;
            finishCallback();
        }

        public override string[] GetDependResList()
        {
            return mDependResList;
        }

        public override bool UnloadImage(bool flag)
        {
            if (assetBundle != null)
            {
                mUnloadFlag = flag;
            }

            return true;
        }
        
        public override void Recycle2Cache()
        {
            ObjectPool<AssetBundleRes>.Instance.Recycle(this);
        }
        
        public override void OnCacheReset()
        {
            base.OnCacheReset();
            mUnloadFlag = true;
            mDependResList = null;
        }

        protected override float CalculateProgress()
        {
            if (mAssetBundleCreateRequest == null)
            {
                return 0;
            }

            return mAssetBundleCreateRequest.progress;
        }

        protected override void OnReleaseRes()
        {
            if (assetBundle != null)
            {
                //ResMgr.Instance.timeDebugger.Begin("Unload AssetBundle:" + mName);
                assetBundle.Unload(mUnloadFlag);
                assetBundle = null;
                //ResMgr.Instance.timeDebugger.End();
            }
        }

        private void InitAssetBundleName()
        {
            mDependResList = AssetDataTable.Instance.GetAllDependenciesByUrl(AssetName);
        }
    }
}
