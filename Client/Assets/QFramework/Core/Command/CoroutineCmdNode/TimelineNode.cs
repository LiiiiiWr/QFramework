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
	/// 时间轴执行节点
	/// </summary>
	public class TimelineNode : ICoroutineCmdNode
	{
		float mCurTime = 0;
		int mExecutedNodeCount = 0;
		int mAllNodeCount;
		public MonoBehaviour CoroutineBehaviour;
		public QVoidDelegate.WithVoid OnTimelineBeganCallback = null;
		public QVoidDelegate.WithVoid OnTimelineEndedCallback = null;
		public QVoidDelegate.WithGeneric<string> OnKeyEventsReceivedCallback=null;

		public class TimelinePair
		{
			public float Time;
			public ICoroutineCmdNode Node;
			public TimelinePair(float time,ICoroutineCmdNode node)
			{
				Time = time;
				Node = node;
			}
		}

		public Queue<TimelinePair> TimelineQueue = new Queue<TimelinePair>();

		public IEnumerator Execute()
		{
			mAllNodeCount = TimelineQueue.Count;
			if (OnTimelineBeganCallback != null) 
			{
				OnTimelineBeganCallback ();
			}

			while (TimelineQueue.Count > 0) 
			{
				TimelinePair nodePair = TimelineQueue.Dequeue ();

				while(mCurTime < nodePair.Time)
				{
					mCurTime += Time.deltaTime;
				    yield return 0;
				}
				
				CoroutineBehaviour.StartCoroutine(ExecuteNode(nodePair.Node));
			}

			while (mExecutedNodeCount != mAllNodeCount) 
			{
				//Debug.Log (mExecutedNodeCount + ":" + mAllNodeCount);
			    yield return 0;
			}

			if (OnTimelineEndedCallback != null) 
			{
				OnTimelineEndedCallback ();
			}
		}

		public IEnumerator ExecuteNode(ICoroutineCmdNode node)
		{
			yield return node.Execute ();
			mExecutedNodeCount++;
		}

		public TimelineNode(MonoBehaviour coroutineBehaviour,params TimelinePair[] pairs)
		{
			CoroutineBehaviour = coroutineBehaviour;

			foreach (var pair in pairs) 
			{
				TimelineQueue.Enqueue (pair);
			}
		}
	}
}