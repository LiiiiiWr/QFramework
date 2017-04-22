using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using UnityEngine;

/// <summary>
///	组合方式实现单例子
/// </summary>
namespace QFramework {

	/// <summary>
	/// class是引用类型
	/// </summary>
	public class QSingletonProperty<T> where T : class,ISingleton
	{
		protected static T mInstance = null;

		public static T Instance
		{
			get {
				if (null == mInstance) {
					mInstance = QSingletonCreator.CreateSingleton<T>();
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