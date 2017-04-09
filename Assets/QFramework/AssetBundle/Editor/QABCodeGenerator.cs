using System.IO;
using Microsoft.CSharp;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using QFramework.Libs;

namespace QFramework
{
	public class AssetBundleInfo{

		public string name = "";
		public AssetBundleInfo(string name){
			this.name = name;
		}
		public string[] assets;

	}

	public  class QABCodeGenerator
	{
		public static void Generate (string ns, List<AssetBundleInfo> assetBundleInfos,string projectTag=null)
		{
			QNamespaceDefine nameSpace = new QNamespaceDefine ();
			nameSpace.Name = ns;
			nameSpace.GenerateDir = ABEditorPathConfig.ABCodeGeneratePath;
			nameSpace.FileName = "QAssets.cs";

			for(int i=0;i<assetBundleInfos.Count;i++){
				AssetBundleInfo assetBundleInfo = assetBundleInfos [i];
				string className = assetBundleInfo.name;
				string bundleName = className.Substring (0, 1).ToUpper () + className.Substring (1);
				className = className.Substring(0,1).ToUpper()+className.Substring(1).Replace("/","_").Replace("@","_").Replace("!","_");
				if (!string.IsNullOrEmpty (projectTag)) {
					className = className.Replace ("_project_" + projectTag, "");
					bundleName = bundleName.Replace ("_project_" + projectTag, "");
				}
				className = className.ToUpper ();

				QClassDefine classDefine = new QClassDefine ();
				nameSpace.Classes.Add (classDefine);
				classDefine.Name = className;

				QVariable variable = new QVariable (QAccessLimit.Public,QCompileType.Const,QTypeDefine.String,"BUNDLE_NAME",bundleName.ToUpperInvariant ());
				classDefine.Variables.Add (variable);

				foreach (var asset in assetBundleInfo.assets) {
					string content = Path.GetFileNameWithoutExtension (asset).ToUpperInvariant();

					QVariable assetVariable = new QVariable (QAccessLimit.Public,QCompileType.Const,QTypeDefine.String,content.Replace("@","_").Replace("!","_"),content);
					classDefine.Variables.Add (assetVariable);
				}
			}

			QCodeGenerator.Generate(nameSpace);
		}

	}
}