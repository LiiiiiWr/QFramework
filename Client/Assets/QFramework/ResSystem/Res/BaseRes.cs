using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class BaseRes : AbstractRes
    {
        public BaseRes(string assetName) : base(assetName)
        {
        }

        public BaseRes()
        {

        }

        protected override void OnReleaseRes()
        {
            //如果Image 直接释放了，这里会直接变成NULL
            if (mAsset != null)
            {
                if (mAsset is GameObject)
                {
                    
                }
                else
                {
//					#if UNITY_EDITOR
//					if (SimulateAssetBundleInEditor && !string.Equals(mName,"assetbundlemanifest")) {
//						UnityEngine.Object.DestroyImmediate(mAsset,true);
//					}
//					else
//					#endif
					{
                    //ResMgr.S.timeDebugger.Begin("Unload AssetRes:" + mName);
                    Resources.UnloadAsset(mAsset);
                    //ResMgr.S.timeDebugger.End();
					}
                }

                mAsset = null;
            }
        }
    }
}
