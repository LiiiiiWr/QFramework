using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace QFramework
{
	public class ABUtility
	{
		#if UNITY_EDITOR
		private static int m_SimulateAssetBundleInEditor = -1;
		private const string kSimulateAssetBundles = "SimulateAssetBundles"; //此处跟editor中保持统一，不能随意更改
		#endif

		#if UNITY_EDITOR
		// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		public static bool SimulateAssetBundleInEditor {
			get {
				if (m_SimulateAssetBundleInEditor == -1)
				{
					m_SimulateAssetBundleInEditor = EditorPrefs.GetBool (kSimulateAssetBundles, true) ? 1 : 0;
				}
				return m_SimulateAssetBundleInEditor != 0;
			}
			set {
				int newValue = value ? 1 : 0;
				if (newValue != m_SimulateAssetBundleInEditor) 
				{
					m_SimulateAssetBundleInEditor = newValue;
					EditorPrefs.SetBool (kSimulateAssetBundles, value);
				}
			}
		}
		#endif
	}
}