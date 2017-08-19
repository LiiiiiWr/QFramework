/****************************************************************************
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
	using UnityEngine;

	/// <summary>
	/// Transform's Util/This Extension
	/// </summary>
	public static class TransformUtil
	{
		public static void LocalIdentity(this Transform selfTrans)
		{
			selfTrans.localPosition = Vector3.zero;
			selfTrans.localRotation = Quaternion.identity;
			selfTrans.localScale = Vector3.one;
		}

		public static void SetLocalPosX(this Transform selfTrans, float x)
		{
			var localPos = selfTrans.localPosition;
			localPos.x = x;
			selfTrans.localPosition = localPos;
		}

		public static void SetLocalPosY(this Transform selfTrans,float y)
		{
			var localPos = selfTrans.localPosition;
			localPos.y = y;
			selfTrans.localPosition = localPos;
		}

		public static void SetLocalPosZ(this Transform selfTrans, float z)
		{
			var localPos = selfTrans.localPosition;
			localPos.z = z;
			selfTrans.localPosition = localPos;
		}

		public static void SetLocalScale(this Transform selftans,float xyz)
		{
			selftans.transform.localScale = Vector3.one * xyz;
		}
	}
}