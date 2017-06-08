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
	
	/// <summary>
	/// fsm with string
	/// </summary>
	public class QFSMLite 
	{
		/// <summary>
		/// FSM callfunc
		/// </summary>
		public delegate void FSMCallfunc(params object[] param);

		/// <summary>
		/// FSM state
		/// </summary>
		class QFSMState 
		{		
			public string Name;

			public QFSMState(string name)
			{
				Name = name;
			}

			/// <summary>
			/// Translation for dict
			/// </summary>
			public Dictionary <string,QFSMTranslation> TranslationDict = new Dictionary<string,QFSMTranslation>();
		}

		/// <summary>
		/// FSM Translation
		/// </summary>
		public class QFSMTranslation
		{
			public string FromState;
			public string Name;
			public string ToState;
			public FSMCallfunc OnTranslationCallback;	// 回调函数

			public QFSMTranslation(string fromState,string name, string toState,FSMCallfunc onTranslationCallback)
			{
				FromState 	= fromState;
				ToState   	= toState;
				Name 		= name;
				OnTranslationCallback = onTranslationCallback;
			}
		}

		/// <summary>
		/// current state
		/// </summary>
		string mCurState;

		public string State 
		{
			get { return mCurState; }
		}

		/// <summary>
		/// fsm for dict
		/// </summary>
		Dictionary <string,QFSMState> mStateDict = new Dictionary<string,QFSMState>();

		/// <summary>
		/// add state
		/// </summary>
		/// <param name="name"></param>
		public void AddState(string name)
		{
			mStateDict [name] = new QFSMState(name);
		}

		/// <summary>
		/// add translation
		/// </summary>
		/// <param name="fromState"></param>
		/// <param name="name"></param>
		/// <param name="toState"></param>
		/// <param name="callfunc"></param>
		public void AddTranslation(string fromState,string name,string toState,FSMCallfunc callfunc)
		{
			mStateDict [fromState].TranslationDict [name] = new QFSMTranslation (fromState, name, toState, callfunc);
		}

		/// <summary>
		/// run fsm
		/// </summary>
		/// <param name="name"></param>
		public void Start(string name)
		{
			mCurState = name;
		}

		/// <summary>
		/// process event
		/// </summary>
		/// <param name="name"></param>
		/// <param name="param"></param>
		public void HandleEvent(string name,params object[] param)
		{	
			if (mCurState != null && mStateDict[mCurState].TranslationDict.ContainsKey(name)) 
			{
				QFSMTranslation tempTranslation = mStateDict [mCurState].TranslationDict [name];
				tempTranslation.OnTranslationCallback (param);
				mCurState =  tempTranslation.ToState;
			}
		}

		/// <summary>
		/// Clear 
		/// </summary>
		public void Clear()
		{
			mStateDict.Clear ();
		}
	}
}