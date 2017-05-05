using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using QFramework;
using QAssetBundle;

/// <summary>
/// 可以接收的消息类型
/// </summary>
public enum Example5UIEvent 
{
	SendEventToExample5UICtrl,
	BtnStartClick,
	BtnAboutClick,
	BtnQuitClick,
	BtnBackClick,
	BtnSureClick,
	BtnCancelClick,
}
	
namespace QFramework 
{
	/// <summary>
	/// 控制器
	/// </summary>
	public class Example5UICtrl : MonoBehaviour
	{
		QFSMLite mFSM = new QFSMLite();

		const string STATE_MAIN_PAGE = "MainPage";
		const string STATE_GAME_PAGE = "GamePage";
		const string STATE_ABOUT_PAGE = "AboutPage";
		const string STATE_QUIT_DIALOG = "Dialog";
		const string STATE_QUIT = "Quit";

		// Use this for initialization
		void Start () 
		{
			// 注册消息
			QEventSystem.Instance.Register(Example5UIEvent.SendEventToExample5UICtrl,ProcessEvent);

			ResMgr.Instance.InitResMgr ();

			QUIManager.Instance.SetResolution (1024, 768);
			QUIManager.Instance.SetMatchOnWidthOrHeight (0);

			mFSM.AddState (STATE_MAIN_PAGE);
			mFSM.AddState (STATE_GAME_PAGE);
			mFSM.AddState (STATE_ABOUT_PAGE);
			mFSM.AddState (STATE_QUIT_DIALOG);
			mFSM.AddState (STATE_QUIT);

			// main->game
			mFSM.AddTranslation (STATE_MAIN_PAGE, Example5UIEvent.BtnStartClick.ToString(), STATE_GAME_PAGE, delegate {
				QUIManager.Instance.CloseUI<UIExample5MainPage> ();
				QUIManager.Instance.OpenUI<UIExample5GamePage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
			});

			// main->about
			mFSM.AddTranslation (STATE_MAIN_PAGE, Example5UIEvent.BtnAboutClick.ToString(), STATE_ABOUT_PAGE, delegate {
				QUIManager.Instance.CloseUI<UIExample5MainPage> ();
				QUIManager.Instance.OpenUI<UIExample5AboutPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
			});

			// main->quit
			mFSM.AddTranslation (STATE_MAIN_PAGE, Example5UIEvent.BtnQuitClick.ToString(), STATE_QUIT_DIALOG, delegate {
				QUIManager.Instance.OpenUI<UIExample5Dialog> (QUILevel.Forward, UIPREFAB.BUNDLE_NAME);
			});

			// about->main
			mFSM.AddTranslation (STATE_ABOUT_PAGE, Example5UIEvent.BtnBackClick.ToString(), STATE_MAIN_PAGE, delegate {
				QUIManager.Instance.CloseUI<UIExample5AboutPage> ();
				QUIManager.Instance.OpenUI<UIExample5MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
			});

			// game->main
			mFSM.AddTranslation (STATE_GAME_PAGE, Example5UIEvent.BtnBackClick.ToString(), STATE_MAIN_PAGE, delegate {
				QUIManager.Instance.CloseUI<UIExample5GamePage> ();
				QUIManager.Instance.OpenUI<UIExample5MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
			});

			// quit->quitgame
			mFSM.AddTranslation (STATE_QUIT_DIALOG, Example5UIEvent.BtnSureClick.ToString(), STATE_QUIT, delegate {
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
				#endif
			});

			// quit->main
			mFSM.AddTranslation (STATE_QUIT_DIALOG, Example5UIEvent.BtnCancelClick.ToString(), STATE_MAIN_PAGE, delegate {
				QUIManager.Instance.CloseUI<UIExample5Dialog> ();
			});

			// 设置好当前状态
			mFSM.Start (STATE_MAIN_PAGE);
			QUIManager.Instance.OpenUI<UIExample5MainPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
		}

		/// <summary>
		/// 处理消息
		/// </summary>
		void ProcessEvent(int key,params object[] paramList) 
		{
			Example5UIEvent eventName = (Example5UIEvent)paramList[0];
			mFSM.HandleEvent (eventName.ToString());
		}

		/// <summary>
		/// 要注销消息
		/// </summary>
		void OnDestroy() 
		{
			QEventSystem.Instance.UnRegister(Example5UIEvent.SendEventToExample5UICtrl,ProcessEvent);
		}
	}
}