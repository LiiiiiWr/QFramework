using UnityEngine;
using System.Collections;

public class QMsgSpan {
	public const int Count = 3000;
}

public class QMgrID
{
	public const int Framework = 0;
	public const int UI = QMsgSpan.Count; 			// 3000
	public const int Sound = QMsgSpan.Count * 2;		// 6000
	public const int NPCManager = QMsgSpan.Count * 3;
	public const int CharactorManager = QMsgSpan.Count * 4;
	public const int AB = QMsgSpan.Count * 5;
	public const int Network = QMsgSpan.Count * 6;
	public const int Data = QMsgSpan.Count * 7;
	public const int Global = QMsgSpan.Count * 8;
}
