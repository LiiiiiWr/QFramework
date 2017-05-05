using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using QFramework;

namespace QFramework.Example {


	public enum EventSystemExampleEvent
	{
		RECEIVE_MSG_FROM_OTHER_OBJECT
	}
	/// <summary>
	/// 轻量级消息机制
	/// </summary>
	public class EventSystemExample : MonoBehaviour 
	{

		void Awake() {
			/// <summary>
			/// 接收消息
			/// 需要实现IMsgReceiver接口
			/// </summary>
			QEventSystem.Instance.Register(EventSystemExampleEvent.RECEIVE_MSG_FROM_OTHER_OBJECT,ReceveMsg);
		}

		void ReceveMsg(int key,params object[] paramList)
		{
			Debug.Log("ReceiveMsg");
			foreach(object msgContentItem in paramList) {
				Debug.Log(msgContentItem);
			}
		}
			
		/// <summary>
		/// 发送消息
		/// 需要实现IMsgSender接口
		/// </summary>
		void OnGUI() {
			if (GUI.Button (new Rect (200, 200, 200, 100), "Send Msg")) {
				QEventSystem.Instance.Send(EventSystemExampleEvent.RECEIVE_MSG_FROM_OTHER_OBJECT,new object[]{ "1", "2", 123});
			}
		}


		void OnDestroy() {
			QEventSystem.Instance.UnRegister (EventSystemExampleEvent.RECEIVE_MSG_FROM_OTHER_OBJECT,ReceveMsg);
		}
	}
}

