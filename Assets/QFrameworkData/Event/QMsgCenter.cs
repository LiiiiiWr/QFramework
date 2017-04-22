using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public static partial class QMsgCenter  {

	/// <summary>
	/// 转发消息
	/// </summary>
	private static void ForwardMsg(QMsg msg)
	{
		int tmpId = msg.GetMgrID();

		switch (tmpId)
		{
			case QMgrID.AB:
				break;
			case QMgrID.Sound:
				break;
			case  QMgrID.CharactorManager:
				break;
			case  QMgrID.Framework:
				break;
			case  QMgrID.Network:
				NetManager.Instance.SendMsg (msg);
				break;
			case  QMgrID.NPCManager:
				break;
			case  QMgrID.UI:
				QUIManager.Instance.SendMsg (msg);
				break;
			case QMgrID.Data:
				ToDoList.ToDoListManager.Instance.SendMsg (msg);
				break;
			default:
				break;
		}
	}
}
