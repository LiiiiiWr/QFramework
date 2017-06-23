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
	using System.Collections.Generic;
	
	public class QFSMState
	{
		public QFSMState(ushort name) 
		{
			Name = name;
		}

		public ushort Name;					// 字符串
		public virtual void OnEnter() {}			// 进入状态(逻辑)
		public virtual void OnExit() {}				// 离开状态(逻辑)
		/// <summary>
		/// 存储事件对应的条转
		/// </summary>
		public Dictionary <ushort,QFSMTranslation> TranslationDict = new Dictionary<ushort,QFSMTranslation>();
	}
	
	/// <summary>
	/// translation class
	/// </summary>
	public class QFSMTranslation
	{
		public QFSMState FromState;
		public ushort EventName;
		public QFSMState ToState;

		public QFSMTranslation(QFSMState fromState,ushort eventName, QFSMState toState)
		{
			FromState = fromState;
			ToState   = toState;
			EventName = eventName;
		}
	}

	public class QFSM
	{
		QFSMState mCurState;

		public QFSMState State 
		{
			get { return mCurState; }
		}

		/// <summary>
		/// state dict
		/// </summary>
		Dictionary<ushort, QFSMState> mStateDict = new Dictionary<ushort, QFSMState>();

		/// <summary>
		/// add state
		/// </summary>
		/// <param name="state">State.</param>
		public void AddState(QFSMState state)
		{
			mStateDict.Add (state.Name, state);
		}


		/// <summary>
		/// add transition
		/// </summary>
		/// <param name="translation">Translation.</param>
		public void AddTranslation(QFSMTranslation translation)
		{
			mStateDict [translation.FromState.Name].TranslationDict.Add(translation.EventName, translation);
		}


		/// <summary>
		/// add transition
		/// </summary>
		/// <param name="translation">Translation.</param>
		public void AddTranslation(QFSMState fromState,ushort eventName,QFSMState toState)
		{
			mStateDict [fromState.Name].TranslationDict.Add(eventName, new QFSMTranslation (fromState, eventName, toState));
		}

		/// <summary>
		/// run fsm
		/// </summary>
		/// <param name="state">State.</param>
		public void Start(QFSMState startState)
		{
			mCurState = startState;
			mCurState.OnEnter ();
		}

		/// <summary>
		/// process event
		/// </summary>
		/// <param name="name">Name.</param>
		public void HandleEvent(ushort eventName)
		{	
			if (mCurState != null && mStateDict[mCurState.Name].TranslationDict.ContainsKey(eventName)) 
			{
				QFSMTranslation tempTranslation = mStateDict [mCurState.Name].TranslationDict [eventName];
				tempTranslation.FromState.OnExit ();
				mCurState =  tempTranslation.ToState;
				tempTranslation.ToState.OnEnter ();
			}
		}
			
		/// <summary>
		/// clear all state
		/// </summary>
		public void Clear()
		{
			mStateDict.Clear ();
		}
	}
}