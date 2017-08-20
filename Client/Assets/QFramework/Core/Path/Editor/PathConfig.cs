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
	using UnityEngine;
	using System.Collections;
	using System.Collections.Generic;

	public enum PATH_ROOT {
		EditorPath,
		ApplicationDataPath,
		ApplicationPersistentDataPath,
		ApplicationStreamingAssetsPath,
		None
	}

	[System.Serializable]
	public class PathItem {
		[Header("描述")]
		[SerializeField] string m_Description = "";
		[SerializeField] PATH_ROOT m_Root = PATH_ROOT.ApplicationDataPath;
		[SerializeField] string m_Name = "";
		[SerializeField] string m_Path = "";
		[SerializeField] bool m_AutoCreateDirectory = false;

		public string Name 
		{
			get { return m_Name; }
		}

		public string Path 
		{
			get { return m_Path; }
		}

		/// <summary>
		/// Editor 时候用的
		/// </summary>
		/// <value>The property get code.</value>
		public string PropertyGetCode 
		{
			get {
				if (string.IsNullOrEmpty (m_Name))
					return null;
				
				var retString = "m_" + m_Name;
				switch (m_Root) {
					case PATH_ROOT.EditorPath:
						retString = "\"" + "Assets/" + "\"" + " + " + retString;
						break;
					case PATH_ROOT.ApplicationDataPath:
						retString = "UnityEngine.Application.dataPath" + " + " + "\"/\"" + " + " + retString;
						break;
					case PATH_ROOT.ApplicationPersistentDataPath:
						retString = "UnityEngine.Application.persistentDataPath" + " + " + "\"/\"" + " + " + retString;
						break;
					case PATH_ROOT.ApplicationStreamingAssetsPath:
						retString = "UnityEngine.Application.streamingAssetsPath" + " + " + "\"/\"" + " + " + retString;
						break;
				}

				if (m_AutoCreateDirectory) 
				{
					retString = "QFramework.Libs.IOUtils.CreateDirIfNotExists (" + retString + ")";
				}

				return retString;
			}
		}

		public string FullPath 
		{
			get 
			{
				switch (m_Root) 
				{
					case PATH_ROOT.EditorPath:
						return "Assets/" + m_Path;		
					case PATH_ROOT.ApplicationDataPath:
						return Application.dataPath + "/" + m_Path;
					case PATH_ROOT.ApplicationPersistentDataPath:
						return PATH_ROOT.ApplicationPersistentDataPath + "/" + m_Path;
					case PATH_ROOT.ApplicationStreamingAssetsPath:
						return PATH_ROOT.ApplicationStreamingAssetsPath + "/" + m_Path;
				}
				return m_Path;
			}
		}

		public string Description 
		{
			get { return m_Description; }
		}

	}

	/// <summary>
	/// Path配置
	/// </summary>
	public class PathConfig : ScriptableObject {
		[Header("注意:每次修改该文件之后，一定要记得按Ctrl/Command + S")]
		[SerializeField]  string m_Description;
		[SerializeField]  List<PathItem> m_PathList;
		[Header("对应的脚本生成的路径")]
		[SerializeField]  string m_ScriptGeneratePath;
		[Header("命名空间(默认QFramework)")]
		[SerializeField]  string m_NameSpace;
		public List<PathItem> List 
		{
			get { return m_PathList; }
		}

		Dictionary<string,PathItem> m_CachedPathDict;

		public string ScriptGeneratePath {
			get { return m_ScriptGeneratePath; }
		}

		public string Description 
		{
			get { return m_Description; }
		}

		public string NameSpace 
		{
			get { return m_NameSpace; }
		}

		/// <summary>
		/// 根据Path做索引
		/// </summary>
		public PathItem this[string pathName] 
		{
			get 
			{
				if (null == m_CachedPathDict) 
				{
					m_CachedPathDict = new Dictionary<string, PathItem> ();
					foreach (var pathItem in m_PathList) 
					{
						if (!string.IsNullOrEmpty (pathItem.Name) && !m_CachedPathDict.ContainsKey (pathItem.Name)) 
						{
							m_CachedPathDict.Add (pathItem.Name, pathItem);
						}
						else 
						{
							Debug.LogError (pathItem.Name + ":" + m_CachedPathDict.ContainsKey (pathItem.Name));
						}
					}
				}

				return m_CachedPathDict[pathName];
			}
		}
	}
}
