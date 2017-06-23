using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class UIExample5MainPage : QUIBehaviour
{
	protected override void InitUI(QUIData uiData = null)
	{
		mUIComponents = mIComponents as UIExample5MainPageComponents;
		//please add init code here
	}
	protected override void ProcessMsg (int key,QMsg msg)
	{
		throw new System.NotImplementedException ();
	}
	protected override void RegisterUIEvent()
	{
		mUIComponents.BtnStart_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example5UIEvent.SendEventToExample5UICtrl,
				new object[]{ Example5UIEvent.BtnStartClick });
		});

		mUIComponents.BtnAbout_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example5UIEvent.SendEventToExample5UICtrl,
				new object[]{ Example5UIEvent.BtnAboutClick });
		});

		mUIComponents.BtnQuitGame_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example5UIEvent.SendEventToExample5UICtrl,
				new object[]{ Example5UIEvent.BtnQuitClick });
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
		Debug.Log("[ UIExample5MainPage:]" + content);
	}

	UIExample5MainPageComponents mUIComponents = null;
}