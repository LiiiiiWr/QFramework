using UnityEngine;
using System.Collections;
using QFramework;

public static partial class QMsgCenter 
{
	public static void SendMsg(QMsg tmpMsg)
	{
		ForwardMsg(tmpMsg);
	}
}