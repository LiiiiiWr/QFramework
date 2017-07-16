using System;
using QFramework;

namespace QFramework
{
	public partial class QUIFactory
	{
		public IUIComponents CreateUIComponentsByUIName(string uiName)
		{
			IUIComponents retComponents = null;
			switch (uiName)
			{
				case "UIExample4AboutPage":
					retComponents = new UIExample4AboutPageComponents();
					break;
				case "UIExample4Dialog":
					retComponents = new UIExample4DialogComponents();
					break;
				case "UIExample4GamePage":
					retComponents = new UIExample4GamePageComponents();
					break;
				case "UIExample4MainPage":
					retComponents = new UIExample4MainPageComponents();
					break;
				case "UIExample5AboutPage":
					retComponents = new UIExample5AboutPageComponents();
					break;
				case "UIExample5Dialog":
					retComponents = new UIExample5DialogComponents();
					break;
				case "UIExample5GamePage":
					retComponents = new UIExample5GamePageComponents();
					break;
				case "UIExample5MainPage":
					retComponents = new UIExample5MainPageComponents();
					break;
				case "UIHomePage":
					retComponents = new UIHomePageComponents();
					break;
			}
			return retComponents;
		}
	}
}
