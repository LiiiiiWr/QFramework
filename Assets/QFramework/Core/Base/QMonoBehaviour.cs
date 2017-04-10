using UnityEngine;
using System.Collections;

namespace QFramework {


	/// <summary>
	/// 可以根据Futile的QNode来写,添加消息机制,替代SendMessage
	/// </summary>
	public abstract class QMonoBehaviour : MonoBehaviour {

		public void Process (int key, params object[] param)  {
			ProcessMsg(key,param[0] as QMsg);
		}

		protected abstract void ProcessMsg (int key,QMsg msg);

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

		private Transform mCachedTrans;
		private GameObject mCachedGameObj;

		public void Show()
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

		public void Hide()
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

		public void RegisterSelf(ushort[] msgs = null)
		{
			if (null != msgs) {
				mMsgIds = msgs;
			}
			mCurMgr.RegisterMsgs(mMsgIds,this.Process);
		}

		public void UnRegisterSelf(ushort[] msg)
		{
			mCurMgr.UnRegisterMsgs(mMsgIds,this.Process);
		}

		public void SendMsg(QMsg msg)
		{
			mCurMgr.SendMsg(msg);
		}

		public ushort[] mMsgIds;

		void OnDestroy()
		{
			if (mMsgIds != null)
			{
				UnRegisterSelf(mMsgIds);
			}
		}

	}
}
