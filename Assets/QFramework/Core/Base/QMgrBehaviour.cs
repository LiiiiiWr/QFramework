using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework {

	/// <summary>
	/// manager基类
	/// </summary>
	public abstract class QMgrBehaviour : QMonoBehaviour 
	{

		QEventSystem mEventSystem = ObjectPool<QEventSystem>.Instance.Allocate();

		protected ushort mMgrId = 0;

		protected abstract void SetupMgrId ();

		protected override void SetupMgr ()
		{
			mCurMgr = this;
		}


		protected QMgrBehaviour() {
			SetupMgrId ();
		}

		// mono:要注册的脚本   
		// msgs:每个脚本可以注册多个脚本
		public void RegisterMsgs(ushort[] msgs,OnEvent process)
		{
			for (int i = 0;i < msgs.Length;i++)
			{
				RegisterMsg(msgs[i],process);
			}
		}

		// 根据: msgid
		// node链表
		public void RegisterMsg(int msgId,OnEvent process)
		{
			mEventSystem.Register (msgId, process);
		}

		// params 可变数组 参数
		// 去掉一个脚本的若干的消息
		public void UnRegisterMsgs(ushort[] msgs,OnEvent process)
		{
			for (int i = 0;i < msgs.Length;i++)
			{
				UnRegistMsg(msgs[i],process);
			}
		}

		// 释放 中间,尾部。
		public void UnRegistMsg(ushort msgId,OnEvent process)
		{
			mEventSystem.UnRegister (msgId, process);
		}

		public void SendMsg(QMsg msg)
		{
			if ((ushort)msg.GetMgrID() == mMgrId)
			{
				Process(msg.msgId,msg);
			}
			else 
			{
				QMsgCenter.Instance.SendToMsg(msg);
			}
		}

		// 来了消息以后,通知整个消息链
		protected override void ProcessMsg(int key,QMsg msg)
		{
			mEventSystem.Send(msg.msgId,msg);
		}
	}
}