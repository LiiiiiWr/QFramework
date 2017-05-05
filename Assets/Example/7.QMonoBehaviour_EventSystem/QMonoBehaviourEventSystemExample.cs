using UnityEngine;
using System.Collections;

using QFramework;

namespace QFramework.Example {

	/// <summary>
	/// 定义管理器,转发消息用的
	/// </summary>
	public class ExampleManager : QMgrBehaviour,ISingleton {

		public static ExampleManager Instance {
			get {
				return QMonoSingletonProperty<ExampleManager>.Instance;
			}
		}

		protected override void SetupMgrId ()
		{
			mMgrId = (ushort)QMgrID.Framework;
		}

		public void OnSingletonInit()
		{

		}
	}
		
	/// <summary>
	/// 定义事件
	/// </summary>
	public enum GameEvent {
		Begin = QMgrID.Framework,
		SayHello,
		End,
	}

	public class QMonoBehaviourEventSystemExample : QMonoBehaviour{

		IEnumerator Start() {

			// 注册消息
			RegisterSelf (new ushort[] {
				(ushort)GameEvent.SayHello
			});

			// 给自己发送消息
			yield return new WaitForSeconds (0.5f);
			this.SendMsg (new QMsg ((ushort)GameEvent.SayHello));
			yield return new WaitForSeconds (0.5f);
			this.SendMsg (new QMsg ((ushort)GameEvent.SayHello));
			yield return new WaitForSeconds (0.5f);
			this.SendMsg (new QMsg ((ushort)GameEvent.SayHello));
		}

		protected override void SetupMgr ()
		{
			mCurMgr = ExampleManager.Instance;
		}

		/// <summary>
		/// 处理消息
		/// </summary>
		protected override void ProcessMsg (int key,QMsg msg)
		{
			switch (msg.msgId) {
				case (ushort)GameEvent.SayHello:
					Debug.Log ("Say Hello");
					break;
			}
		}
			
	}

}