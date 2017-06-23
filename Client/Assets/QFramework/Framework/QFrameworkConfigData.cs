/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
****************************************************************************/

namespace QFramework
{
	using Libs;
	using UnityEngine;
	#if UNITY_EDITOR
	using UnityEditor;
	#endif
	using System.IO;
	
	[System.Serializable]
    public class QFrameworkConfigData
	{
		public static string[] LUA_SUPPORT_TEXTS = {"NoneLua", "uLua", "sLua", "xLua"};
		public static string[] LUA_SUPPORT_SYMBOLS = {"NONE_LUA_SUPPORT", "ULUA_SUPPORT", "SLUA_SUPPORT", "XLUA_SUPPORT"};

		private static string mConfigSavedDir = Application.dataPath + "/QFrameworkData/ProjectConfig/";
		private static string mConfigSavedFileName = "ProjectConfig.json";

		public string Namespace = null;
		public int LuaSupportIndex = 0;

		public string LuaSupportSymbol
		{
			get { return LUA_SUPPORT_SYMBOLS[LuaSupportIndex]; }
		}

		public string UIScriptDir = "/Scripts/UI";

		public string UIScriptDirFullPath
		{
			get { return Application.dataPath + UIScriptDir; }
		}

		public string UIPrefabDir = "/QArt/AssetBundles/UIPrefab";

		public string UIPrefabDirFullPath
		{
			get { return Application.dataPath + UIPrefabDir; }
		}

		public string UIFactoryFileDir = "/QFrameworkData/Framework/Script";

		public string UIFactoryFileFullPath
		{
			get { return string.Format("{0}/{1}.cs", Application.dataPath + UIFactoryFileDir, "UIFactory"); }
		}

		public static QFrameworkConfigData Load()
		{
			IOUtils.CreateDirIfNotExists(mConfigSavedDir);

			if (!File.Exists(mConfigSavedDir + mConfigSavedFileName))
			{
				using (var fileStream = File.Create(mConfigSavedDir + mConfigSavedFileName))
				{
					fileStream.Close();
				}
			}

			var frameworkConfigData = SerializeHelper.LoadJson<QFrameworkConfigData>(mConfigSavedDir + mConfigSavedFileName);

			if (frameworkConfigData == null || string.IsNullOrEmpty(frameworkConfigData.Namespace))
			{
				frameworkConfigData = new QFrameworkConfigData();
				frameworkConfigData.Namespace = "Company.ProjectName";
			}

			return frameworkConfigData;
		}

		public void Save()
		{
			this.SaveJson(mConfigSavedDir + mConfigSavedFileName);
			#if UNITY_EDITOR
			AssetDatabase.Refresh();
			#endif
		}
	
		#region AssetBundle 相关
		public const string ABMANIFEST_AB_NAME = "qframework";
		public const string ABMANIFEST_ASSET_NAME = "assetbundlemanifest";
		public static string AB_RELATIVE_PATH
		{
			get {
				return "AssetBundles/"+ PlatformUtils.GetPlatformName() + "/qframework/";
			}
		}

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