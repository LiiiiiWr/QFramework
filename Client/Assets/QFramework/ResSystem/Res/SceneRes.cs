using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class SceneRes : AssetRes
    {
        public new static SceneRes Allocate(string name)
        {
            SceneRes res = ObjectPool<SceneRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
                res.InitAssetBundleName();
            }
            return res;
        }

        public SceneRes(string assetName) : base(assetName)
        {

        }

        public SceneRes()
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

            if (string.IsNullOrEmpty(AssetBundleName))
            {
                return false;
            }

            AssetBundleRes abR = ResMgr.Instance.GetRes<AssetBundleRes>(AssetBundleName);

            if (abR == null || abR.assetBundle == null)
            {
                Log.e("Failed to Load Asset, Not Find AssetBundleImage:" + abR);
                return false;
            }


            ResState = eResState.kReady;
            return true;
        }

        public override void LoadAsync()
        {
            LoadSync();
        }


        public override void Recycle2Cache()
        {
            ObjectPool<SceneRes>.Instance.Recycle(this);
        }
    }
}
