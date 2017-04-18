using UnityEngine;
using System.Collections;
using QFramework;
using QAssetBundle;

namespace QFramework {
	public class UIFrameworkCtrl : MonoBehaviour {

		IEnumerator Start () {
			ResMgr.Instance.InitResMgr ();

			QUIManager.Instance.SetResolution (1024, 768);
			QUIManager.Instance.SetMatchOnWidthOrHeight (0);

			QUIManager.Instance.OpenUI<UIHomePage> (QUILevel.Common,UIPREFAB.BUNDLE_NAME);
			yield return null;
		}
	}
}
