using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class AssetRes : BaseRes
    {
        protected string[]            mAssetBundleArray;
        protected AssetBundleRequest  mAssetBundleRequest;

        public static AssetRes Allocate(string name,string onwerBundleName = null)
        {
            AssetRes res = ObjectPool<AssetRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
	            res.mOwnerBundleName = onwerBundleName;
                res.InitAssetBundleName();
            }
            return res;
        }

        protected string AssetBundleName
        {
            get
            {
//	            if (null != mOwnerBundleName)
//	            {
//		            return mOwnerBundleName.ToLower();
//	            }
	            
                if (mAssetBundleArray == null)
                {
                    return null;
                }
                return mAssetBundleArray[0];
            }
        }
        public AssetRes(string assetName) : base(assetName)
        {
            
        }

        public AssetRes()
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
                Log.e("Failed to Load Asset, Not Find AssetBundleImage:" + AssetBundleName);
                return false;
            }

            ResState = eResState.kLoading;

            //TimeDebugger timer = ResMgr.S.timeDebugger;

            //timer.Begin("LoadSync Asset:" + mName);


            HoldDependRes();


			UnityEngine.Object obj = null;

			#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor && !string.Equals(mAssetName,"assetbundlemanifest")) {
				string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (abR.AssetName, mAssetName);
				if (assetPaths.Length == 0) {
					Log.e("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					return false;
				}
				obj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (assetPaths [0]);
//				obj = UnityEngine.Object.Instantiate(obj);
//				UnityEditor.AssetDatabase.RemoveUnusedAssetBundleNames();
			}
			else
			#endif
			{	
				obj = abR.assetBundle.LoadAsset (mAssetName);
			}
            //timer.End();

            UnHoldDependRes();

            if (obj == null)
            {
                Log.e("Failed Load Asset:" + mAssetName);
                OnResLoadFaild();
                return false;
            }

            mAsset = obj;

            ResState = eResState.kReady;
            //Log.i(string.Format("Load AssetBundle Success.ID:{0}, Name:{1}", mAsset.GetInstanceID(), mName));

            //timer.Dump(-1);
            return true;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            if (string.IsNullOrEmpty(AssetBundleName))
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

            AssetBundleRes abR = ResMgr.Instance.GetRes<AssetBundleRes>(AssetBundleName);

            if (abR == null || abR.assetBundle == null)
            {
                Log.e("Failed to Load Asset, Not Find AssetBundleImage:" + AssetBundleName);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            //确保加载过程中依赖资源不被释放:目前只有AssetRes需要处理该情况
            HoldDependRes();


			#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor && !string.Equals(mAssetName,"assetbundlemanifest")) {
				string[] assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (abR.AssetName, mAssetName);
				if (assetPaths.Length == 0) {
					Log.e("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					finishCallback();
					yield break;
				}
				mAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object> (assetPaths [0]);
			}
			else
			#endif
			{	
				AssetBundleRequest abQ = abR.assetBundle.LoadAssetAsync(mAssetName);
				mAssetBundleRequest = abQ;

				yield return abQ;

				mAssetBundleRequest = null;

				UnHoldDependRes();

				if (!abQ.isDone)
				{
					Log.e("Failed Load Asset:" + mAssetName);
					OnResLoadFaild();
					finishCallback();
					yield break;
				}

				mAsset = abQ.asset;
			}

            ResState = eResState.kReady;

            finishCallback();
        }

        public override string[] GetDependResList()
        {
            return mAssetBundleArray;
        }

        public override void OnCacheReset()
        {
            mAssetBundleArray = null;
        }
        
        public override void Recycle2Cache()
        {
            ObjectPool<AssetRes>.Instance.Recycle(this);
        }

        protected override float CalculateProgress()
        {
            if (mAssetBundleRequest == null)
            {
                return 0;
            }

            return mAssetBundleRequest.progress;
        }

		protected void InitAssetBundleName()
		{
			mAssetBundleArray = null;

			AssetData config = mOwnerBundleName != null
				? AssetDataTable.Instance.GetAssetData(mAssetName, mOwnerBundleName)
				: AssetDataTable.Instance.GetAssetData(mAssetName);

			if (config == null)
			{
				Log.e("Not Find AssetData For Asset:" + mAssetName);
				return;
			}

			string assetBundleName = AssetDataTable.Instance.GetAssetBundleName(config.AssetName, config.AssetBundleIndex,mOwnerBundleName);

			if (string.IsNullOrEmpty(assetBundleName))
			{
				Log.e("Not Find AssetBundle In Config:" + config.AssetBundleIndex);
				return;
			}
			mAssetBundleArray = new string[1];
			mAssetBundleArray[0] = assetBundleName;
		}
    }
}
