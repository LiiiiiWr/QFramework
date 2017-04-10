using System;
using UnityEngine;
using SCFramework;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ProtoBuf;

namespace QFramework
{
	public static class SerializeHelper
	{
		public static bool SerializeBinary(string path, object obj)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.w("SerializeBinary Without Valid Path.");
				return false;
			}

			if (obj == null)
			{
				Log.w("SerializeBinary obj is Null.");
				return false;
			}

			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				bf.Serialize(fs, obj);
				return true;
			}
		}

		public static object DeserializeBinary(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.w("DeserializeBinary Without Valid Path.");
				return null;
			}

			FileInfo fileInfo = new FileInfo(path);

			using (FileStream fs = fileInfo.OpenRead())
			{
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				object data = bf.Deserialize(fs);

				if (data != null)
				{
					return data;
				}
			}

			Log.w("DeserializeBinary Failed:" + path);
			return null;
		}

		public static bool SerializeXML(string path, object obj)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.w("SerializeBinary Without Valid Path.");
				return false;
			}

			if (obj == null)
			{
				Log.w("SerializeBinary obj is Null.");
				return false;
			}

			using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
			{
				XmlSerializer xmlserializer = new XmlSerializer(obj.GetType());
				xmlserializer.Serialize(fs, obj);
				return true;
			}
		}

		public static object DeserializeXML<T>(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				Log.w("DeserializeBinary Without Valid Path.");
				return null;
			}

			FileInfo fileInfo = new FileInfo(path);

			using (FileStream fs = fileInfo.OpenRead())
			{
				XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
				object data = xmlserializer.Deserialize(fs);

				if (data != null)
				{
					return data;
				}
			}

			Log.w("DeserializeBinary Failed:" + path);
			return null;
		}


		/// <summary>
		/// 序列化、反序列化，存储数据
		/// </summary>


		public static string ToJson<T>(this T obj) where T : class
		{
			#if UNITY_EDITOR
			return JsonUtility.ToJson(obj, true);
			#else
			return JsonUtility.ToJson(obj);
			#endif
		}

		public static T FromJson<T>(this string json) where T : class
		{
			return (T) JsonUtility.FromJson(json, typeof(T));
		}

		public static void SaveJson<T>(this T obj, string path) where T : class
		{
			System.IO.File.WriteAllText(path, obj.ToJson<T>());
		}

		public static T LoadJson<T>(string path) where T : class
		{
			return System.IO.File.ReadAllText(path).FromJson<T>();
		}


		public static byte[] ToProtoBuff<T>(this T obj) where T : class
		{
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				ProtoBuf.Serializer.Serialize<T>(ms, obj);
				return ms.ToArray();
			}
		}

		public static T FromProtoBuff<T>(this byte[] bytes) where T : class
		{
			if (bytes == null || bytes.Length == 0)
			{
				throw new System.ArgumentNullException("bytes");
			}
			T t = ProtoBuf.Serializer.Deserialize<T>(new System.IO.MemoryStream(bytes));
			return t;
		}

		public static void SaveProtoBuff<T>(this T obj, string path) where T : class
		{
			System.IO.File.WriteAllBytes(path, obj.ToProtoBuff<T>());
		}

		public static T LoadProtoBuff<T>(string path) where T : class
		{
			return System.IO.File.ReadAllBytes(path).FromProtoBuff<T>();
		}

		#if UNITY_EDITOR

		//Example
		//*********json 数据结构***********
		[System.Serializable]
		class PlayerInfo
		{
			public string name;
			public int lives;
			public float health;
			public ChildInfo[] childs;
		}

		[System.Serializable]
		class ChildInfo
		{
			public string name;
			public int lives;
		}

		//*********protobuf 数据结构***********
		[ProtoBuf.ProtoContract]
		class Person
		{
			[ProtoBuf.ProtoMember(1)]
			public int Id { get; set; }

			[ProtoBuf.ProtoMember(2)]
			public string Name { get; set; }

			[ProtoBuf.ProtoMember(3)]
			public Address Address { get; set; }
		}

		[ProtoBuf.ProtoContract]
		class Address
		{
			[ProtoBuf.ProtoMember(1)]
			public string Line1 { get; set; }

			[ProtoBuf.ProtoMember(2)]
			public string Line2 { get; set; }
		}

		static void Example()
		{
			//use json
			PlayerInfo playerInfo = new PlayerInfo();
			playerInfo.name = "qINGYUN";
			playerInfo.lives = 25;

			playerInfo.SaveJson<PlayerInfo>("/UserData/..");

			//use protobuf
			Person person = new Person();
			person.Name = "zhenhua";

			person.SaveProtoBuff<Person>("/UserData/..");
		}
		#endif
	}
}
