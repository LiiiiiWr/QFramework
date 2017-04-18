using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QFramework.Libs;

namespace QFramework
{
    public class ProjectPathConfigTemp
    {
        #region 工程目录
        public const string APP_CONFIG_PATH = "Resources/Config/AppConfig";
        #endregion

        #region UIRoot
        public const string UI_ROOT_PATH = "Resources/UIManager";
        #endregion

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
		#endregion
    }
}
