

namespace ToDoList 
{
	using QFramework;
	using SCFramework;
	
	[QMonoSingletonAttribute("[App]/AppMgr")]
	public class AppMgr : AbstractModuleMgr, ISingleton
	{
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

		public void Dispose()
		{
			QMonoSingletonProperty<AppMgr>.Dispose();
		}

		protected override void OnActorStart()
		{
			StartProcessModule module = AddMonoCom<StartProcessModule>();

			module.SetFinishListener(OnStartProcessFinish);
		}

		protected void OnStartProcessFinish()
		{
			QUIManager.Instance.OpenUI<UIToDoListPage> (QUILevel.Common);
		}
	}
}