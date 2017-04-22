using UnityEngine;
using System;
/// <summary>
/// 需要使用MonoBehaviour的单例模式
/// </summary>
namespace QFramework {

	public abstract class QMonoSingleton<T> : MonoBehaviour,ISingleton where T : QMonoSingleton<T>
	{
		protected static T mInstance = null;
		static object mLock = new object();

		public static T Instance
		{
			get {
				lock (mLock) 
				{
					if (mInstance == null) {
						mInstance = QSingletonCreator.CreateMonoSingleton<T> ();
					}
				}
				return mInstance;
			}
		}

		public virtual void OnSingletonInit()
		{

		}

		protected virtual void OnDestroy()
		{
			mInstance = null;
		}
	}
}