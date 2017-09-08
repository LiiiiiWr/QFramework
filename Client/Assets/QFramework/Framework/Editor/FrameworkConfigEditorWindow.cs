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

/// <summary>
/// Project config editor window.
/// </summary>
namespace QFramework.Editor
{
	using UnityEngine;
	using UnityEditor;
	using QFramework.Libs.Editor;
	
	public class FrameworkConfigEditorWindow : EditorWindow
	{
		[MenuItem("QFramework/FrameworkConfig")]
		static void Open() 
		{
			FrameworkConfigEditorWindow frameworkConfigEditorWindow = (FrameworkConfigEditorWindow)EditorWindow.GetWindow(typeof(FrameworkConfigEditorWindow),true);
			frameworkConfigEditorWindow.titleContent = new  GUIContent("FrameworkConfig");
			frameworkConfigEditorWindow.CurConfigData = FrameworkConfigData.Load ();
			frameworkConfigEditorWindow.Show ();
		}

		public FrameworkConfigEditorWindow() {}

		public FrameworkConfigData CurConfigData;
	
		void OnGUI() 
		{
			CurConfigData.Namespace = EditorGUIUtils.GUILabelAndTextField ("Namespace", CurConfigData.Namespace);
		
			#if NONE_LUA_SUPPORT 
			CurConfigData.UIScriptDir = EditorGUIUtils.GUILabelAndTextField ("UI Script Generate Dir", CurConfigData.UIScriptDir);
			CurConfigData.UIPrefabDir = EditorGUIUtils.GUILabelAndTextField ("UI Prefab Dir", CurConfigData.UIPrefabDir);
			#endif

			CurConfigData.ResLoaderSupportIndex = EditorGUIUtils.GUILabelAndPopup("AB Support",
				CurConfigData.ResLoaderSupportIndex, FrameworkConfigData.RES_LOADER_SUPPORT_TEXTS);
			CurConfigData.LuaSupportIndex = EditorGUIUtils.GUILabelAndPopup("Lua Support", CurConfigData.LuaSupportIndex,
				FrameworkConfigData.LUA_SUPPORT_TEXTS);
			CurConfigData.CocosSupportIndex = EditorGUIUtils.GUILabelAndPopup("Cocos Support", CurConfigData.CocosSupportIndex,
				FrameworkConfigData.COCOS_SUPPORT_TEXTS);
			if (GUILayout.Button ("Apply")) 
			{
				CurConfigData.Save ();
				MicroEditor.ApplyAllPlatform ();
			}
		}
	}

	[InitializeOnLoad]
	public class MicroEditor
	{
		static MicroEditor()
		{
			Debug.Log (">>>>>> Initialize MicroEditor");

			ApplyAllPlatform ();
		}

		public static void ApplyAllPlatform()
		{
			var frameworkConfigData = FrameworkConfigData.Load ();

			ApplySymbol (frameworkConfigData, BuildTargetGroup.iOS);
			ApplySymbol (frameworkConfigData, BuildTargetGroup.Android);
			ApplySymbol (frameworkConfigData, BuildTargetGroup.Standalone);
		}


		public static void ApplySymbol(FrameworkConfigData frameworkConfigData,BuildTargetGroup targetGroup)
		{
			string 	symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup (targetGroup);

			if (string.IsNullOrEmpty (symbols)) {
				symbols = frameworkConfigData.ResLoaderSupportSymbol;
			}
			else {
				string[] symbolSplit = symbols.Split (new char[]{ ';' });

				symbols = "";

				for (int i = 0; i < symbolSplit.Length; i++) 
				{
					var symbol = symbolSplit [i];
					if (string.Equals (symbol, FrameworkConfigData.RES_LOADER_SUPPORT_SYMBOLS [0]) ||
						string.Equals (symbol, FrameworkConfigData.RES_LOADER_SUPPORT_SYMBOLS [1]) ||
						string.Equals (symbol, FrameworkConfigData.LUA_SUPPORT_SYMBOLS [0]) ||
						string.Equals (symbol, FrameworkConfigData.LUA_SUPPORT_SYMBOLS [1]) ||
						string.Equals (symbol, FrameworkConfigData.LUA_SUPPORT_SYMBOLS [2]) ||
						string.Equals (symbol, FrameworkConfigData.LUA_SUPPORT_SYMBOLS [3]) ||
					    string.Equals (symbol, FrameworkConfigData.COCOS_SUPPORT_SYMBOLS[0]) ||
					    string.Equals (symbol, FrameworkConfigData.COCOS_SUPPORT_SYMBOLS[1]))
					{

					}
					else {
						symbols += symbol + ";";
					}
				}

				symbols += frameworkConfigData.ResLoaderSupportSymbol + ";";
				symbols += frameworkConfigData.LuaSupportSymbol + ";";
				symbols += frameworkConfigData.CocosSupportSymbol + ";";
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup (targetGroup, symbols);
		}
	}
}
