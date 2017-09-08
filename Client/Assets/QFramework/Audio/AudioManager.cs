/****************************************************************************
 * Copyright (c) 2017 snowcold
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

/// <summary>
/// 音频管理工具
/// </summary>
namespace QFramework
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	
	#region 消息id定义

	public enum AudioEvent
	{
		Began = QMgrID.Audio,
		SoundSwitch,
		MusicSwitch,
		VoiceSwitch,
		SetSoundVolume,
		SetMusicVolume,
		SetVoiceVolume,
		PlayMusic,
		PlaySound,
		PlayVoice,
		PauseMusic,
		StopMusic,
		PauseVoice,
		StopVoice,
		StopAllSound,
		PlayNode,
		Ended
	}

	#endregion

	#region 消息体定义

	public class AudioMsgWithBool : QMsg
	{
		public bool on;

		public AudioMsgWithBool(ushort msgId, bool on) : base(msgId)
		{
			this.on = on;
		}
	}

	public class AduioMsgPlayVoiceLoop : QMsg
	{
		public string VoiceName;
		public QVoidDelegate.WithVoid OnVoiceBeganCallback;
		public QVoidDelegate.WithVoid OnVoiceEndedCallback;
	}

	public class AudioMsgWithNode : QMsg
	{
		public ICoroutineCmdNode Node;

		public AudioMsgWithNode(ushort msgId, ICoroutineCmdNode node) : base(msgId)
		{
			Node = node;
		}
	}

	#endregion


	public class AudioSoundNode : ICoroutineCmdNode
	{
		public string SoundName;
		public QVoidDelegate.WithVoid OnSoundBeganCallback = null;
		public QVoidDelegate.WithVoid OnSoundEndedCallback = null;

		public AudioSoundNode(string voiceName)
		{
			SoundName = voiceName;
		}

		public IEnumerator Execute()
		{
			Debug.Log("VoiceName:" + SoundName);
			bool played = false;
			AudioManager.Instance.SendMsg(new AudioMsgPlaySound((ushort) AudioEvent.PlaySound, SoundName, delegate
			{
				if (null != OnSoundBeganCallback)
				{
					OnSoundBeganCallback();
				}
			}, delegate
			{
				played = true;
				if (null != OnSoundEndedCallback)
				{
					OnSoundEndedCallback();
				}
			}));
			while (!played)
			{
				yield return null;
			}
		}
	}


	public class AudioVoiceNode : ICoroutineCmdNode
	{

		public string VoiceName;
		public QVoidDelegate.WithVoid OnVoiceBeganCallback = null;
		public QVoidDelegate.WithVoid OnVoiceEndedCallback = null;

		public AudioVoiceNode(string voiceName)
		{
			VoiceName = voiceName;
		}

		public IEnumerator Execute()
		{
			bool played = false;
			AudioManager.Instance.SendMsg(new AudioMsgPlayVoice((ushort) AudioEvent.PlayVoice, VoiceName, delegate
			{
				if (null != OnVoiceBeganCallback)
				{
					OnVoiceBeganCallback();
				}

			}, delegate
			{
				played = true;
				if (null != OnVoiceEndedCallback)
				{
					OnVoiceEndedCallback();
				}

			}));
			while (!played)
			{
				yield return null;
			}
		}
	}

	/// <summary>
	/// TODO:不支持本地化
	/// </summary>
	[QMonoSingletonAttribute("[Audio]/AudioManager")]
	public class AudioManager : QMgrBehaviour, ISingleton
	{
		#region Audio设置数据

		// 用来存储的Key
		const string KEY_AUDIO_MANAGER_SOUND_ON = "KEY_AUDIO_MANAGER_SOUND_ON";

		const string KEY_AUDIO_MANAGER_MUSIC_ON = "KEY_AUDIO_MANAGER_MUSIC_ON";
		const string KEY_AUDIO_MANAGER_VOICE_ON = "KEY_AUDIO_MANAGER_VOICE_ON";
		const string KEY_AUDIO_MANAGER_VOICE_VOLUME = "KEY_AUDIO_MANAGER_VOICE_VOLUME";
		const string KEY_AUDIO_MANAGER_SOUND_VOLUME = "KEY_AUDIO_MANAGER_SOUND_VOLUME";
		const string KEY_AUDIO_MANAGER_MUSIC_VOLUME = "KEY_AUDIO_MANAGER_MUSIC_VOLUME";

		bool mMusicOn = true;
		bool mSoundOn = true;
		bool mVoiceOn = true;
		float mMusicVolume = 1.0f;
		float mSoundsVolume = 1.0f;
		float mVoiceVolume = 1.0f;

		/// <summary>
		/// 读取音频数据
		/// </summary>
		void ReadAudioSetting()
		{
			mSoundOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_SOUND_ON, 1) == 1 ? true : false;
			mMusicOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_MUSIC_ON, 1) == 1 ? true : false;
			mVoiceOn = PlayerPrefs.GetInt(KEY_AUDIO_MANAGER_VOICE_ON, 1) == 1 ? true : false;

			mSoundsVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_SOUND_VOLUME, 1.0f);
			mMusicVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_MUSIC_VOLUME, 1.0f);
			mVoiceVolume = PlayerPrefs.GetFloat(KEY_AUDIO_MANAGER_VOICE_VOLUME, 1.0f);
		}

		/// <summary>
		/// 保存音频数据
		/// </summary>
		void SaveAudioSetting()
		{
			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_SOUND_ON, mSoundOn == true ? 1 : 0);
			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_MUSIC_ON, mMusicOn == true ? 1 : 0);
			PlayerPrefs.SetInt(KEY_AUDIO_MANAGER_VOICE_ON, mMusicOn == true ? 1 : 0);
			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_SOUND_VOLUME, mSoundsVolume);
			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_MUSIC_VOLUME, mMusicVolume);
			PlayerPrefs.SetFloat(KEY_AUDIO_MANAGER_VOICE_VOLUME, mVoiceVolume);
		}

		void OnApplicationQuit()
		{
			SaveAudioSetting();
		}

		void OnApplicationFocus(bool focus)
		{
			SaveAudioSetting();
		}

		void OnApplicationPause(bool pause)
		{
			SaveAudioSetting();
		}

		#endregion

		#region 消息处理

		void Awake()
		{
			RegisterEvents(new ushort[]
			{
				(ushort) AudioEvent.SoundSwitch,
				(ushort) AudioEvent.MusicSwitch,
				(ushort) AudioEvent.VoiceSwitch,
				(ushort) AudioEvent.SetSoundVolume,
				(ushort) AudioEvent.SetMusicVolume,
				(ushort) AudioEvent.SetVoiceVolume,
				(ushort) AudioEvent.PlayMusic,
				(ushort) AudioEvent.PlaySound,
				(ushort) AudioEvent.PlayVoice,
				(ushort) AudioEvent.PlayNode
			});
		}

		protected override void SetupMgrId()
		{
			mMgrId = (ushort) QMgrID.Audio;
		}

		protected override void ProcessMsg(int key, QMsg msg)
		{
			switch (msg.msgId)
			{
				case (ushort) AudioEvent.SoundSwitch:
					AudioMsgWithBool soundSwitchMsg = msg as AudioMsgWithBool;
					mSoundOn = soundSwitchMsg.on;
					break;
				case (ushort) AudioEvent.MusicSwitch:
					AudioMsgWithBool musicSwitchMsg = msg as AudioMsgWithBool;
					mMusicOn = musicSwitchMsg.on;
					if (!mMusicOn)
					{
						StopMusic();
					}
					break;
				case (ushort) AudioEvent.PlayMusic:
					Debug.LogFormat("play music msg: {0}, is musicOn: ", AudioEvent.PlayMusic.ToString(), mMusicOn);
					PlayMusic(msg as AudioMsgPlayMusic);
					break;
				case (ushort) AudioEvent.StopMusic:
					StopMusic();
					break;
				case (ushort) AudioEvent.PlaySound:
					AudioMsgPlaySound audioMsgPlaySound = msg as AudioMsgPlaySound;
					PlaySound(audioMsgPlaySound);
					break;

				case (ushort) AudioEvent.PlayVoice:
					PlayVoice(msg as AudioMsgPlayVoice);
					break;
				case (ushort) AudioEvent.StopVoice:
					StopVoice();
					break;

				case (ushort) AudioEvent.PlayNode:
					ICoroutineCmdNode msgPlayNode = (msg as AudioMsgWithNode).Node;

					StartCoroutine(msgPlayNode.Execute());
					break;
			}
		}

		#endregion


		#region 对外接口

		bool mInited = false;

		public void Init()
		{
			if (mInited)
				return;
			mInited = true;

			CheckAudioListener();

			gameObject.transform.position = Vector3.zero;

			// 读取存储
			ReadAudioSetting();
		}

		public void CheckAudioListener()
		{
			// 确保有一个AudioListener
			if (FindObjectOfType<AudioListener>() == null)
			{
				gameObject.AddComponent<AudioListener>();
			}
		}

		public bool SoundOn { get; private set; }

		public bool MusicOn
		{
			get { return mMusicOn; }
		}

		public bool VoiceOn
		{
			get { return mVoiceOn; }
		}

		public float SoundVolume
		{
			get { return mSoundsVolume; }
		}

		public float MusicVolume
		{
			get { return mMusicVolume; }
		}

		public float VoiceVolume
		{
			get { return mVoiceVolume; }
		}

		#endregion




		#region 内部实现

		const int AUDIO_SOURCES_NUM = 8;

		int mCurSourceIndex;


		/// <summary>
		/// 播放音乐
		/// </summary>
		void PlayMusic(AudioMsgPlayMusic msg)
		{

			if (!MusicOn && msg.allowMusicOff)
			{
				if (null != msg.onMusicBeganCallback)
				{
					msg.onMusicBeganCallback();
				}

				if (null != msg.onMusicEndedCallback)
				{
					msg.onMusicEndedCallback();
				}

				return;
			}

			Debug.Log(">>>>>> Start Play Music");

			// TODO: 需要按照这个顺序去 之后查一下原因
			//需要先注册事件，然后再play
			mMainUnit.SetOnStartListener(delegate(AudioUnit musicUnit)
			{
				if (null != msg.onMusicBeganCallback)
				{
					msg.onMusicBeganCallback();
				}

				//调用完就置为null，否则应用层每注册一个而没有注销，都会调用
				mMainUnit.SetOnStartListener(null);
			});

			mMainUnit.SetAudio(gameObject, msg.musicName, msg.loop);

			mMainUnit.SetOnFinishListener(delegate(AudioUnit musicUnit)
			{
				if (null != msg.onMusicEndedCallback)
				{
					msg.onMusicEndedCallback();
				}

				//调用完就置为null，否则应用层每注册一个而没有注销，都会调用
				mMainUnit.SetOnFinishListener(null);
			});
		}

		void SetVolume(AudioUnit audioUnit, VolumeLevel volumeLevel)
		{
			switch (volumeLevel)
			{
				case VolumeLevel.Max:
					audioUnit.SetVolume(1.0f);
					break;
				case VolumeLevel.Normal:
					audioUnit.SetVolume(0.5f);
					break;
				case VolumeLevel.Min:
					audioUnit.SetVolume(0.2f);
					break;
			}
		}

		public AudioUnit PlaySound(bool loop = false, System.Action<AudioUnit> callBack = null, int customEventID = -1)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			AudioUnit unit = ObjectPool<AudioUnit>.Instance.Allocate();

			unit.SetAudio(gameObject, name, loop);
			unit.SetOnFinishListener(callBack);
			unit.customEventID = customEventID;
			return unit;
		}

		/// <summary>
		/// 停止播放音乐 
		/// </summary>
		void StopMusic()
		{
			mMainUnit.Stop();
		}

		/// <summary>
		/// 播放音效
		/// </summary>
		void PlaySound(AudioMsgPlaySound msg)
		{
			AudioUnit unit = ObjectPool<AudioUnit>.Instance.Allocate();

			unit.SetOnStartListener(delegate(AudioUnit soundUnit)
			{
				if (null != msg.onSoundBeganCallback)
				{
					msg.onSoundBeganCallback();
				}
				unit.SetOnStartListener(null);
			});

			unit.SetAudio(gameObject, msg.soundName, false);

			unit.SetOnFinishListener(delegate(AudioUnit soundUnit)
			{
				if (null != msg.onSoundEndedCallback)
				{
					msg.onSoundEndedCallback();
				}
				unit.SetOnFinishListener(null);
			});
		}

		/// <summary>
		/// 播放语音
		/// </summary>
		void PlayVoice(AudioMsgPlayVoice msg)
		{
			mVoiceUnit.SetOnStartListener(delegate(AudioUnit musicUnit)
			{
				SetVolume(mMainUnit, VolumeLevel.Min);

				if (null != msg.onVoiceBeganCallback)
				{
					msg.onVoiceBeganCallback();
				}

				mVoiceUnit.SetOnStartListener(null);
			});

			mVoiceUnit.SetAudio(gameObject, msg.voiceName, msg.loop);

			mVoiceUnit.SetOnFinishListener(delegate(AudioUnit musicUnit)
			{
				SetVolume(mMainUnit, VolumeLevel.Max);

				if (null != msg.onVoiceEndedCallback)
				{
					msg.onVoiceEndedCallback();
				}

				mVoiceUnit.SetOnFinishListener(null);
			});
		}

		void StopVoice()
		{
			mVoiceUnit.Stop();
		}

		void PauseVoice()
		{
			mVoiceUnit.Pause();
		}

		void ResumeVoice()
		{
			mVoiceUnit.Resume();
		}

		#region 单例实现

		private AudioManager()
		{
			Debug.Log("Create AudioManager");
		}

		public static AudioManager Instance
		{
			get { return QMonoSingletonProperty<AudioManager>.Instance; }
		}

		#endregion

		void Example()
		{
			// 按钮点击音效
			this.SendMsg(new AudioMsgPlaySound((ushort) AudioEvent.PlaySound, "Sound.CLICK"));

			//播放背景音乐
			this.SendMsg(new AudioMsgPlayMusic("music", true));

			//停止播放音乐
			this.SendMsg(new QMsg((ushort) AudioEvent.StopMusic));

			this.SendMsg(new AudioMsgPlayVoice((ushort) AudioEvent.PlayVoice, "Sound.CLICK", delegate { }, delegate { }));
		}

		protected int mMaxSoundCount = 5;
		protected AudioUnit mMainUnit;
		protected AudioUnit mVoiceUnit;

		public void OnSingletonInit()
		{
			ObjectPool<AudioUnit>.Instance.Init(mMaxSoundCount, 1);
			mMainUnit = new AudioUnit();
			mMainUnit.usedCache = false;
			mVoiceUnit = new AudioUnit();
			mVoiceUnit.usedCache = false;
		}

		public void Dispose()
		{
		}


		//常驻内存不卸载音频资源
		protected ResLoader mRetainResLoader;

		protected List<string> mRetainAudioNames;

		/// <summary>
		/// 添加常驻音频资源，建议尽早添加，不要在用的时候再添加
		/// </summary>
		public void AddRetainAudio(string audioName)
		{
			if (mRetainResLoader == null)
				mRetainResLoader = ResLoader.Allocate();
			if (mRetainAudioNames == null)
				mRetainAudioNames = new List<string>();

			if (!mRetainAudioNames.Contains(audioName))
			{
				mRetainAudioNames.Add(audioName);
				mRetainResLoader.Add2Load(audioName);
				mRetainResLoader.LoadAsync();
			}
		}

		/// <summary>
		/// 删除常驻音频资源
		/// </summary>
		public void RemoveRetainAudio(string audioName)
		{
			if (mRetainAudioNames != null && mRetainAudioNames.Remove(audioName))
			{
				mRetainResLoader.ReleaseRes(audioName);
			}
		}

		#endregion
	}
}