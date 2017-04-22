using UnityEngine;

/// <summary>
/// 需要使用组合方式实现Unity生命周期的单例模式
/// </summary>
namespace QFramework {

	public abstract class QMonoSingletonProperty<T> where T : MonoBehaviour,ISingleton
	{
		protected static T mInstance = null;

		public static T Instance
		{
			get {
				if (mInstance == null) {
					mInstance = QSingletonCreator.CreateMonoSingleton<T> ();
				}

				return mInstance;

			}
		}
			
		public static void Dispose()
		{
			mInstance = null;
		}
	}

}