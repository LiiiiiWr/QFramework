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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework {

	/// <summary>
	/// 几种常见的根目录
	/// </summary>
	public enum PathRoot {
		EditorPath,
		ApplicationDataPath,
		ApplicationPersistentDataPath,
		ApplicationStreamingAssetsPath,
		None
	}

	/// <summary>
	/// 每一个路径
	/// </summary>
	[System.Serializable]
	public class PathItem {
		[Header("描述")]
		[SerializeField] string m_Description = "";
		[SerializeField] PathRoot m_Root = PathRoot.ApplicationDataPath;
		[SerializeField] string m_Name = "";
		[SerializeField] string m_Path = "";
		[SerializeField] bool m_AutoCreateDirectory = false;

		/// <summary>
		/// 路径的名字
		/// </summary>
		public string Name {
			get {
				return m_Name;
			}
		}

		/// <summary>
		/// 路径
		/// </summary>
		public string Path {
			get {
				return m_Path;
			}
		}

		/// <summary>
		/// Editor 时候用的,用来生成Get Propery代码的。
		/// </summary>
		public string PropertyGetCode {
			get {
				if (string.IsNullOrEmpty (m_Name))
					return null;
				
				var retString = "m_" + m_Name;
				switch (m_Root) {
					case PathRoot.EditorPath:
						retString = "\"" + "Assets/" + "\"" + " + " + retString;
						break;
					case PathRoot.ApplicationDataPath:
						retString = "UnityEngine.Application.dataPath" + " + " + "\"/\"" + " + " + retString;
						break;
					case PathRoot.ApplicationPersistentDataPath:
						retString = "UnityEngine.Application.persistentDataPath" + " + " + "\"/\"" + " + " + retString;
						break;
					case PathRoot.ApplicationStreamingAssetsPath:
						retString = "UnityEngine.Application.streamingAssetsPath" + " + " + "\"/\"" + " + " + retString;
						break;
				}

				if (m_AutoCreateDirectory) {
					retString = "QFramework.Libs.IOUtils.CreateDirIfNotExists (" + retString + ")";
				}

				return retString;
			}
		}

		public string Description {
			get {
				return m_Description;
			}
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
		public List<PathItem> List {
			get {
				return m_PathList;
			}
		}

		public string ScriptGeneratePath {
			get {
				return m_ScriptGeneratePath;
			}
		}

		public string Description {
			get {
				return m_Description;
			}
		}

		public string NameSpace {
			get {
				return m_NameSpace;
			}
		}
	}
}
