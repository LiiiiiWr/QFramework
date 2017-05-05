using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class UIExample5Dialog : QUIBehaviour
{
	protected override void InitUI(QUIData uiData = null)
	{
		mUIComponents = mIComponents as UIExample5DialogComponents;
		//please add init code here
	}
	protected override void ProcessMsg (int key,QMsg msg)
	{
		throw new System.NotImplementedException ();
	}
	protected override void RegisterUIEvent()
	{
		mUIComponents.BtnSure_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example5UIEvent.SendEventToExample5UICtrl,
				new object[]{Example5UIEvent.BtnSureClick });
		});

		mUIComponents.BtnCancel_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example5UIEvent.SendEventToExample5UICtrl,
				new object[]{Example5UIEvent.BtnCancelClick });
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
		Debug.Log("[ UIExample5Dialog:]" + content);
	}

	UIExample5DialogComponents mUIComponents = null;
}