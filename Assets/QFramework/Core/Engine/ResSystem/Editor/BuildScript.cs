using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Xml;

using System;
using System.Text;
using System.Security.Cryptography;
using ICSharpCode.SharpZipLib.Zip;
using System.Linq;
using QFramework.Libs;

namespace QFramework
{
	public class BuildScript
	{
		public static string overloadedDevelopmentServerURL = "";
	
		private static string projectTag = "qframework";

		public static void SetProjectTag(){

			string projectMark = "_project_";
			var allAbPaths = AssetDatabase.GetAllAssetPaths ();
			foreach (string path in allAbPaths) {
				if (string.IsNullOrEmpty (path)) {
					continue;
				}
				AssetImporter ai = AssetImporter.GetAtPath (path);
				if (!string.IsNullOrEmpty(ai.assetBundleName)) {
					string abName = ai.assetBundleName;
					int lastIndex = abName.LastIndexOf (projectMark);
					if (lastIndex >= 0) {
						abName = abName.Remove (lastIndex, abName.Length -lastIndex );
					}
					Debug.Log (abName+":abname:"+projectTag+":bbb:"+string.IsNullOrEmpty (projectTag));
					if (string.IsNullOrEmpty (projectTag)) {
						ai.assetBundleName = abName;
					} else {
						ai.assetBundleName = abName + projectMark + projectTag;
					}

				}
				AssetDatabase.SaveAssets ();

			}
		}

		public static void BuildAssetBundles(BuildTarget buildTarget,string inputProjectTag)
		{
			if (string.IsNullOrEmpty (inputProjectTag)) {
				projectTag = "qframework";
			} else {
				projectTag = inputProjectTag;
			}
			SetProjectTag ();

			// Choose the output path according to the build target.
			string outputPath = Path.Combine(QAssetBundleTool.AssetBundlesOutputPath,  GetPlatformName());
		
			outputPath = outputPath + "/" + projectTag;
		
			if (!Directory.Exists (outputPath)) {
				Directory.CreateDirectory (outputPath);
			}
				
			BuildPipeline.BuildAssetBundles(outputPath,BuildAssetBundleOptions.ChunkBasedCompression,buildTarget);

			List<string> finalzips = PackZips (outputPath);
			List<string> finalFiles = PackPTFiles (outputPath);

			GenerateVersionConfig (outputPath,finalzips,finalFiles);

			string finalDir = Application.streamingAssetsPath+"/AssetBundles/"+GetPlatformName()+"/"+projectTag;

			IOUtils.DeleteDirIfExists (finalDir);
			IOUtils.CreateDirIfNotExists (finalDir);
			FileUtil.ReplaceDirectory (outputPath,finalDir);
			AssetDatabase.Refresh ();
			// TODO: 欧阳Framework支持
			AssetBundleExporter.BuildDataTable ();
		}

		/// <summary>
		/// 遍历压缩所有zip文件
		/// </summary>
		/// <returns>The zips.</returns>
		/// <param name="outpath">Outpath.</param>
		public static  List<string> PackZips(string outpath){
			 List<string> finalZipFiles = new List<string>();
			var allDirs = AssetDatabase.GetAllAssetPaths ().Where(path=>(Directory.Exists(path)));
			foreach(var path in allDirs){
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath (path,typeof(UnityEngine.Object));
				if (QAssetBundleTool.HasPTABLabel (obj, QAssetBundleTool.LABEL_ZIP)) {
					PackFiles(outpath + "/"+Path.GetFileName(path)+".zip", path);
					finalZipFiles.Add (outpath + "/"+Path.GetFileName(path)+".zip");
				}
			}
			return finalZipFiles;
		}
			
		/// <summary>
		/// 遍历复制所有标记了file的文件
		/// </summary>
		/// <returns>The point files.</returns>
		/// <param name="outpath">Outpath.</param>
		public static List<string> PackPTFiles(	string outpath){

			List<string> finalMarkedFiles = new List<string>();
			var allFiles = AssetDatabase.GetAllAssetPaths ().Where(path=>(File.Exists(path)));
			List<string> files = new List<string> ();
			foreach(var k in allFiles){
				UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath (k,typeof(UnityEngine.Object));
				if (QAssetBundleTool.HasPTABLabel (obj, QAssetBundleTool.LABEL_FILE)) {
					files.Add (k);
					FileUtil.ReplaceFile (k,outpath+"/"+Path.GetFileName(k));
					finalMarkedFiles.Add (outpath + "/"+Path.GetFileName(k));
				}
			}

			return finalMarkedFiles;
		}

