/****************************************************************************
 * Copyright (c) 2017 snowcold
 * Copyright (c) 2017 liangxie
 * 
 * http://liangxiegame.com
 * https://github.com/liangxiegame/QFramework
 * https://github.com/SnowCold/SCFramework_Engine
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace QFramework
{
    public class QSingletonCreator
    {		
		public static T CreateSingleton<T>() where T : class,ISingleton
		{
			if (Framework.IsApplicationQuit)
			{
				return null;
			}

			T retInstance = default(T);
			// 先获取所有非public的构造方法
			ConstructorInfo[] ctors = typeof(T).GetConstructors (BindingFlags.Instance | BindingFlags.NonPublic);
			// 从ctors中获取无参的构造方法
			ConstructorInfo ctor = Array.Find (ctors, c => c.GetParameters ().Length == 0);

			if (ctor == null) 
			{
				Debug.LogWarning ("Non-public ctor() not found!");
				ctors = typeof(T).GetConstructors (BindingFlags.Instance | BindingFlags.Public);
				ctor = Array.Find (ctors, c => c.GetParameters ().Length == 0);
			} 

			retInstance = ctor.Invoke (null) as T;

			retInstance.OnSingletonInit ();

			return retInstance;
		}


        public static T CreateMonoSingleton<T>() where T : MonoBehaviour, ISingleton
        {
			if (Framework.IsApplicationQuit)
            {
                return null;
            }

            T instance = null;

			if (instance == null && !Framework.IsApplicationQuit)
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

        protected static T CreateComponentOnGameObject<T>(string path, bool dontDestroy) where T : MonoBehaviour
        {
            GameObject obj = FindGameObject(null, path, true, dontDestroy);
            if (obj == null)
            {
                obj = new GameObject("Singleton of " + typeof(T).Name);
                if (dontDestroy)
                {
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                }
            }

            return obj.AddComponent<T>();
        }

		static GameObject FindGameObject(GameObject root, string path, bool build, bool dontDestroy)
		{
			if (path == null || path.Length == 0)
			{
				return null;
			}

			string[] subPath = path.Split('/');
			if (subPath == null || subPath.Length == 0)
			{
				return null;
			}

			return FindGameObject(null, subPath, 0, build, dontDestroy);
		}

		static GameObject FindGameObject(GameObject root, string[] subPath, int index, bool build, bool dontDestroy)
		{
			GameObject client = null;

			if (root == null)
			{
				client = GameObject.Find(subPath[index]);
			}
			else
			{
				var child = root.transform.Find(subPath[index]);
				if (child != null)
				{
					client = child.gameObject;
				}
			}

			if (client == null)
			{
				if (build)
				{
					client = new GameObject(subPath[index]);
					if (root != null)
					{
						client.transform.SetParent(root.transform);
					}
					if (dontDestroy && index == 0)
					{
						GameObject.DontDestroyOnLoad(client);
					}
				}
			}

			if (client == null)
			{
				return null;
			}

			if (++index == subPath.Length)
			{
				return client;
			}

			return FindGameObject(client, subPath, index, build, dontDestroy);
		}
    }
}