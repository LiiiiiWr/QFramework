using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 1.泛型
/// 2.反射
/// 3.抽象类
/// 4.命名空间
/// </summary>
namespace QFramework {
	
	public abstract class QSingleton<T> :ISingleton where T : QSingleton<T>
	{
		protected static T mInstance = null;

		protected QSingleton()
		{
		}

		public static T Instance
		{
			get 
			{
				if (mInstance == null) 
				{
					mInstance = QSingletonCreator.CreateSingleton<T> ();
				}

				return mInstance;
			}
		}

		public static T ResetInstance()
		{
			mInstance = null;
			mInstance = Instance;
			mInstance.OnSingletonInit();
			return mInstance;
		}


		public void Dispose()
		{
			mInstance = null;
		}

		public virtual void OnSingletonInit()
		{
		}
	}
}