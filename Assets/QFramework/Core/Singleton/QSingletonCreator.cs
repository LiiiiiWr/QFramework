using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace QFramework
{
    public class QSingletonCreator : MonoBehaviour
    {
        private static bool m_IsApplicationQuit = false;

        public static bool isApplicationQuit
        {
            get { return m_IsApplicationQuit; }
        }
			
		public static K CreateSingleton<K>() where K : class,ISingleton
		{
			if (m_IsApplicationQuit)
			{
				return null;
			}

			K retInstance = default(K);
			// 先获取所有非public的构造方法
			ConstructorInfo[] ctors = typeof(K).GetConstructors (BindingFlags.Instance | BindingFlags.NonPublic);
			// 从ctors中获取无参的构造方法
			ConstructorInfo ctor = Array.Find (ctors, c => c.GetParameters ().Length == 0);

			if (ctor == null) 
			{
				Debug.LogWarning ("Non-public ctor() not found!");
				ctors = typeof(K).GetConstructors (BindingFlags.Instance | BindingFlags.Public);
				ctor = Array.Find (ctors, c => c.GetParameters ().Length == 0);
			} 

			retInstance = ctor.Invoke (null) as K;

			retInstance.OnSingletonInit ();

			return retInstance;
		}


        public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
        {
            if (m_IsApplicationQuit)
            {
                return null;
            }

            T instance = null;

            if (instance == null && !m_IsApplicationQuit)
            {
                instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (instance == null)
                {
                    MemberInfo info = typeof(T);
                    object[] attributes = info.GetCustomAttributes(true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        QMonoSingletonAttribute defineAttri = attributes[i] as QMonoSingletonAttribute;
                        if (defineAttri == null)
                        {
                            continue;
                        }
                        instance = CreateComponentOnGameObject<T>(defineAttri.AbsolutePath, true);
                        break;
                    }

                    if (instance == null)
                    {
                        GameObject obj = new GameObject("Singleton of " + typeof(T).Name);
                        UnityEngine.Object.DontDestroyOnLoad(obj);
                        instance = obj.AddComponent<T>();
                    }

                    instance.OnSingletonInit();
                }
            }

            return instance;
        }

        protected static K CreateComponentOnGameObject<K>(string path, bool dontDestroy) where K : MonoBehaviour
        {
            GameObject obj = GameObjectHelper.FindGameObject(null, path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of " + typeof(K).Name);
                if (dontDestroy)
                {
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }

            return obj.AddComponent<K>();
        }

        protected void OnApplicationQuit()
        {
            m_IsApplicationQuit = true;
        }
    }
}
