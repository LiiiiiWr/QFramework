using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace QFramework.Example
{
	public class ClassA : ICacheAble
	{
		public string Name = "NewObjA";

		bool mCacheFlag = false;

		public bool CacheFlag
		{
			get {
				return mCacheFlag;
			}
			set {
				mCacheFlag = value;
			}
		}

		public ClassA()
		{
			Debug.Log ("new ClassA()");
		}

		/// <summary>
		/// reset object state when call ObjectPool<ClassA>.Instance.Recycle(classAObj);
		/// </summary>
		public void OnCacheReset()
		{
			Debug.Log ("ClassA OnCacheReset:" + Name);
		}
	}


	public class ClassB : ICacheAble,ICacheType
	{
		public string Name = "NewObjB";

		bool mCacheFlag = false;

		public bool CacheFlag
		{
			get {
				return mCacheFlag;
			}
			set {
				mCacheFlag = value;
			}
		}

		public ClassB()
		{
			Debug.Log ("new ClassB()");
		}

		/// <summary>
		/// reset object state when call ObjectPool<ClassA>.Instance.Recycle(classAObj);
		/// </summary>
		public void OnCacheReset()
		{
			Debug.Log ("ClassB OnCacheReset:" + Name);
		}

		public static ClassB Allocate()
		{
			return ObjectPool<ClassB>.Instance.Allocate ();
		}

		public void Recycle2Cache()
		{
			ObjectPool<ClassB>.Instance.Recycle (this);
		}
	}

	public class ExampleObjectPool : MonoBehaviour 
	{

		// Use this for initialization
		void Start () 
		{
			var objA1 = ObjectPool<ClassA>.Instance.Allocate ();
			objA1.Name = "objA1";

			var objA2 = ObjectPool<ClassA>.Instance.Allocate ();
			objA2.Name = "objA2";

			ObjectPool<ClassA>.Instance.Recycle (objA1);
			ObjectPool<ClassA>.Instance.Recycle (objA2);

			Debug.Log(ObjectPool<ClassA>.Instance.Allocate ().Name);
			Debug.Log(ObjectPool<ClassA>.Instance.Allocate ().Name);
			Debug.Log(ObjectPool<ClassA>.Instance.Allocate ().Name);



			var objB1 = ClassB.Allocate ();
			objB1.Name = "objB1";
			objB1.Recycle2Cache ();

			Debug.Log ("ClassA Obj In ObjectPool Count Is " + ObjectPool<ClassA>.Instance.CurCount);
			Debug.Log ("ClassB Obj In ObjectPool Count Is " + ObjectPool<ClassB>.Instance.CurCount);
		}
	}
}