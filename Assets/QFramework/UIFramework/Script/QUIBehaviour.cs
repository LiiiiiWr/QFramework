using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using QFramework;
using UnityEngine.UI;

namespace QFramework {

	/// <summary>
	/// 每个UIBehaviour对应Data
	/// </summary>
	public class QUIData {
		
	}

	public abstract class QUIBehaviour : QMonoBehaviour,IUI {

		ResLoader mResLoader = null;

		protected override void SetupMgr ()
		{
			mCurMgr = QUIManager.Instance;
		}

		void OnDestroy()
		{
			DestroyUI();
			if (mUIComponentsDic != null)
			{
				mUIComponentsDic.Clear();
			}
			if (mIComponents != null)
			{
				mIComponents.Clear();
			}

			if (mMsgIds != null)
			{
				UnRegisterSelf(mMsgIds);
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
				GameObject.Destroy (gameObject);
			}
			mResLoader.ReleaseAllRes ();
			mResLoader.Recycle2Cache ();
			mResLoader = null;
		}


		public void CloseSelf() {
			QUIManager.Instance.CloseUI (this.name);
		}

		/// <summary>
		/// 关闭
		/// </summary>
		protected virtual void OnClose() {

		}


		public Transform Get(string behaivourName)
		{
			if (mUIComponentsDic.ContainsKey(behaivourName))
			{
				return mUIComponentsDic[behaivourName];
			}
			return null;
		}

		public void SetVisible(bool visible)
		{
			this.gameObject.SetActive(visible);
			if(visible)
			{
				OnShow();
			}
		}

		void InnerInit(QUIData uiData = null)
		{
			FindAllCanHandleWidget(this.transform);
			mIComponents = QUIFactory.Instance.CreateUIComponents(this.name);
			mIComponents.InitUIComponents();
			InitUI(uiData);
		}

		protected virtual void InitUI(QUIData uiData = null) { }
		protected virtual void RegisterUIEvent() { }
		protected virtual void DestroyUI() { }

		protected void SetUIBehavior(IUIComponents uiChild)
		{
			mIComponents = uiChild;
			mIComponents.InitUIComponents();
		}
			

		void FindAllCanHandleWidget(Transform trans)
		{
			for (int i = 0; i < trans.childCount; i++)
			{
				Transform childTrans = trans.GetChild(i);
				QUIMark uiMark = childTrans.GetComponent<QUIMark>();
				if (null != uiMark)
				{
					if (mUIComponentsDic.ContainsKey(childTrans.name))
					{
						Debug.LogError("Repeat Id: " + childTrans.name);
					}
					else
					{
						mUIComponentsDic.Add(childTrans.name, childTrans);
					}
				}
				FindAllCanHandleWidget(childTrans);
			}
		}

		protected virtual bool mUnloadAll
		{
			get { return false; }
		}
		protected IUIComponents mIComponents = null;
		private Dictionary<string, Transform> mUIComponentsDic = new Dictionary<string, Transform>();

		protected override void ProcessMsg (int key,QMsg msg)
		{
			throw new System.NotImplementedException ();
		}


		public void RegisterSelf(QMonoBehaviour behaviour,ushort[] msgs = null)
		{
			if (null != msgs) {
				mMsgIds = msgs;
				QUIManager.Instance.RegisterMsgs (msgs, Process);
			} else {
				QUIManager.Instance.RegisterMsgs (mMsgIds, Process);
			}
		}

		public void UnRegisterSelf()
		{
			QUIManager.Instance.UnRegisterMsgs(mMsgIds,Process);
		}

		public void SendMsg(QMsg msg)
		{
			QUIManager.Instance.SendMsg(msg);
		}
			
	}
}