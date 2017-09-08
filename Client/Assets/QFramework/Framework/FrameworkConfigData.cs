/****************************************************************************
 * Copyright (c) 2017 imagicbell
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
 * 
 ****************************************************************************/

namespace QFramework
{
	using Libs;
	using UnityEngine;
	using System.IO;
#if UNITY_EDITOR
	using UnityEditor;
#endif
	using UnityEngine;

	[System.Serializable]
	public class FrameworkConfigData
	{
		public static string[] RES_LOADER_SUPPORT_TEXTS = {"QABManager", "QResSystem"};
		public static string[] RES_LOADER_SUPPORT_SYMBOLS = {"QABMANAGER_SUPPORT", "QRESSYSTEM_SUPPORT"};
		public static string[] LUA_SUPPORT_TEXTS = {"NoneLua", "uLua", "sLua", "xLua"};
		public static string[] LUA_SUPPORT_SYMBOLS = {"NONE_LUA_SUPPORT", "ULUA_SUPPORT", "SLUA_SUPPORT", "XLUA_SUPPORT"};
		public static string[] COCOS_SUPPORT_TEXTS = {"NoneCocos", "Cocos"};
		public static string[] COCOS_SUPPORT_SYMBOLS = {"NONE_COCOS_SUPPORT", "COCOS_SUPPORT"};

		static string mConfigSavedDir = Application.dataPath + "/QGameData/ProjectConfig/";
		static string mConfigSavedFileName = "ProjectConfig.json";

		public string Namespace = null;
		public int ResLoaderSupportIndex = 0;
		public int LuaSupportIndex = 0;
		public int CocosSupportIndex = 0;

		public string ResLoaderSupportSymbol
		{
			get { return RES_LOADER_SUPPORT_SYMBOLS[ResLoaderSupportIndex]; }
		}

		public string LuaSupportSymbol
		{
			get { return LUA_SUPPORT_SYMBOLS[LuaSupportIndex]; }
		}

		public string CocosSupportSymbol
		{
			get { return COCOS_SUPPORT_SYMBOLS[CocosSupportIndex]; }
		}

		public string UIScriptDir = "/Scripts/UI";

		public string UIScriptDirFullPath
		{
			get { return Application.dataPath + UIScriptDir; }
		}

		public string UIPrefabDir = "/ArtRes/AssetBundles/UIPrefab";

		public string UIPrefabDirFullPath
		{
			get { return Application.dataPath + UIPrefabDir; }
		}

		public string UIFactoryFileDir = "/QGameData/Framework/Script";

		public string UIFactoryFileFullPath
		{
			get { return string.Format("{0}/{1}.cs", Application.dataPath + UIFactoryFileDir, "QUIFactory"); }
		}

		public static FrameworkConfigData Load()
		{
			IOUtils.CreateDirIfNotExists(mConfigSavedDir);

			if (!File.Exists(mConfigSavedDir + mConfigSavedFileName))
			{
				using (var fileStream = File.Create(mConfigSavedDir + mConfigSavedFileName))
				{
					fileStream.Close();
				}
			}

			var frameworkConfigData = SerializeHelper.LoadJson<FrameworkConfigData>(mConfigSavedDir + mConfigSavedFileName);

			if (frameworkConfigData == null || string.IsNullOrEmpty(frameworkConfigData.Namespace))
			{
				frameworkConfigData = new FrameworkConfigData();
				frameworkConfigData.Namespace = "Putao.ProjectName";
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

		public const string ABMANIFEST_AB_NAME = "putaogame";
		public const string ABMANIFEST_ASSET_NAME = "assetbundlemanifest";

		public static string AB_RELATIVE_PATH
		{
			get { return "AssetBundles/" + PlatformUtil.GetPlatformName() + "/putaogame/"; }
		}

		public static string AssetBundleUrl2Name(string url)
		{
			string retName = null;
			string parren = FilePath.streamingAssetsPath + "AssetBundles/" + PlatformUtil.GetPlatformName() + "/" +
			                ABMANIFEST_AB_NAME + "/";
			retName = url.Replace(parren, "");

			parren = FilePath.persistentDataPath + "AssetBundles/" + PlatformUtil.GetPlatformName() + "/" +
			         ABMANIFEST_AB_NAME + "/";
			retName = retName.Replace(parren, "");
			return retName;
		}

		public static string AssetBundleName2Url(string name)
		{
			string retUrl = FilePath.persistentDataPath + "AssetBundles/" + PlatformUtil.GetPlatformName() + "/" +
			                ABMANIFEST_AB_NAME + "/" + name;
			
			if (File.Exists(retUrl))
			{
				return retUrl;
			}
			
			return FilePath.streamingAssetsPath + "AssetBundles/" + PlatformUtil.GetPlatformName() + "/" +
			       ABMANIFEST_AB_NAME + "/" + name;
		}

		//导出目录
		public static string EDITOR_AB_EXPORT_ROOT_FOLDER
		{
			get { return "StreamingAssets/AssetBundles/" + RELATIVE_AB_ROOT_FOLDER; }
		}

		/// <summary>
		/// AssetBundle存放路径
		/// </summary>
		public static string RELATIVE_AB_ROOT_FOLDER
		{
			get { return "/AssetBundles/" + PlatformUtil.GetPlatformName() + "/putaogame/"; }
		}

		/// <summary>
		/// AssetBundle 配置路径
		/// </summary>
		public static string EXPORT_ASSETBUNDLE_CONFIG_FILENAME
		{
			get { return "asset_bindle_config.bin"; }
		}

		#endregion
	}
}