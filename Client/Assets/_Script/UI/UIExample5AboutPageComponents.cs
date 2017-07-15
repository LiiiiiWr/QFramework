using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
public class UIExample5AboutPageComponents : IUIComponents
{
	public void InitUIComponents()
	{
		BtnBack_Button = QUIManager.Instance.Get<UIExample5AboutPage>("BtnBack").GetComponent<Button>();
		Content_Text = QUIManager.Instance.Get<UIExample5AboutPage>("Content").GetComponent<Text>();
	}

	public void Clear()
	{
		BtnBack_Button = null;
		Content_Text = null;
	}

	public Button BtnBack_Button;
	public Text Content_Text;
}
