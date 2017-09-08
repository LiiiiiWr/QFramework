using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class InternalRes : BaseRes
    {
        private ResourceRequest mResourceRequest;

        public static InternalRes Allocate(string name)
        {
            InternalRes res = ObjectPool<InternalRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
            }
            return res;
        }

        public static string Name2Path(string name)
        {
            return name.Substring(10);
        }

        public InternalRes(string assetName) : base(assetName)
        {

        }

        public InternalRes()
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

            if (string.IsNullOrEmpty(mAssetName))
            {
                return false;
            }

            ResState = eResState.kLoading;

            //TimeDebugger timer = ResMgr.Instance.timeDebugger;

            //timer.Begin("Resources.Load:" + mName);
            mAsset = Resources.Load(Name2Path(mAssetName));
            //timer.End();

            if (mAsset == null)
            {
                Log.e("Failed to Load Asset From Resources:" + Name2Path(mAssetName));
                OnResLoadFaild();
                return false;
            }

            ResState = eResState.kReady;
            return true;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            if (string.IsNullOrEmpty(mAssetName))
            {
                return;
            }

            ResState = eResState.kLoading;

            ResMgr.Instance.PostIEnumeratorTask(this);
        }

        public override IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            if (RefCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            ResourceRequest rQ = Resources.LoadAsync(Name2Path(mAssetName));

            mResourceRequest = rQ;
            yield return rQ;
            mResourceRequest = null;

            if (!rQ.isDone)
            {
                Log.e("Failed to Load Resources:" + mAssetName);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            mAsset = rQ.asset;

            ResState = eResState.kReady;

            finishCallback();
        }

        public override void Recycle2Cache()
        {
            ObjectPool<InternalRes>.Instance.Recycle(this);
        }

        protected override float CalculateProgress()
        {
            if (mResourceRequest == null)
            {
                return 0;
            }

            return mResourceRequest.progress;
        }
    }
}
