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

namespace QFramework 
{
	using UnityEngine;
	using System.Collections.Generic;
	using UnityEngine.UI;
	
	public enum QUILevel
	{
		Bg,          			//背景层UI
		Common,              	//普通层UI
		PopUI,                 	//弹出层UI
		Const,               	//持续存在层UI
		Toast,               	//对话框层UI
		Forward,              	//最高UI层用来放置UI特效和模型
	}

	//// <summary>
	/// UGUI UI界面管理器
	/// </summary>
	public class QUIManager : QMgrBehaviour,ISingleton
	{ 
		[SerializeField]
		Dictionary<string,QUIBehaviour> mAllUI = new Dictionary<string, QUIBehaviour> ();

		[SerializeField] Transform mBgTrans;
		[SerializeField] Transform mCommonTrans;
		[SerializeField] Transform mPopUITrans;
		[SerializeField] Transform mConstTrans;
		[SerializeField] Transform mToastTrans;
		[SerializeField] Transform mForwardTrans;
		[SerializeField] Camera mUICamera;
		[SerializeField] Canvas mCanvas;
		[SerializeField] CanvasScaler mCanvasScaler;

		static GameObject mObj;
		public static QUIManager Instance 
		{
			get 
			{
				if (!mObj)
				{
					mObj = GameObject.Find ("QUIManager");
					if (!mObj) 
					{
						mObj = Instantiate (Resources.Load ("QUIManager")) as GameObject;
					}
					mObj.name = "QUIManager";
				}

				return QMonoSingletonProperty<QUIManager>.Instance;
			}
		}
		
		public Canvas RootCanvas
		{
			get { return mCanvas; }
		}
		
		public Camera UICamera
		{
			get { return mUICamera; }
		}
		
		public void OnSingletonInit() {}

		public void Dispose()
		{
			QMonoSingletonProperty<QUIManager>.Dispose();
		}

		void Awake() { DontDestroyOnLoad (this);}

		public void SetResolution(int width,int height) 
		{
			mCanvasScaler.referenceResolution = new Vector2 (width, height);
		}

		public void SetMatchOnWidthOrHeight(float heightPercent) 
		{
			mCanvasScaler.matchWidthOrHeight = heightPercent;
		}

		/// <summary>
		/// Create&ShowUI
		/// </summary>
		public T OpenUI<T>(QUILevel canvasLevel,QUIData uiData = null) where T : QUIBehaviour
		{
			string behaviourName = GetUIBehaviourName<T> ();

			QUIBehaviour ui;
			if (!mAllUI.TryGetValue(behaviourName, out ui))
			{
				ui = CreateUI<T>(canvasLevel, uiData);
			}
			ui.Show();
			return ui as T;
		}

		public Transform Get<T>(string strUIName)
		{
			string strDlg = GetUIBehaviourName<T> ();
			if (mAllUI.ContainsKey(strDlg))
			{
				return mAllUI[strDlg].transform.Find(strUIName);
			}
			else
			{
				Debug.LogError(string.Format("panel={0},ui={1} not exist!", strDlg, strUIName));
			}
			return null;
		}

