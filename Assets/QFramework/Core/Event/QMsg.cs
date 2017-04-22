using UnityEngine;
using System.Collections;
using QFramework;

/// <summary>
/// 消息体
/// </summary>
public class QMsg   {

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

public class QSoundMsg : QMsg {
	public bool soundOn;

	public QSoundMsg(ushort msgId,bool soundOn) :base(msgId)
	{
		this.soundOn = soundOn;
	}
}

public class QStrMsg  :QMsg {
	public string strMsg;

	public QStrMsg(ushort msgId,string strMsg) :base(msgId) {
		this.strMsg = strMsg;
	}
}
	

public class MsgTransform :QMsg
{
	public Transform value;

	public MsgTransform(ushort msgId,Transform tmpTrans) : base(msgId)
	{
		this.value = tmpTrans;
	}
}