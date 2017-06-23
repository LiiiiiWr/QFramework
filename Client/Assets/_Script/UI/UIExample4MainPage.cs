using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class UIExample4MainPage : QUIBehaviour
{
	protected override void InitUI(QUIData uiData = null)
	{
		mUIComponents = mIComponents as UIExample4MainPageComponents;
		//please add init code here
	}
	protected override void ProcessMsg (int key,QMsg msg)
	{
		throw new System.NotImplementedException ();
	}
	protected override void RegisterUIEvent()
	{
		mUIComponents.BtnStart_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example4UICtrlEvent.SendEventToExample4UICtrl,
				new object[]{ Example4UICtrlEvent.MainPageBtnBackClick });
		});

		mUIComponents.BtnAbout_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example4UICtrlEvent.SendEventToExample4UICtrl,
				new object[]{ Example4UICtrlEvent.MainPageBtnAboutClick });
		});

		mUIComponents.BtnQuitGame_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example4UICtrlEvent.SendEventToExample4UICtrl,
				new object[]{ Example4UICtrlEvent.MainPageBtnQuitClick });
		});
	}
	protected override void OnShow()
	{
		base.OnShow();
	}

	protected override void OnHide()
	{
		base.OnHide();
	}

	void ShowLog(string content)
	{
		Debug.Log("[ UIExample4MainPage:]" + content);
	}

	UIExample4MainPageComponents mUIComponents = null;
}