		/// <summary>
		/// 增加UI层
		/// </summary>
		public T CreateUI<T>(QUILevel level,QUIData initData = null) where T : QUIBehaviour
		{
			string behaviourName = GetUIBehaviourName<T> ();

			QUIBehaviour ui;
			if (mAllUI.TryGetValue(behaviourName, out ui))
			{
				Debug.LogWarning(behaviourName + ": already exist");
				//直接返回，不要再调一次Init(),Init()应该只能调用一次
				return ui as T;
			}
			ResLoader resLoader = ResLoader.Allocate ();

			GameObject	prefab = resLoader.LoadSync (behaviourName) as GameObject;

			GameObject mUIGo = Instantiate (prefab);
			switch (level) {
				case QUILevel.Bg:
					mUIGo.transform.SetParent (mBgTrans);
					break;
				case QUILevel.Common:
					mUIGo.transform.SetParent (mCommonTrans);
					break;
				case QUILevel.PopUI:
					mUIGo.transform.SetParent (mPopUITrans);
					break;
				case QUILevel.Const:
					mUIGo.transform.SetParent (mConstTrans);
					break;
				case QUILevel.Toast:
					mUIGo.transform.SetParent (mToastTrans);
					break;
				case QUILevel.Forward:
					mUIGo.transform.SetParent (mForwardTrans);
					break;
			}

			var uiGoRectTrans = mUIGo.GetComponent<RectTransform> ();
			uiGoRectTrans.offsetMin = Vector2.zero;
			uiGoRectTrans.offsetMax = Vector2.zero;
			uiGoRectTrans.anchoredPosition3D = Vector3.zero;
			uiGoRectTrans.anchorMin = Vector2.zero;
			uiGoRectTrans.anchorMax = Vector2.one;

			mUIGo.transform.localScale = Vector3.one;


			mUIGo.gameObject.name = behaviourName;

			Debug.Log(behaviourName + " Load Success");

			ui = mUIGo.AddComponent<T>();
			mAllUI.Add(behaviourName, ui);
			ui.Init(resLoader,initData);

			return ui as T;
		}

		/// <summary>
		/// 显示UI层
		/// </summary>
		/// <param name="layerName">Layer name.</param>
		public void ShowUI<T>()
		{
			string behaviourName = GetUIBehaviourName<T> ();

			if (mAllUI.ContainsKey(behaviourName))
			{
				mAllUI[behaviourName].Show ();
			}
		}

		/// <summary>
		/// 隐藏UI层
		/// </summary>
		/// <param name="layerName">Layer name.</param>
		public void HideUI<T>()
		{
			string behaviourName = GetUIBehaviourName<T> ();

			if (mAllUI.ContainsKey (behaviourName)) 
			{
				mAllUI [behaviourName].Hide ();
			}
		}

		/// <summary>
		/// 删除所有UI层
		/// </summary>
		public void CloseAllUI()
		{
			foreach (var layer in mAllUI) 
			{
				Destroy (layer.Value);
			}

			mAllUI.Clear ();
		}

		/// <summary>
		/// 删除掉UI
		/// </summary>
		public void CloseUI<T>()
		{
			string behaviourName = GetUIBehaviourName<T> ();

			CloseUI (behaviourName);
		}

		public void CloseUI(string behaviourName)
		{
			QUIBehaviour behaviour = null;

			mAllUI.TryGetValue (behaviourName, out behaviour);

			if (null != behaviour) 
			{
				(behaviour as IUI).Close ();
				mAllUI.Remove (behaviourName);
			}
		}

		/// <summary>
		/// 获取UIBehaviour
		public T GetUI<T>()
		{
			string behaviourName = GetUIBehaviourName<T> ();

			if (mAllUI.ContainsKey (behaviourName)) 
			{
				return mAllUI [behaviourName].GetComponent<T> ();
			}
			return default(T);
		}

		/// <summary>
		/// 获取UI相机
		/// </summary>
		/// <returns></returns>
		public Camera GetUICamera() 
		{
			return mUICamera;
		}

		protected override void SetupMgrId ()
		{
			mMgrId = QMgrID.UI;
		}
		
		/// <summary>
		/// 命名空间对应名字的缓存
		/// </summary>
		private Dictionary<string,string> mFullname4UIBehaviourName = new Dictionary<string, string>();

		private string GetUIBehaviourName<T>() 
		{
			string fullBehaviourName = typeof(T).ToString();
			string retValue = null;

			if (mFullname4UIBehaviourName.ContainsKey (fullBehaviourName)) 
			{
				retValue = mFullname4UIBehaviourName[fullBehaviourName];
			}
			else 
			{
				string[] nameSplits = fullBehaviourName.Split (new char[] { '.' });
				retValue = nameSplits [nameSplits.Length - 1];
				mFullname4UIBehaviourName.Add (fullBehaviourName, retValue);
			}

			return retValue;
		}
	}
}