//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//
//namespace PTGame.Localize {
//
//	/// <summary>
//	/// 多语言Text
//	/// </summary>
//	public class PTText : MonoBehaviour,ILocalizeView {
//
//		#region 对外
//		/// <summary>
//		/// 设置Key
//		/// </summary>
//		public static PTText Localize(GameObject go,string key) {
//			var ptText = go.GetComponent<PTText> ();
//			if (null == ptText) {
//				ptText = go.AddComponent<PTText> ();
//			}
//			ptText.SetKey (key);
//			return ptText;
//		}
//		public static PTText Localize(Text text,string key) {
//			return Localize (text.gameObject, key);
//		}
//		public static PTText Localize(Transform trans,string key) {
//			return Localize (trans.gameObject, key);
//		}
//
//
//		[SerializeField] string m_Key = "";
//		public string Key {
//			get {
//				return m_Key;
//			}
//			set {
//				m_Key = value;
//				UpdateLocalizeView ();
//			}
//		}
//		/// <summary>
//		/// 设置Key
//		/// </summary>
//		public void SetKey(string key) {
//			Key = key;
//		}
//
//		public string GetKey() {
//			return m_Key;
//		}
//
//		public Text Text {
//			get {
//				return GetComponent<Text> ();
//			}
//		}
//
//		#if UNITY_EDITOR
//		string mConfig = null;
//		#endif
//		#endregion
//
//		void Awake() {
//			PTLanguageManager.Instance.PushActiveLocalizeView (this);
//			if (!string.IsNullOrEmpty(Key)) {
//				UpdateLocalizeView ();
//			}
//		}
//			
//		public void UpdateLocalizeView() {
//			Text text = GetComponent<Text> ();
//			text.text =	PTLanguageManager.Instance.Text4Key (Key.Trim()).Replace("\\n","\n");
//			string textConfig = PTLanguageManager.Instance.Config4Key (Key.Trim ());
//
//			// 解析配置用的
//			if (string.IsNullOrEmpty (textConfig)) {
//
//			}
//			else {
//				string[] configs = textConfig.Split (new char[]{ ',' });
//
//				for (int i = 0; i < configs.Length; i++) {
//					string config = configs [i];
//					string[] configKeyValuePair = config.Split (new char[]{ ':' });
//					string key = configKeyValuePair [0];
//					string value = configKeyValuePair [1];
//					switch (key) {
//						case "posX":
//							{
//								float posX = float.Parse (value);
//								var textRectTrans = text.GetComponent<RectTransform> ();
//								var textAnchoredPos = textRectTrans.anchoredPosition;
//								textAnchoredPos.x = posX;
//								textRectTrans.anchoredPosition = textAnchoredPos;
//							}
//							break;
//						case "posY":
//							{
//								float posY = float.Parse (value);
//								var textRectTrans = text.GetComponent<RectTransform> ();
//								var textAnchoredPos = textRectTrans.anchoredPosition;
//								textAnchoredPos.y = posY;
//								textRectTrans.anchoredPosition = textAnchoredPos;
//							}
//							break;
//					}
//				}
//			}
//		}
//
//		void OnDestroy() {
//			PTLanguageManager.Instance.RemoveActiveLocalizeView (this);
//		}
//	}
//
//}
