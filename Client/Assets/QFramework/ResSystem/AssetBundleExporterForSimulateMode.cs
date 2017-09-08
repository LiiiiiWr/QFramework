#if UNITY_EDITOR
using UnityEditor;

using System.Collections.Generic;
using System.IO;
using QFramework.Libs;

namespace QFramework.ResSystem
{
    public class AssetBundleExporterForSimulateMode
    {
		public static void BuildDataTable()
		{
			Log.i("Start BuildAssetDataTable!");
			AssetDataTable table = AssetDataTable.Create();

			ProcessAssetBundleRes(table);

		    string filePath =
		        IOUtils.CreateDirIfNotExists(FilePath.streamingAssetsPath + FrameworkConfigData.RELATIVE_AB_ROOT_FOLDER) +
		        FrameworkConfigData.EXPORT_ASSETBUNDLE_CONFIG_FILENAME;
			table.Save(filePath);
			AssetDatabase.Refresh ();
		}


#region 构建 AssetDataTable

        private static string AssetPath2Name(string assetPath)
        {
            int startIndex = assetPath.LastIndexOf("/") + 1;
            int endIndex = assetPath.LastIndexOf(".");

            if (endIndex > 0)
            {
                int length = endIndex - startIndex;
                return assetPath.Substring(startIndex, length).ToLower();
            }

            return assetPath.Substring(startIndex).ToLower();
        }

        private static void ProcessAssetBundleRes(AssetDataTable table)
        {
            AssetDataGroup group = null;

			int abIndex = table.AddAssetBundleName(FrameworkConfigData.ABMANIFEST_AB_NAME, null, out group);

            if (abIndex > 0)
            {
				group.AddAssetData(new AssetData(FrameworkConfigData.ABMANIFEST_ASSET_NAME, eResType.kABAsset, abIndex,null));
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();

            string[] abNames = AssetDatabase.GetAllAssetBundleNames();
            if (abNames != null && abNames.Length > 0)
            {
                for (int i = 0; i < abNames.Length; ++i)
                {
                    string[] depends = AssetDatabase.GetAssetBundleDependencies(abNames[i], false);
                    abIndex = table.AddAssetBundleName(abNames[i], depends, out group);
                    if (abIndex < 0)
                    {
                        continue;
                    }

                    string[] assets = AssetDatabase.GetAssetPathsFromAssetBundle(abNames[i]);
                    foreach (var cell in assets)
                    {
                        if (cell.EndsWith(".unity"))
                        {
                            group.AddAssetData(new AssetData(AssetPath2Name(cell), eResType.kABScene, abIndex,abNames[i]));
                        }
                        else
                        {
                            group.AddAssetData(new AssetData(AssetPath2Name(cell), eResType.kABAsset, abIndex,abNames[i]));
                        }
                    }
                }
            }

            table.Dump();
        }
#endregion

    }
}
#endif