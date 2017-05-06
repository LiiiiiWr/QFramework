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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
	/// <summary>
	/// 序列执行节点
	/// </summary>
	public class SequenceNode : ICoroutineCmdNode
	{
		public QVoidDelegate.WithVoid OnBeganCallback = null;
		public QVoidDelegate.WithVoid OnEndedCallback = null;
		public Queue<ICoroutineCmdNode> NodeQueue = new Queue<ICoroutineCmdNode>();
		public bool Completed = false;
		public IEnumerator Execute ()
		{
			if (null != OnBeganCallback) {
				OnBeganCallback ();
			}

			while (NodeQueue.Count > 0) {
				var node = NodeQueue.Dequeue ();
				yield return node.Execute ();
			}

			if (null != OnEndedCallback) {
				OnEndedCallback ();
			}
			Completed = true;
		}

		public SequenceNode(params ICoroutineCmdNode[] nodes)
		{
			for (int i = 0; i < nodes.Length; i++) {
				NodeQueue.Enqueue (nodes[i]);
			}
		}
	}

}