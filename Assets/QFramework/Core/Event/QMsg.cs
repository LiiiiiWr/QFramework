using UnityEngine;
using System.Collections;
using QFramework;

/// <summary>
/// 消息体
/// </summary>
public class QMsg   
{
	// 表示 65535个消息 占两个字节
	public int msgId;

	public int GetMgrID()
	{
		int tmpId = msgId / QMsgSpan.Count;

		return (int)(tmpId * QMsgSpan.Count);
	}

	public QMsg() {}

	public QMsg(ushort msg)
	{
		msgId = msg;
	}
		
}