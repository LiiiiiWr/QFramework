/****************************************************************************
 * Copyright (c) 2016 qianmo
 * Copyright (c) 2017 liangxie
 *
 * https://github.com/QianMo/Unity-Design-Pattern/blob/master/Assets/Creational%20Patterns/Singleton%20Pattern/Structure/SingletonStructure.cs
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework.Example 
{
	public class PatternSingleton : MonoBehaviour 
	{
		void Start () 
		{
			Singleton singleton = Singleton.Instance ();
		}
	}

	/// <summary>
	/// The 'Singleton' class
	/// </summary>
	class Singleton
	{
		private static Singleton mInstance;

		// Constructor is 'protected'
		protected Singleton()
		{
		}

		public static Singleton Instance()
		{
			// Uses lazy initialization.
			// Note: this is not thread safe.
			if (null == mInstance) 
			{
				mInstance = new Singleton ();
			}

			return mInstance;
		}
	}
}