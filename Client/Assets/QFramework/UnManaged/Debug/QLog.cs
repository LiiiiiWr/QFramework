/****************************************************************************
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * https://github.com/liangxiegame/QSingleton
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
	using System.Collections.Generic;
	using System.Threading;
	using UnityEngine.Events;

	/// <summary>
	/// 封装日志模块
	/// </summary>
	[QMonoSingletonPath("[Tool]/QLog")]
	public class QLog : QMonoSingleton<QLog>
	{
		/// <summary>
		/// 日志等级，为不同输出配置用
		/// </summary>
		public enum LogLevel
		{
			LOG = 0,
			WARNING = 1,
			ASSERT = 2,
			ERROR = 3,
			MAX = 4,
		}

		/// <summary>
		/// 日志数据类
		/// </summary>
		public class LogData
		{
			public string Log { get; set; }
			public string Track { get; set; }
			public LogLevel Level { get; set; }
		}

		/// <summary>
		/// UI输出日志等级，只要大于等于这个级别的日志，都会输出到屏幕
		/// </summary>
		public LogLevel UIOutputLogLevel = LogLevel.LOG;

		/// <summary>
		/// 文本输出日志等级，只要大于等于这个级别的日志，都会输出到文本
		/// </summary>
		public LogLevel FileOutputLogLevel = LogLevel.MAX;

		/// <summary>
		/// unity日志和日志输出等级的映射
		/// </summary>
		private Dictionary<LogType, LogLevel> mLogTypeLevelDict = null;

		/// <summary>
		/// OnGUI回调
		/// </summary>
		public UnityAction OnGUIEvent = null;

		/// <summary>
		/// 日志输出列表
		/// </summary>
		private List<ILogOutput> logOutputList = null;

		private int mainThreadID = -1;

		/// <summary>
		/// Unity的Debug.Assert()在发布版本有问题
		/// </summary>
		/// <param name="condition">条件</param>
		/// <param name="info">输出信息</param>
		public static void Assert(bool condition, string info)
		{
			if (condition)
				return;
			Debug.LogError(info);
		}

		private void Awake()
		{
			Application.logMessageReceived += LogCallback;
			Application.logMessageReceivedThreaded += LogMultiThreadCallback;

			this.mLogTypeLevelDict = new Dictionary<LogType, LogLevel>
			{
				{LogType.Log, LogLevel.LOG},
				{LogType.Warning, LogLevel.WARNING},
				{LogType.Assert, LogLevel.ASSERT},
				{LogType.Error, LogLevel.ERROR},
				{LogType.Exception, LogLevel.ERROR},
			};

			this.UIOutputLogLevel = LogLevel.LOG;
			this.FileOutputLogLevel = LogLevel.ERROR;
			this.mainThreadID = Thread.CurrentThread.ManagedThreadId;
			this.logOutputList = new List<ILogOutput>
			{
				new QFileLogOutput(),
			};

		}

		void OnGUI()
		{
			if (this.OnGUIEvent != null)
				this.OnGUIEvent();
		}

		void OnDestroy()
		{
			Application.logMessageReceived -= LogCallback;
			Application.logMessageReceivedThreaded -= LogMultiThreadCallback;
		}

		/// <summary>
		/// 日志调用回调，主线程和其他线程都会回调这个函数，在其中根据配置输出日志
		/// </summary>
		/// <param name="log">日志</param>
		/// <param name="track">堆栈追踪</param>
		/// <param name="type">日志类型</param>
		void LogCallback(string log, string track, LogType type)
		{
			if (this.mainThreadID == Thread.CurrentThread.ManagedThreadId)
				Output(log, track, type);
		}

		void LogMultiThreadCallback(string log, string track, LogType type)
		{
			if (this.mainThreadID != Thread.CurrentThread.ManagedThreadId)
				Output(log, track, type);
		}

		void Output(string log, string track, LogType type)
		{
			LogLevel level = this.mLogTypeLevelDict[type];
			LogData logData = new LogData
			{
				Log = log,
				Track = track,
				Level = level,
			};
			for (int i = 0; i < this.logOutputList.Count; ++i)
				this.logOutputList[i].Log(logData);
		}
	}
}