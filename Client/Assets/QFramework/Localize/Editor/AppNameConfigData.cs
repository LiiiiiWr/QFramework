using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using QFramework.Libs;
using System.IO;

namespace QFramework {

	[System.Serializable]
	public class LanguageData {
		public int Index = 0;
		public string AppName = "";
		public LanguageData() {}
		public LanguageData(int index,string appName) {
			Index = index;
			AppName = appName;
		}
	}


	public class AppNameConfigData  {

		const string COUNTRY_PREFIX = "PTLocalizeAppName";

		public static string [] LanguageDef = new string[]{
			"None",
			"English",
			"Frence",
			"German",
			"Chinese(Simplified)",
			"Chinese(Traditional)",
			"Japanese",
			"Spanish",
			"Spanish(Mexico)",
			"Italian",
			"Dutch",
			"Korean",
			"Portuguese(Brazil)",
			"Portuguese(Portugal)",
			"Danish",
			"Finnish",
			"Norwegian Bokmal",
			"Swedish",
			"Russian",
			"Polish",
			"Turkish",
			"Arabic",
			"Thai",
			"Czech",
			"Hungarian",
			"Catalan",
			"Croatian",
			"Greek",
			"Hebrew",
			"Romanian",
			"Slovak",
			"Ukrainian",
			"Indonesian",
			"Malay",
			"Vietnamese"
		};

		public string[] LanguageDefAndroid = new string[] {
			"None",
			"en",
			"fr",
			"de",
			"zh-rCN",
			"zh",
			"ja",
			"es",
			"es-rUS",
			"it",
			"nl",
			"ko",
			"pt-rBR",
			"pt-rPT",
			"da",
			"fi",
			"nb",
			"sv",
			"ru",
			"pl",
			"tr",
			"ar",
			"th",
			"cs",
			"hu",
			"ca",
			"hr",
			"el",
			"he",
			"ro",
			"sk",
			"uk",
			"id",
			"ms",
			"vi"
		};

		public string[] LanguageDefiOS = new string[] {
			"None",
			"en",
			"fr",
			"de",
			"zh-Hans",
			"zh-Hant",
			"ja",
			"es",
			"es-MX",
			"it",
			"nl",
			"ko",
			"pt-BR",
			"pt-PT",
			"da",
			"fi",
			"nb",
			"sv",
			"ru",
			"pl",
			"tr",
			"ar",
			"th",
			"cs",
			"hu",
			"ca",
			"hr",
			"el",
			"he",
			"ro",
			"sk",
			"uk",
			"id",
			"ms",
			"vi"
		};


		[System.Serializable]
		public class LocalizeConfig
		{
			public LanguageData[] LanguageDatas;

		}

		public List<LanguageData> SupportedLanguageItems = new List<LanguageData>();


		static string mConfigSavedDir = Application.dataPath + "/QFrameworkData/Localize/";
		static string mConfigSavedFileName = "AppNameConfig.json";

		public static AppNameConfigData Load() {

			if (!Directory.Exists (mConfigSavedDir)) 
			{
				Directory.CreateDirectory (mConfigSavedDir);
			}

			if (!File.Exists (mConfigSavedDir + mConfigSavedFileName)) {
				var fileStream = File.Create (mConfigSavedDir + mConfigSavedFileName);
				fileStream.Close ();
			}

			AssetDatabase.Refresh ();

			var retConfigData = new AppNameConfigData ();

			var localizeConfig = SerializeHelper.LoadJson<LocalizeConfig> (mConfigSavedDir + mConfigSavedFileName);

			if (localizeConfig == null || localizeConfig.LanguageDatas == null || localizeConfig.LanguageDatas.Length == 0) {
				retConfigData.SupportedLanguageItems.Add (new LanguageData (4, Application.productName));
			}	
			else {
				retConfigData.SupportedLanguageItems.AddRange (localizeConfig.LanguageDatas);
			}

			return retConfigData;
		}

		public void Save() {
			var localizeConfig = new LocalizeConfig ();
			localizeConfig.LanguageDatas = SupportedLanguageItems.ToArray ();
			localizeConfig.SaveJson (mConfigSavedDir + mConfigSavedFileName);
		}
	}

}