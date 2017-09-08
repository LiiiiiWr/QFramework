using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class ResFactory
    {
        private delegate IRes ResCreator(string name);

        static ResFactory()
        {
            Log.i("Init[ResFactory]");
            ObjectPool<AssetBundleRes>.Instance.MaxCacheCount = 20;
            ObjectPool<AssetRes>.Instance.MaxCacheCount = 40;
            ObjectPool<InternalRes>.Instance.MaxCacheCount = 40;
            ObjectPool<NetImageRes>.Instance.MaxCacheCount = 20;
        }
        
        public static IRes Create(string assetName,string ownerBundleName)
        {
            short assetType = 0;
            if (assetName.StartsWith("Resources/"))
            {
                assetType = eResType.kInternal;
            }
            else if (assetName.StartsWith("NetImage:"))
            {
                assetType = eResType.kNetImageRes;
            }
            else
            {
                AssetData data = AssetDataTable.Instance.GetAssetData(assetName,ownerBundleName);
                if (data == null)
                {
                    Log.e("Failed to Create Res. Not Find AssetData:" + ownerBundleName + assetName );
                    return null;
                }
                else
                {
                    assetType = data.AssetType;
                }
            }

            return Create(assetName,ownerBundleName,assetType);
        }
        
        public static IRes Create(string assetName,string ownerBundleName, short assetType)
        {
            switch (assetType)
            {
                case eResType.kAssetBundle:
                    return AssetBundleRes.Allocate(assetName);
                case eResType.kABAsset:
                    return AssetRes.Allocate(assetName,ownerBundleName);
                case eResType.kABScene:
                    return SceneRes.Allocate(assetName);
                case eResType.kInternal:
                    return InternalRes.Allocate(assetName);
                case eResType.kNetImageRes:
                    return NetImageRes.Allocate(assetName);
                default:
                    Log.e("Invalid assetType :" + assetType);
                    return null;
            }
        }
        
        

        public static IRes Create(string assetName)
        {
            short assetType = 0;
            if (assetName.StartsWith("Resources/"))
            {
                assetType = eResType.kInternal;
            }
            else if (assetName.StartsWith("NetImage:"))
            {
                assetType = eResType.kNetImageRes;
            }
            else
            {
                AssetData data = AssetDataTable.Instance.GetAssetData(assetName);
                if (data == null)
                {
                    Log.e("Failed to Create Res. Not Find AssetData:" + assetName);
                    return null;
                }
                else
                {
                    assetType = data.AssetType;
                }
            }

            return Create(assetName, assetType);
        }

        public static IRes Create(string assetName, short assetType)
        {
            switch (assetType)
            {
                case eResType.kAssetBundle:
                    return AssetBundleRes.Allocate(assetName);
                case eResType.kABAsset:
                    return AssetRes.Allocate(assetName);
                case eResType.kABScene:
                    return SceneRes.Allocate(assetName);
                case eResType.kInternal:
                    return InternalRes.Allocate(assetName);
                case eResType.kNetImageRes:
                    return NetImageRes.Allocate(assetName);
                default:
                    Log.e("Invalid assetType :" + assetType);
                    return null;
            }
        }
    }
}
