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

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using QFramework.Libs;

namespace QFramework.Editor
{
    public class IOPathEditor
    {
		const string m_DefaultPathConfigGenerateForder = "Assets/QFrameworkData/Path/Config";

        [MenuItem("QFramework/IOPath/Gen Path Asset File")]
        public static void GenPathAssetFile()
        {
			AssetDatabase.SaveAssets ();

			PathConfig data = null;

			IOUtils.CreateDirIfNotExists (m_DefaultPathConfigGenerateForder);

			string newConfigPath = IOEditorPathConfig.IOGeneratorPath + "/NewPathConfig.asset";

			data = AssetDatabase.LoadAssetAtPath<PathConfig>(newConfigPath);
            if (data == null)
            {
				data = ScriptableObject.CreateInstance<PathConfig>();
				AssetDatabase.CreateAsset(data, newConfigPath);
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
		}

		const string m_DefaultPathScriptGenerateForder = "Assets/QFrameworkData/Path/Script";

		[MenuItem("QFramework/IOPath/Gen Path Script")]
		public static void GeneratePathScript() 
		{
			AssetDatabase.SaveAssets ();

			IOUtils.CreateDirIfNotExists (m_DefaultPathScriptGenerateForder);

			string[] fullPathFileNames = Directory.GetFiles(m_DefaultPathConfigGenerateForder, "*PathConfig.asset", SearchOption.AllDirectories);

			foreach(string fullPathFileName in fullPathFileNames) 
			{
				Debug.Log (fullPathFileName);
				if (!fullPathFileName.EndsWith (".meta")) 
				{
					Debug.Log ("gen: " + fullPathFileName);

					PathConfig config = AssetDatabase.LoadAssetAtPath<PathConfig> (fullPathFileName);
					QNamespaceDefine nameSpace = new QNamespaceDefine ();
					nameSpace.Name = string.IsNullOrEmpty (config.NameSpace) ? "QFramework" : config.NameSpace;
					nameSpace.FileName = config.name + ".cs";
					nameSpace.GenerateDir = string.IsNullOrEmpty (config.ScriptGeneratePath) ? m_DefaultPathScriptGenerateForder : IOUtils.CreateDirIfNotExists ("Assets/" + config.ScriptGeneratePath);
					var classDefine = new QClassDefine ();
					classDefine.Comment = config.Description;
					classDefine.Name = config.name;
					nameSpace.Classes.Add (classDefine);
					Debug.Log (nameSpace.GenerateDir);
					foreach (var pathItem in config.List) 
					{
						if (!string.IsNullOrEmpty(pathItem.Name)) 
						{
							var variable = new QVariable (QAccessLimit.Private, QCompileType.Const, QTypeDefine.String,"m_" + pathItem.Name, pathItem.Path);
							classDefine.Variables.Add (variable);

							var property = new QProperty (QAccessLimit.Public, QCompileType.Static, QTypeDefine.String, pathItem.Name, pathItem.PropertyGetCode, pathItem.Description);
							classDefine.Properties.Add (property);
						}
					}
					QCodeGenerator.Generate (nameSpace);

					EditorUtility.SetDirty (config);
					Resources.UnloadAsset (config);
				}
			}
				
			AssetDatabase.SaveAssets();
		}
    }
}
