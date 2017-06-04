using UnityEngine.UI;
using QFramework;

public class UIExample5AboutPageComponents : IUIComponents
{
	public void InitUIComponents()
	{
		BtnBack_Button = QUIManager.Instance.Get<UIExample5AboutPage>("BtnBack").GetComponent<Button>();
	}

	public void Clear()
	{
		BtnBack_Button = null;
	}

	public Button BtnBack_Button;
}
