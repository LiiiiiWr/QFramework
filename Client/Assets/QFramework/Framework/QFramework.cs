﻿/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
****************************************************************************/

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

		#region 全局生命周期回调
		public QVoidDelegate.WithVoid OnUpdateEvent = delegate{};
		public QVoidDelegate.WithVoid OnFixedUpdateEvent = delegate{};
		public QVoidDelegate.WithVoid OnLatedUpdateEvent = delegate{};
		public QVoidDelegate.WithVoid OnGUIEvent = delegate {};
		public QVoidDelegate.WithVoid OnDestroyEvent = delegate {};
		public QVoidDelegate.WithVoid OnApplicationQuitEvent = delegate {};

		void Update()
		{
			if (OnUpdateEvent != null)
				OnUpdateEvent();
		}

		void FixedUpdate()
		{
			if (OnFixedUpdateEvent != null)
				OnFixedUpdateEvent ();

		}

		void LatedUpdate()
		{
			if (OnLatedUpdateEvent != null)
				OnLatedUpdateEvent ();
		}

		void OnGUI()
		{
			if (OnGUIEvent != null)
				OnGUIEvent();
		}

        protected override void OnDestroy() 
		{
			QMonoSingletonProperty<Framework>.Dispose ();

			if (OnDestroyEvent != null)
				OnDestroyEvent();
		}

		void OnApplicationQuit()
		{
			if (OnApplicationQuitEvent != null)
				OnApplicationQuitEvent();
		}
		#endregion
	}
}
