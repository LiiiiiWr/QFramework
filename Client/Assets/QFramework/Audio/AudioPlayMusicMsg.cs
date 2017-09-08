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
	using System;

	public enum VolumeLevel
	{
		Max,
		Normal,
		Min,
	}
	/// <summary>
	/// 播放音乐用的消息
	/// </summary>
	public class AudioMsgPlayMusic : QMsg
	{
		public string musicName;
		public bool loop = true;
		public VolumeLevel volumeLevel = VolumeLevel.Normal;

		/// <summary>
		/// 是否受MusicOn(bool)管理
		/// </summary>
		public bool allowMusicOff = true;

		public QVoidDelegate.WithVoid onMusicBeganCallback;
		public QVoidDelegate.WithVoid onMusicEndedCallback;

		public AudioMsgPlayMusic(string musicName, bool loop = true, bool allowMusicOff = true,
			QVoidDelegate.WithVoid onMusicBeganCallback = null,
			QVoidDelegate.WithVoid onMusicEndedCallback = null) : base((ushort) AudioEvent.PlayMusic)
		{
			this.musicName = musicName;
			this.loop = loop;
			this.allowMusicOff = allowMusicOff;
			this.onMusicBeganCallback = onMusicBeganCallback;
			this.onMusicEndedCallback = onMusicEndedCallback;
		}

		[Obsolete("弃用 yeah!")]
		public AudioMsgPlayMusic(ushort msgId, string musicName, bool loop = true,
			QVoidDelegate.WithVoid onMusicBeganCallback = null,
			QVoidDelegate.WithVoid onMusicEndedCallback = null) : base(msgId)
		{
			this.musicName = musicName;
			this.loop = loop;
			this.onMusicBeganCallback = onMusicBeganCallback;
			this.onMusicEndedCallback = onMusicEndedCallback;
		}
	}


	public class AudioMsgStopMusic : QMsg
	{
		public AudioMsgStopMusic() : base((ushort)AudioEvent.StopMusic){}
	}
}