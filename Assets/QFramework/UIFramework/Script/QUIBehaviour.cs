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
 * 
****************************************************************************/

using UnityEngine;

namespace QFramework {

	/// <summary>
	/// 每个UIBehaviour对应Data
	/// </summary>
	public class QUIData {}

	public class DefaultUIDate : QUIData {}

	public class QUIDataWithObject : QUIData {}

	public abstract class QUIBehaviour : QMonoBehaviour,IUI {

		ResLoader mResLoader = null;

		protected override void SetupMgr ()
		{
			mCurMgr = QUIManager.Instance;
		}

		protected override void OnBeforeDestroy()
		{
			DestroyUI();

			if (mIComponents != null)
			{
				mIComponents.Clear();
			}
				
			Debug.Log(name + " remove Success");
		}

		public void Init(ResLoader resLoader, QUIData uiData = null)
		{
			mResLoader = resLoader;
			InnerInit(uiData);
			RegisterUIEvent();
		}

		/// <summary>
		/// 关闭
		/// </summary>
		void IUI.Close(bool destroy = true) {
			OnClose ();
			if (destroy) {
				Destroy (gameObject);
			}
			
			mResLoader.Recycle2Cache ();
			mResLoader = null;
		}


		public void CloseSelf() {
			QUIManager.Instance.CloseUI (name);
		}

		/// <summary>
		/// 关闭
		/// </summary>
		protected virtual void OnClose() {}

		void InnerInit(QUIData uiData = null)
		{
			mIComponents = QUIFactory.Instance.CreateUIComponents(this.name);
			mIComponents.InitUIComponents();
			
			InitUI(uiData);
		}

		protected virtual void InitUI(QUIData uiData = null) { }
		protected virtual void RegisterUIEvent() { }
		protected virtual void DestroyUI() { }

		protected void SetUIComponents(IUIComponents uiChild)
		{
			mIComponents = uiChild;
			mIComponents.InitUIComponents();
		}

		protected IUIComponents mIComponents = null;

		protected override void ProcessMsg (int key,QMsg msg) {}
	}
}