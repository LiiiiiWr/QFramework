/****************************************************************************
 * Copyright (c) 2016 qianmo
 * Copyright (c) 2017 liangxie
 *
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * https://github.com/QianMo/Unity-Design-Pattern/blob/master/Assets/Creational%20Patterns/Builder%20Pattern/Structure/BuilderStructure.cs
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace QFramework.Example 
{
	public class TestQSingleton : MonoBehaviour 
	{
		// Use this for initialization
		void Start () 
		{
			var deviceMgr = DeviceMgr.Instance;
			var spriteMgr = SpriteMgr.Instance;
			var textureManager = TextureManager.Instance;
			var imageManager = ImageManager.Instance;
		}
	}
		
	/// <summary>
	/// 通过继承实现的单例
	/// </summary>
	public class DeviceMgr : QSingleton<DeviceMgr>
	{
		protected DeviceMgr()
		{
			Debug.Log ("DeviceMgr ctor");
		}

		public override void OnSingletonInit()
		{
			Debug.Log ("DeviceMgr Init");
		}
	}

	/// <summary>
	/// 不通过继承实现的单例
	/// </summary>
	public class SpriteMgr : ScriptableObject,ISingleton
	{
		protected SpriteMgr()
		{
			Debug.Log ("Sprite Mgr ctor");
		}

		public static SpriteMgr Instance
		{
			get 
			{
				return QSingletonProperty<SpriteMgr>.Instance;
			}
		}

		public void OnSingletonInit()
		{
			Debug.Log ("SpriteMgr Init");
		}
	}

	/// <summary>
	/// 继承MonoBehaviour的单例
	/// </summary>
	public class TextureManager : QMonoSingleton<TextureManager>
	{
		public override void OnSingletonInit()
		{
			Debug.Log ("TextureManager Init");
		}
	}

	/// <summary>
	/// 无需继承的MonoBehaviour单例
	/// </summary>
	[QMonoSingletonAttribute("[Image]/ImageManager")]
	public class ImageManager : MonoBehaviour,ISingleton
	{
		public static ImageManager Instance
		{
			get 
			{
				return QMonoSingletonProperty<ImageManager>.Instance;
			}
		}

		public void OnSingletonInit()
		{
			Debug.Log ("ImageManager Init");
		}
	}
}