/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
****************************************************************************/

namespace QFramework
{
	public partial class QUIFactory
	{
		public IUIComponents CreateUIComponentsByUIName(string uiName)
		{
			IUIComponents retComponents = null;
			switch (uiName)
			{
				case "UIEditPanel":
					retComponents = new UIEditPanelComponents();
					break;
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
				case "UIToDoListPage":
					retComponents = new UIToDoListPageComponents();
					break;
			}
			return retComponents;
		}
	}
}
