using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public class ABManifestHandler
    {
        private static AssetBundleManifest mManifest;

        public static AssetBundleManifest Manifest
        {
            get
            {
				return mManifest;
            }

            set
            {
				mManifest = value;
            }
        }


        public static AssetBundleManifest LoadInstance()
        {
            ResLoader loader = ResLoader.Allocate();

			AssetBundleManifest manifest = loader.LoadSync(FrameworkConfigData.ABMANIFEST_ASSET_NAME) as AssetBundleManifest;

            loader.UnloadImage(false);

            return manifest;
        }

        public static string[] GetAllDependenciesByUrl(string url)
        {
			return mManifest.GetAllDependencies(FrameworkConfigData.AssetBundleUrl2Name(url));
        }
    }
}
