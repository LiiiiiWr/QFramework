using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace QFramework {

	/// <summary>
	/// 基础类
	/// </summary>
	public abstract class QMonoBehaviour : MonoBehaviour {

		protected void Process (int key, params object[] param)  {
			if (gameObject.activeInHierarchy) 
			{
				ProcessMsg (key, param [0] as QMsg);
				// 兼容之前版本
				ProcessMsg (param [0] as QMsg);
			}
		}

		protected abstract void ProcessMsg (int eventId,QMsg msg);
		/// <summary>
		/// 兼容之前版本
		/// </summary>
		protected virtual void ProcessMsg (QMsg msg) {}

		protected abstract void SetupMgr ();
		private QMgrBehaviour mPrivateMgr = null;
		protected QMgrBehaviour mCurMgr {
			get {
				if (mPrivateMgr == null ) {
					SetupMgr ();
				}
				if (mPrivateMgr == null) {
					Debug.LogError ("没有设置Mgr");
				}

				return mPrivateMgr;
			}

			set {
				mPrivateMgr = value;
			}
		}

		public virtual void Show()
		{
			gameObject.SetActive (true);
			Debug.LogWarning ("On Show:" + name);

			OnShow ();
		}

		/// <summary>
		/// 显示时候用,或者,Active为True
		/// </summary>
		protected virtual void OnShow()
		{
		}

		public virtual void Hide()
		{
			OnHide ();

			gameObject.SetActive (false);
			Debug.LogWarning ("On Hide:" + name);
		}

		/// <summary>
		/// 隐藏时候调用,即将删除 或者,Active为False
		/// </summary>
		protected virtual void OnHide()
		{
			
		}

		protected void RegisterEvent<T>(T eventId) where T:IConvertible
		{
			mEventIds.Add (eventId.ToUInt16 (null));
			mCurMgr.RegisterEvent (eventId, this.Process);
		}

		protected void UnRegisterEvent<T>(T eventId) where T:IConvertible
		{
			mEventIds.Remove (eventId.ToUInt16 (null));
			mCurMgr.UnRegistEvent (eventId.ToInt32(null), this.Process);
		}

		protected void UnRegisterAllEvent()
		{
			if (null != mPrivateEventIds) 
			{
				mCurMgr.UnRegisterEvents (mEventIds, this.Process);
			}
		}

		public virtual void SendMsg(QMsg msg)
		{
			mCurMgr.SendMsg(msg);
		}

		public virtual void SendEvent<T>(T eventId) where T : IConvertible
		{
			mCurMgr.SendMsg(new QMsg(eventId.ToUInt16(null)));
		}

		private List<ushort> mPrivateEventIds = null;

		private List<ushort> mEventIds {
			get {
				if (null == mPrivateEventIds) 
				{
					mPrivateEventIds = new List<ushort> ();
				}
				return mPrivateEventIds;
			}
		}

		void OnDestroy()
		{
			OnBeforeDestroy ();
			if (!Framework.IsApplicationQuit) {
				UnRegisterAllEvent();
			}
		}

		protected virtual void OnBeforeDestroy() {}


		/// <summary>
		/// Registers the self.
		/// </summary>
		[Obsolete("RegisterSelf is depreciate,please use RegisterEvent instead")]
		protected void RegisterSelf(ushort[] msgs)
		{
			if (null != msgs) {
				mEventIds.AddRange (msgs);
			}
			mCurMgr.RegisterEvents(mEventIds,this.Process);
		}

		[Obsolete("UnRegisterSelf is depreciate,please use UnRegisterEvent instead")]
		protected void UnRegisterSelf()
		{
			UnRegisterAllEvent ();
		}
	}
}
