﻿/****************************************************************************
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

using System;
using System.Collections.Generic;

namespace QFramework 
{
	/// <summary>
	/// manager基类
	/// </summary>
	public abstract class QMgrBehaviour : QMonoBehaviour 
	{
		private readonly QEventSystem mEventSystem = ObjectPool<QEventSystem>.Instance.Allocate();

		protected int mMgrId = 0;

		protected abstract void SetupMgrId ();

		protected override void SetupMgr ()
		{
			mCurMgr = this;
		}
			
		protected QMgrBehaviour() 
		{
			SetupMgrId ();
		}

		public void RegisterEvents<T>(List<T> eventIds,OnEvent process) where T: IConvertible
		{
			for (int i = 0;i < eventIds.Count;i++)
			{
				RegisterEvent(eventIds[i],process);
			}
		}

		public void RegisterEvent<T>(T msgId,OnEvent process) where T:IConvertible
		{
			mEventSystem.Register (msgId, process);
		}

		public void UnRegisterEvents(List<ushort> msgs,OnEvent process)
		{
			for (int i = 0;i < msgs.Count;i++)
			{
				UnRegistEvent(msgs[i],process);
			}
		}

		public void UnRegistEvent(int msgEvent,OnEvent process)
		{
			mEventSystem.UnRegister (msgEvent, process);
		}

		public override void SendMsg(QMsg msg)
		{
			if (msg.GetMgrID() == mMgrId)
			{
				Process(msg.msgId,msg);
			}
			else 
			{
				QMsgCenter.SendMsg(msg);
			}
		}

		// 来了消息以后,通知整个消息链
		protected override void ProcessMsg(int eventId,QMsg msg)
		{
			mEventSystem.Send(msg.msgId,msg);
		}
	}
}