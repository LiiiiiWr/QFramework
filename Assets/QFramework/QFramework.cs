

namespace QFramework 
{
	/// <summary>
	/// 全局唯一继承于MonoBehaviour的单例类，保证其他公共模块都以App的生命周期为准
	/// </summary>
	public class Framework : QMgrBehaviour,ISingleton
	{

		protected override void SetupMgrId ()
		{
			mMgrId = 0;
		}

		protected override void SetupMgr ()
		{

		}

		public void OnSingletonInit()
		{

		}

		/// <summary>
		/// 组合的方式实现单例的模板
		/// </summary>
		/// <value>The instance.</value>
		public static Framework Instance 
		{
			get { return QMonoSingletonProperty<Framework>.Instance; }
		}
			
		private Framework() {}

		static bool mIsApplicationQuit = false;
		public static bool IsApplicationQuit 
		{
			get {
				return mIsApplicationQuit;
			}
		}

		#region 全局生命周期回调
		public QVoidDelegate.WithVoid OnUpdateCallback = delegate{};
		public QVoidDelegate.WithVoid OnFixedUpdateCallback = delegate{};
		public QVoidDelegate.WithVoid OnLatedUpdateCallback = delegate{};
		public QVoidDelegate.WithVoid OnGUICallback = delegate {};
		public QVoidDelegate.WithVoid OnDestroyCallback = delegate {};
		public QVoidDelegate.WithVoid OnApplicationQuitCallback = delegate {};

		void Update()
		{
			if (this.OnUpdateCallback != null)
				this.OnUpdateCallback();
		}

		void FixedUpdate()
		{
			if (this.OnFixedUpdateCallback != null)
				this.OnFixedUpdateCallback ();

		}

		void LatedUpdate()
		{
			if (this.OnLatedUpdateCallback != null)
				this.OnLatedUpdateCallback ();
		}

		void OnGUI()
		{
			if (this.OnGUICallback != null)
				this.OnGUICallback();
		}

		protected  void OnDestroy() 
		{
			QMonoSingletonProperty<Framework>.Dispose ();

			if (this.OnDestroyCallback != null)
				this.OnDestroyCallback();
		}

		void OnApplicationQuit()
		{
			mIsApplicationQuit = true;
			if (this.OnApplicationQuitCallback != null)
				this.OnApplicationQuitCallback();
		}
		#endregion
	}
}