		/// <summary>
		/// MAC上再文件夹内容一致的情况下用fastzip压缩出来的zip文件 md5不一致，所以单独用shell脚本处理。
		/// </summary>
		/// <param name="filename">Filename.</param>
		/// <param name="directory">Directory.</param>
		public static void PackFiles(string filename, string directory)
		{
			try
			{
				if(Application.platform==RuntimePlatform.OSXEditor){
					Debug.Log("pack files in osx editor ***********");
					string shell = Application.dataPath +"/PTUGame/PTABManager/Editor/shell.sh";
					Debug.Log(File.Exists(shell));
					string arg1 = Path.GetFullPath(filename);
					string arg2 = Path.GetFullPath(directory);
					Debug.Log(arg1+":");
					Debug.Log(arg2+":");
					string argss =  shell +" "+ arg1 +" " + arg2;
					if(File.Exists(arg1)){
						File.Delete(arg1);
					}
					System.Diagnostics.Process sampleProcess= new System.Diagnostics.Process();
					sampleProcess.StartInfo.FileName = "/bin/bash";   //IE浏览器，可以更换
					sampleProcess.StartInfo.Arguments = argss;
					sampleProcess.Start();
					sampleProcess.WaitForExit();

				}else{
					Debug.Log("pack files in windows editor ***********");
					FastZip fz = new FastZip();
					fz.CreateEmptyDirectories = true;
					fz.CreateZip(filename, directory, true, "");
					fz = null;
				}
			
			}
			catch (Exception e)
			{
				Debug.Log (e);
				throw;
			}
		}

		private static void GenerateVersionConfig(string outputPath,List<string> finalZips,List<string> finalFiles){
			string abManifestFile;
			if (projectTag != "")
				abManifestFile = Path.Combine (outputPath, projectTag);
			else 
				abManifestFile = Path.Combine (outputPath, GetPlatformName ());

			AssetBundle ab = AssetBundle.LoadFromFile (abManifestFile);

			AssetBundleManifest abMainfest = (AssetBundleManifest)ab.LoadAsset("AssetBundleManifest");
			string[] allABNames = abMainfest.GetAllAssetBundles ();
			XmlDocument xmlDoc = new XmlDocument ();
			XmlElement xmlRoot = xmlDoc.CreateElement ("config");
			xmlRoot.SetAttribute ("res_version", QAssetBundleBuilder.resVersion);
			xmlDoc.AppendChild (xmlRoot);
			assetBundleInfos.Clear ();
			for (int i = 0; i < allABNames.Length; i++) {

				XmlElement xmlItem = CreateConfigItem(xmlDoc,Path.Combine (outputPath, allABNames [i]),allABNames [i],allABNames [i]);
				xmlRoot.AppendChild (xmlItem);

				AssetBundle assetBundle = AssetBundle.LoadFromFile (Path.Combine (outputPath, allABNames [i]));
				AssetBundleInfo abInfo= new AssetBundleInfo (allABNames[i]);
				abInfo.assets = assetBundle.GetAllAssetNames ();
				assetBundleInfos.Add (abInfo);
				assetBundle.Unload (true);

			}
			// 这里要加上平台相关的xml
			string platformBundleName = GetPlatformName ();
			XmlElement platformItem ;
			if (projectTag == "") {
				platformItem = CreateConfigItem(xmlDoc,abManifestFile,platformBundleName,platformBundleName);
			} else {
				platformItem = CreateConfigItem(xmlDoc,abManifestFile,projectTag,projectTag);
			}
			xmlRoot.AppendChild (platformItem);
		
			foreach(var zipPath in finalZips){
				XmlElement zipItem = CreateConfigItem(xmlDoc,zipPath,Path.GetFileName(zipPath),Path.GetFileName(zipPath));
				xmlRoot.AppendChild (zipItem);
			}
			foreach(var filePath in finalFiles){
				XmlElement fileItem = CreateConfigItem(xmlDoc,filePath,Path.GetFileName(filePath),Path.GetFileName(filePath));
				xmlRoot.AppendChild (fileItem);
			}

			ab.Unload (true);

			xmlDoc.Save (outputPath + "/resconfig.xml");
			AssetDatabase.Refresh ();

			if (QAssetBundleBuilder.isEnableGenerateClass) {
				IOUtils.CreateDirIfNotExists ("QFrameworkData");

				QABCodeGenerator.Generate ("QAssetBundle",assetBundleInfos,projectTag);
			}
		}

