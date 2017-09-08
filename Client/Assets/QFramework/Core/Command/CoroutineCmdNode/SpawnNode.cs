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
 * 
 ****************************************************************************/

namespace QFramework 
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	/// <summary>
	/// 并发执行的协程
	/// </summary>
	public class SpawnNode :ICoroutineCmdNode 
	{
		int mExcutedNodeCount = 0;

		public QVoidDelegate.WithVoid OnBeganCallback = null;
		public QVoidDelegate.WithVoid OnEndedCallback = null;
		public MonoBehaviour CoroutineBehaviour;

		public List<ICoroutineCmdNode> NodeLists = new List<ICoroutineCmdNode>();

		public IEnumerator Execute()
		{
			if (null != OnBeganCallback) 
			{
				OnBeganCallback ();
			}

			foreach (var node in NodeLists) 
			{
				CoroutineBehaviour.StartCoroutine(ExecuteNode(node));
			}

			while (mExcutedNodeCount != NodeLists.Count) 
			{
				yield return new WaitForEndOfFrame ();
			}

			if (null != OnEndedCallback) 
			{
				OnEndedCallback ();
			}

			CoroutineBehaviour = null;
			NodeLists.Clear ();
		}

		IEnumerator ExecuteNode(ICoroutineCmdNode node)
		{
			yield return node.Execute();
			mExcutedNodeCount++;
		}

		public SpawnNode(MonoBehaviour coroutineBehaviour,params ICoroutineCmdNode[] nodes)
		{
			CoroutineBehaviour = coroutineBehaviour;
			NodeLists.AddRange (nodes);
		}
	}
}