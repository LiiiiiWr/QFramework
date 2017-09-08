using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Linq;
using QFramework.Libs;

namespace QFramework.ResSystem
{
	public class QResSystemBuilder : EditorWindow
	{

		private int buildTargetIndex = 0;
		private string[] platformLabels = new string[] {"Windows32", "iOS", "Android"};
		private Vector2 scrollPos;
		private const string KEY_QAssetBundleBuilder_RESVERSION = "KEY_QAssetBundleBuilder_RESVERSION";
		private const string KEY_AUTOGENERATE_CLASS = "KEY_AUTOGENERATE_CLASS";

		private const string KEY_ProjectTag = "KEY_ProjectTag";
		private const string KEY_ZipFramework = "KEY_ZipFramework";


		public static string resVersion = "100";

		private static string projectTag = "";

		//public static bool isUseFramework = true;
		public static bool isEnableGenerateClass = false;

		public static void ForceClear()
		{

			IOUtils.DeleteDirIfExists(QAssetBundleTool.AssetBundlesOutputPath);
			IOUtils.DeleteDirIfExists(Application.streamingAssetsPath + "/AssetBundles");

			AssetDatabase.Refresh();
		}

#if QABMANAGER_SUPPORT

#else
		[MenuItem("QFramework/ResSystem/Builder")]
#endif
		public static void ExecuteAssetBundle()
		{

			QResSystemBuilder window = (QResSystemBuilder) GetWindow(typeof(QResSystemBuilder), true);
			Debug.Log(Screen.width + " screen width*****");
			window.position = new Rect(100, 100, 500, 400);
			window.Show();
		}

		void OnEnable()
		{
			resVersion = EditorPrefs.GetString(KEY_QAssetBundleBuilder_RESVERSION, "100");
			isEnableGenerateClass = EditorPrefs.GetBool(KEY_AUTOGENERATE_CLASS, true);

			projectTag = EditorPrefs.GetString(KEY_ProjectTag, "");
			//isUseFramework = EditorPrefs.GetBool (KEY_ZipFramework,true);


			switch (EditorUserBuildSettings.activeBuildTarget)
			{
				case BuildTarget.Android:
					buildTargetIndex = 2;
					break;
				case BuildTarget.iOS:
					buildTargetIndex = 1;
					break;
				default:
					buildTargetIndex = 0;
					break;

			}
		}

		void DrawMenu()
		{
			GUILayout.Toolbar(buildTargetIndex, platformLabels);
		}

		void DrawAssetBundleList()
		{
		}


		public void OnDisable()
		{
			EditorPrefs.SetBool(KEY_AUTOGENERATE_CLASS, isEnableGenerateClass);
			EditorPrefs.SetString(KEY_QAssetBundleBuilder_RESVERSION, resVersion);
			EditorPrefs.SetString(KEY_ProjectTag, projectTag);
		}

		void OnGUI()
		{
			scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(500), GUILayout.Height(400));
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			GUILayout.Label("PersistanePath:");
			GUILayout.TextField(Application.persistentDataPath);
			GUILayout.EndHorizontal();
			if (GUILayout.Button("Go To Persistance"))
			{
				EditorUtility.RevealInFinder(Application.persistentDataPath);
			}

			DrawMenu();
			DrawAssetBundleList();

			isEnableGenerateClass = GUILayout.Toggle(isEnableGenerateClass, "auto generate class");



			GUILayout.BeginHorizontal();
			GUILayout.Label("ResVersion:");
			resVersion = GUILayout.TextField(resVersion);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Project Tag:");
			projectTag = GUILayout.TextField(projectTag);

			GUILayout.EndHorizontal();

			if (GUILayout.Button("1.Gen Res Tree File"))
			{
				AssetBundleExporter.BuildDataTable();
			}

			if (GUILayout.Button("2.Build"))
			{
				AssetBundleExporter.BuildDataTable();
				BuildWithTarget(EditorUserBuildSettings.activeBuildTarget);
			}

			if (GUILayout.Button("ForceClear"))
			{
				ForceClear();
			}

			GUILayout.EndVertical();
			GUILayout.Space(50);

			EditorGUILayout.EndScrollView();

		}

		void BuildWithTarget(BuildTarget buildTarget)
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();
			AssetDatabase.Refresh();
			BuildScript.BuildAssetBundles(buildTarget, projectTag);
		}
	}
}