using UnityEngine;
using UnityEditor;
using System.Collections;

namespace QFramework
{
	public class AssetBundlesMenuItems
	{
		const string kSimulationMode = "QFramework/Res/Simulation Mode";
	
		[MenuItem(kSimulationMode)]
		public static void ToggleSimulationMode ()
		{
			ABUtility.SimulateAssetBundleInEditor = !ABUtility.SimulateAssetBundleInEditor;
		}
	
		[MenuItem(kSimulationMode, true)]
		public static bool ToggleSimulationModeValidate ()
		{
			Menu.SetChecked(kSimulationMode, ABUtility.SimulateAssetBundleInEditor);
			return true;
		}
	}
}