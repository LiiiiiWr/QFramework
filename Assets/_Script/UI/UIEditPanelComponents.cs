using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
public class UIEditPanelComponents : IUIComponents
{
	public void InitUIComponents()
	{
		ContentInputField_InputField = QUIManager.Instance.Get<UIEditPanel>("ContentInputField").GetComponent<InputField>();
		BtnSave_Button = QUIManager.Instance.Get<UIEditPanel>("BtnSave").GetComponent<Button>();
		BtnCancel_Button = QUIManager.Instance.Get<UIEditPanel>("BtnCancel").GetComponent<Button>();
	}

	public void Clear()
	{
		ContentInputField_InputField = null;
		BtnSave_Button = null;
		BtnCancel_Button = null;
	}

	public InputField ContentInputField_InputField;
	public Button BtnSave_Button;
	public Button BtnCancel_Button;
}
