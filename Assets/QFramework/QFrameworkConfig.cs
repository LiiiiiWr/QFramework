using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QFramework.Libs;

namespace QFramework
{
    public class QFrameworkConfig
    {

		#region AssetBundle 相关
		public const string ABMANIFEST_AB_NAME = "qframework";
		public static string AB_RELATIVE_PATH 
		{
			get {
				return "AssetBundles/"+ PlatformUtils.GetPlatformName() + "/qframework/";
			}
		}

		public const string ABMANIFEST_ASSET_NAME = "assetbundlemanifest";

		public static string AssetBundleUrl2Name(string url)
		{
			string parren = FilePath.streamingAssetsPath + AB_RELATIVE_PATH;
			return url.Replace(parren, "");
		}

		public static string AssetBundleName2Url(string name)
		{
			string parren = FilePath.streamingAssetsPath + AB_RELATIVE_PATH;
			return parren + name;
		}

		public const string EXPORT_ASSETBUNDLE_CONFIG_PATH = "asset_bindle_config.bin";

		public static string EXPORT_ROOT_FOLDER {
			get {
				return "Assets/StreamingAssets/AssetBundles/" + PlatformUtils.GetPlatformName() + "/qframework/";
			}
		}

		#endregion
    }
}
