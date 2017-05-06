using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using QFramework;
using QAssetBundle;

/// <summary>
/// 可以接收的消息类型
/// </summary>
public enum Example4UICtrlEvent {

	SendEventToExample4UICtrl,

	MainPageBtnBackClick,
	MainPageBtnAboutClick,
	MainPageBtnQuitClick,

	AboutPageBtnBackClick,
	GamePageBtnBackClick,

	DialogBtnSureClick,
	DialogBtnCancelClick,
}


namespace QFramework {

	/// <summary>
	/// 控制器
	/// </summary>
	public class Example4UICtrl : MonoBehaviour {


		// Use this for initialization
		void Start () {


			ResMgr.Instance.InitResMgr ();

			QUIManager.Instance.SetResolution (1024, 768);
			QUIManager.Instance.SetMatchOnWidthOrHeight (0);

			QUIManager.Instance.OpenUI<UIExample4MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);

			// 注册消息
			QEventSystem.Instance.Register(Example4UICtrlEvent.SendEventToExample4UICtrl,ProcessEvent);
		}

		/// <summary>
		/// 处理消息
		/// </summary>
		void ProcessEvent(int key,params object[] paramList) 
		{
			Example4UICtrlEvent eventName = (Example4UICtrlEvent)paramList[0];

			switch (eventName) {
				case Example4UICtrlEvent.MainPageBtnBackClick:
					QUIManager.Instance.CloseUI<UIExample4MainPage> ();
					QUIManager.Instance.OpenUI<UIExample4GamePage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
					break;
				case Example4UICtrlEvent.MainPageBtnAboutClick:
					QUIManager.Instance.CloseUI<UIExample4MainPage> ();
					QUIManager.Instance.OpenUI<UIExample4AboutPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
					break;
				case Example4UICtrlEvent.MainPageBtnQuitClick:
					QUIManager.Instance.OpenUI<UIExample4Dialog> (QUILevel.Forward, UIPREFAB.BUNDLE_NAME);
					break;
				case Example4UICtrlEvent.AboutPageBtnBackClick:
					QUIManager.Instance.CloseUI<UIExample4AboutPage> ();
					QUIManager.Instance.OpenUI<UIExample4MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
					break;
				case Example4UICtrlEvent.GamePageBtnBackClick:
					QUIManager.Instance.CloseUI<UIExample4GamePage> ();
					QUIManager.Instance.OpenUI<UIExample4MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
					break;

				case Example4UICtrlEvent.DialogBtnSureClick:
					#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying = false;
					#endif
					break;
				case Example4UICtrlEvent.DialogBtnCancelClick:
					QUIManager.Instance.CloseUI<UIExample4Dialog> ();
					break;
			}
		}

		/// <summary>
		/// 要注销消息
		/// </summary>
		void OnDestroy() 
		{
			QEventSystem.Instance.UnRegister(Example4UICtrlEvent.SendEventToExample4UICtrl,ProcessEvent);
		}

	}

}
