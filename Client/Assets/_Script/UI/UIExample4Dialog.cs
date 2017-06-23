using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class UIExample4Dialog : QUIBehaviour
{
	protected override void InitUI(QUIData uiData = null)
	{
		mUIComponents = mIComponents as UIExample4DialogComponents;
		//please add init code here
	}
	protected override void ProcessMsg (int key,QMsg msg)
	{
		throw new System.NotImplementedException ();
	}
	protected override void RegisterUIEvent()
	{
		mUIComponents.BtnSure_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example4UICtrlEvent.SendEventToExample4UICtrl,
				new object[]{Example4UICtrlEvent.DialogBtnSureClick });
		});

		mUIComponents.BtnCancel_Button.onClick.AddListener (delegate {
			QEventSystem.Instance.Send (Example4UICtrlEvent.SendEventToExample4UICtrl,
				new object[]{Example4UICtrlEvent.DialogBtnCancelClick });
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
		Debug.Log("[ UIExample4Dialog:]" + content);
	}

	UIExample4DialogComponents mUIComponents = null;
}