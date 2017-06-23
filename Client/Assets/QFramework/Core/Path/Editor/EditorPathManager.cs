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

namespace PTGame.Framework
{
	using System.Collections.Generic;
	using QFramework;
	using UnityEditor;
	
	public class EditorPathManager
	{
		public const string DEFAULT_PATH_CONFIG_GENERATE_FOLDER = "Assets/QFrameworkData/Path/Config";

		public const string DEFAULT_PATH_SCRIPT_GENERATE_FOLDER = "Assets/QFrameworkData/Path/Script";

		private static Dictionary<string, PathConfig> mCachedPathConfigDict;

		static PathConfig Load(string configName)
		{
			if (null == mCachedPathConfigDict || mCachedPathConfigDict.Count == 0)
			{
				mCachedPathConfigDict = new Dictionary<string, PathConfig>();
			}

			PathConfig retConfig = null;

			mCachedPathConfigDict.TryGetValue(configName, out retConfig);

			if (null == retConfig)
			{
				retConfig = AssetDatabase.LoadAssetAtPath<PathConfig>(DEFAULT_PATH_CONFIG_GENERATE_FOLDER + "/" + configName +
				                                                      ".asset");
				mCachedPathConfigDict.Add(configName,retConfig);
			}

			return retConfig;
		}

		public static PathConfig GetPathConfig<T>() 
		{
			string configName = typeof(T).ToString ();
			return Load (configName);
		}

		public static PathItem GetPathItem<T>(string pathName) 
		{
			string configName = typeof(T).ToString ();
			return Load (configName) [pathName];
		}

		public static string GetPath<T>(string pathName)  
		{
			return GetPathItem<T>(pathName).Path;
		}

		public static string GetAssetPath<T>(string pathName)
		{
			return "Assets/" + GetPathItem<T>(pathName).Path;
		}
	}
}