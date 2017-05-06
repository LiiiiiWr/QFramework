using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using SCFramework;
using QAssetBundle;

namespace ToDoList {
	[QMonoSingletonAttribute("[App]/AppMgr")]
	public class AppMgr : AbstractModuleMgr, ISingleton
	{
		private static AppMgr mInstance;

		public static AppMgr Instance
		{
			get  { 	return QMonoSingletonProperty<AppMgr>.Instance; }
		}

		public void InitAppMgr()
		{
			Log.i("Init[GameMgr]");
		}

		public void OnSingletonInit()
		{

		}

		protected override void OnActorStart()
		{
			StartProcessModule module = AddMonoCom<StartProcessModule>();

			module.SetFinishListener(OnStartProcessFinish);
		}

		protected void OnStartProcessFinish()
		{
			QUIManager.Instance.OpenUI<UIToDoListPage> (QUILevel.Common, UIPREFAB.BUNDLE_NAME);
		}
	}
}