		private static XmlElement CreateConfigItem(XmlDocument xmlDoc,string filePath,string fileName,string finalPath){
			XmlElement platformItem = xmlDoc.CreateElement ("item");

			platformItem.SetAttribute ("name", fileName);
			platformItem.SetAttribute ("path", finalPath);

			byte[] platformFileBytes = getFileBytes(filePath);
			platformItem.SetAttribute ("hash", getMD5(platformFileBytes));
			platformItem.SetAttribute ("size", getSize (platformFileBytes));
			return platformItem;
		}

		private static List<AssetBundleInfo> assetBundleInfos = new List<AssetBundleInfo>();
	
		public static string GetBuildTargetName(BuildTarget target)
		{
			switch(target)
			{
			case BuildTarget.Android :
				return "/test.apk";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "/test.exe";
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSXUniversal:
				return "/test.app";
			case BuildTarget.WebPlayer:
			case BuildTarget.WebPlayerStreamed:
			case BuildTarget.WebGL:
				return "";
				// Add more build targets for your own.
			default:
				Debug.Log("Target not implemented.");
				return null;
			}
		}
		public static string GetPlatformName()
		{
			return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
		}
		private static string GetPlatformForAssetBundles(BuildTarget target)
				{
					switch(target)
					{
					case BuildTarget.Android:
						return "Android";
					case BuildTarget.iOS:
						return "iOS";
					case BuildTarget.WebGL:
						return "WebGL";
					case BuildTarget.WebPlayer:
						return "WebPlayer";
					case BuildTarget.StandaloneWindows:
					case BuildTarget.StandaloneWindows64:
						return "Windows";
					case BuildTarget.StandaloneOSXIntel:
					case BuildTarget.StandaloneOSXIntel64:
					case BuildTarget.StandaloneOSXUniversal:
						return "OSX";
						// Add more build targets for your own.
						// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
					default:
						return null;
					}
				}
	
		static void CopyAssetBundlesTo(string outputPath)
		{
			// Clear streaming assets folder.
			FileUtil.DeleteFileOrDirectory(Application.streamingAssetsPath);
			Directory.CreateDirectory(outputPath);
	
			string outputFolder = GetPlatformName();
	
			// Setup the source folder for assetbundles.
			var source = Path.Combine(Path.Combine(System.Environment.CurrentDirectory, QAssetBundleTool.AssetBundlesOutputPath), outputFolder);
			if (!System.IO.Directory.Exists(source) )
				Debug.Log("No assetBundle output folder, try to build the assetBundles first.");
	
			// Setup the destination folder for assetbundles.
			var destination = System.IO.Path.Combine(outputPath, outputFolder);
			if (System.IO.Directory.Exists(destination) )
				FileUtil.DeleteFileOrDirectory(destination);
			
			FileUtil.CopyFileOrDirectory(source, destination);
		}
	
		static string[] GetLevelsFromBuildSettings()
		{
			List<string> levels = new List<string>();
			for(int i = 0 ; i < EditorBuildSettings.scenes.Length; ++i)
			{
				if (EditorBuildSettings.scenes[i].enabled)
					levels.Add(EditorBuildSettings.scenes[i].path);
			}
	
			return levels.ToArray();
		}
			

		public static byte[] getFileBytes(string filePath)
		{
			FileStream fs = new FileStream(filePath, FileMode.Open);
			int len = (int)fs.Length;
			byte[] data = new byte[len];
			fs.Read(data, 0, len);
			fs.Close();
			return data;
		}

		public static string getMD5(byte[] data)
		{
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(data);
			string fileMD5 = "";
			foreach (byte b in result)
			{
				fileMD5 += Convert.ToString(b, 16);
			}
			return fileMD5;   
		}

		public static string getSize(byte[] data)
		{
			return data.Length.ToString();
		}

		private static void GetDirectorys(string strPath, ref List<string> lstDirect)  
		{  
			DirectoryInfo diFliles = new DirectoryInfo(strPath);  
			DirectoryInfo[] diArr = diFliles.GetDirectories();  
			foreach (DirectoryInfo di in diArr)  
			{  
				try  
				{  
					lstDirect.Add(di.FullName);  
					GetDirectorys(di.FullName, ref lstDirect);  
				}  
				catch   
				{  
					continue;  
				}  
			}  
		}  

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}
	