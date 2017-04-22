using UnityEngine;
using System;
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

		public QEventSystem EventSystem 
		{
			get 
			{
				return mEventSystem;
			}
		}
			
		protected QMgrBehaviour() {
			SetupMgrId ();
		}

		// mono:要注册的脚本   
		// msgs:每个脚本可以注册多个脚本
		public void RegisterEvents<T>(List<T> eventIds,OnEvent process) where T: IConvertible
		{
			for (int i = 0;i < eventIds.Count;i++)
			{
				RegisterEvent(eventIds[i],process);
			}
		}

		// 根据: msgid
		// node链表
		public void RegisterEvent<T>(T msgId,OnEvent process) where T:IConvertible
		{
			mEventSystem.Register (msgId, process);
		}

		// params 可变数组 参数
		// 去掉一个脚本的若干的消息
		public void UnRegisterEvents(List<ushort> msgs,OnEvent process)
		{
			for (int i = 0;i < msgs.Count;i++)
			{
				UnRegistEvent(msgs[i],process);
			}
		}

		// 释放 中间,尾部。
		public void UnRegistEvent(int msgEvent,OnEvent process)
		{
			mEventSystem.UnRegister (msgEvent, process);
		}

		public override void SendMsg(QMsg msg)
		{
			if ((int)msg.GetMgrID() == mMgrId)
			{
				Process(msg.msgId,msg);
			}
			else 
			{
				QMsgCenter.SendMsg(msg);
			}
		}

		// 来了消息以后,通知整个消息链
		protected override void ProcessMsg(int key,QMsg msg)
		{
			mEventSystem.Send(msg.msgId,msg);
		}
	}
}