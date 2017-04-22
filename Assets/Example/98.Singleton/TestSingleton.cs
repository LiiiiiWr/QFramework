using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace QFramework.Example {
	

	public class TestSingleton : MonoBehaviour {

		// Use this for initialization
		void Start () 
		{
			var deviceMgr = DeviceMgr.Instance;
			var spriteMgr = SpriteMgr.Instance;
			var textureManager = TextureManager.Instance;
			var imageManager = ImageManager.Instance;
		}
			
	}

	public class DeviceMgr : QSingleton<DeviceMgr>
	{
		protected DeviceMgr()
		{
			Debug.Log ("DeviceMgr ctor");
		}

		public override void OnSingletonInit()
		{
			Debug.Log ("DeviceMgr Init");
		}
	}

	public class SpriteMgr : ScriptableObject,ISingleton
	{
		protected SpriteMgr()
		{
			Debug.Log ("Sprite Mgr ctor");
		}

		public static SpriteMgr Instance
		{
			get {
				return QSingletonProperty<SpriteMgr>.Instance;
			}
		}

		public void OnSingletonInit()
		{
			Debug.Log ("SpriteMgr Init");
		}
	}


	public class TextureManager : QMonoSingleton<TextureManager>
	{
		public override void OnSingletonInit()
		{
			Debug.Log ("TextureManager Init");
		}
	}

	[QMonoSingletonAttribute("[Image]/ImageManager")]
	public class ImageManager : MonoBehaviour,ISingleton
	{
		public static ImageManager Instance
		{
			get {
				return QMonoSingletonProperty<ImageManager>.Instance;
			}
		}

		public void OnSingletonInit()
		{
			Debug.Log ("ImageManager Init");
		}
	}

}