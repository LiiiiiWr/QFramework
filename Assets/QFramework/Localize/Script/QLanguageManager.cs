using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
/// <summary>
/// 运行时的本地化管理器
/// TODO:
/// 1.监听系统语言切换事件,对注册其他语言的Component进行广播。
/// 2.
/// </summary>
namespace QFramework {

	public enum Language {
		Chinese 					= 6,
		English 					= 10,
		French 						= 14,
		Japanese					= 22,
		Russian						= 30,
		Spanish						= 34,
		ChineseSimplified 			= 40,
		ChineseTraditional 			= 41,
	}


//	-- Afrikaans, 0
//	-- Arabic,1
//	-- Basque,2
//	-- Belarusian,3
//	-- Bulgarian,4
//	-- Catalan,5
//	-- Chinese,6
//	-- Czech,7
//	-- Danish,8
//	-- Dutch,9
//	-- Estonian,11
//	-- Faroese,12
//	-- Finnish,13
//	-- German,15
//	-- Greek,16
//	-- Hebrew,17
//	-- Hugarian,18
//	-- Icelandic,19
//	-- Indonesian,20
//	-- Italian,21
//	-- Korean,23
//	-- Latvian,24
//	-- Lithuanian,25
//	-- Norwegian,26
//	-- Polish,27
//	-- Portuguese,28
//	-- Romanian,29
//	-- Russian,30
//	-- SerboCroatian,31
//	-- Slovak,32
//	-- Slovenian,33
//	-- Spanish,34
//	-- Swedish,35
//	-- Thai,36
//	-- Turkish,37
//	-- Ukrainian,38
//	-- Vietnamese,39
//
//	-- Unknown,42
//	-- Hungarian = 18
		
	public class QLanguageManager : MonoBehaviour {

		private QLanguageManager() {}

		/// <summary>
		/// 单例实现
		/// </summary>
		public static QLanguageManager Instance {
			get {
				return QMonoSingletonComponent<QLanguageManager>.Instance;
			}
		}

		/// <summary>
		/// 临时支持跳转用的
		/// </summary>
		public bool IsChinese {
			get {
				return Application.systemLanguage == SystemLanguage.Chinese ||
				Application.systemLanguage == SystemLanguage.ChineseSimplified ||
				Application.systemLanguage == SystemLanguage.ChineseTraditional;
			}
		}


//		/// <summary>
//		/// 后缀
//		/// </summary>
//		/// <value>The suffix.</value>
//		public string Suffix {
//			get {
//				string retValue = "cn";
//				switch (Application.systemLanguage) {
//					// TODO:考虑用map实现
//					case SystemLanguage.Chinese:
//					case SystemLanguage.ChineseSimplified:
//					case SystemLanguage.ChineseTraditional:
//						retValue = "cn";
//						break;
//					case SystemLanguage.English:
//						retValue = "en";
//						break;
//				}
//				return retValue;
//			}
//		}
//
//
//		/// <summary>
//		/// 解析
//		/// </summary>
//		public void Parse() {
//			if (null == mCurConfig) {
//				#if UNITY_EDITOR
//				ConfigManager.Instance.Load ();
//				#endif
//
//				mCurConfig =  ConfigManager.Instance.GetConfig<I18nConfig> ();					
//			}
//		}
//
//		/// <summary>
//		/// 存储所有Text的字典
//		/// </summary>
//		I18nConfig mCurConfig;
//
//
//		/// <summary>
//		/// 根据Key查找文本
//		/// </summary>
//		public string Text4Key(string key) {
//			Parse ();
//
//			I18nDefine define = mCurConfig.GetI18nByKey (key);
//
//			string retText = "";
//			if (null != define && !string.IsNullOrEmpty(GetValue(define,Suffix))) {
//				retText = GetValue (define, Suffix);
//			}
//			else {
//				Debug.LogError ("No Key Name:" + key + " or No Nation " + Suffix + " Please Check The word.xml file");
//			}
//			return retText;
//		}
//
//		string GetValue(I18nDefine define,string attribName) {
//			switch (attribName) {
//				case "cn":
//					return define.CN;
//				case "cn_config":
//					return define.CNConfig;
//			}
//			return null;
//		}
//
//		/// <summary>
//		/// 配置
//		/// </summary>
//		public string Config4Key(string key) {
//			string retConfig = "";
//			I18nDefine define = mCurConfig.GetI18nByKey (key);
//
//			if ( define != null && !string.IsNullOrEmpty(GetValue(define,Suffix))) {
//				retConfig = GetValue (define, Suffix + "_config");
//				Debug.Log (retConfig);
//			}
//			else {
//				Debug.LogError ("No Key Name:" + key + " or No Nation " + Suffix + " Please Check The word.xml file");
//			}
//			return retConfig;
//		}
//
//		void ChangeLanguage() {
//
//			List<ILocalizeView> activeViews = new List<ILocalizeView> ();
//
//			foreach (var localizeView in mActiveLocalizeViews) {
//				if (null != localizeView) {
//					activeViews.Add (localizeView);
//					localizeView.UpdateLocalizeView ();
//				}
//			}
//
//			mActiveLocalizeViews.Clear ();
//			mActiveLocalizeViews = activeViews;
//		}
//
//		List<ILocalizeView> mActiveLocalizeViews = new List<ILocalizeView>();
//
//		public void PushActiveLocalizeView(ILocalizeView localizeView) {
//			if (!mActiveLocalizeViews.Contains (localizeView)) {
//				mActiveLocalizeViews.Add (localizeView);
//			}
//			else {
//				Debug.LogError ("加了两次");
//			}
//		}
//
//		public void RemoveActiveLocalizeView(ILocalizeView localizeView) {
//			if (mActiveLocalizeViews.Contains (localizeView)) {
//				mActiveLocalizeViews.Remove (localizeView);
//			}
//			else {
//				Debug.LogError ("已经删除过了");
//			}
//		}
	}
}