using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using QFramework;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToDoList {

	[System.Serializable]
	[ProtoBuf.ProtoContract]
	public class ToDoListItemData {

		[ProtoBuf.ProtoMember(1)]
		public string Id;

		[ProtoBuf.ProtoMember(2)]
		public string Content;

		[ProtoBuf.ProtoMember(3)]
		public bool Complete;

		[ProtoBuf.ProtoMember(4)]
		public bool Deleted = false;

		public void Description() {
			Debug.Log (Id + ":" + Complete + ":" + Content + ":" + Deleted);
		}
	}

	[System.Serializable]
	[ProtoBuf.ProtoContract]
	public class ToDoListSavedDataFile
	{
		public static string FILE_NAME_JSON = "/ToDoListData.json";
		public static string FILE_NAME_Protobuf = "/ToDoListData.bytes";


		[ProtoBuf.ProtoMember(1)]
		public ToDoListItemData[] Datas = new ToDoListItemData[0];
	}


	public class SaveManager  {
		
		/// <summary>
		/// 是否使用protobuf存储，否则使用json
		/// </summary>
		static bool mUseProtobuf
		{
			get
			{
				#if UNITY_EDITOR
				return EditorPrefs.GetBool("UseProtobuf", false);
				#else
				return false;
				#endif
			}
		}

		static ToDoListSavedDataFile mLetterDataFile = null;

		/// <summary>
		/// 读取
		/// </summary>
		public static List<ToDoListItemData> Load() {
			List<ToDoListItemData> retList = new List<ToDoListItemData> ();

			if (!mUseProtobuf)
			{
				if (File.Exists(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_JSON))
					mLetterDataFile = SerializeHelper.LoadJson<ToDoListSavedDataFile>(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_JSON);
			}
			else
			{
				if (File.Exists(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_Protobuf))
					mLetterDataFile = SerializeHelper.LoadProtoBuff<ToDoListSavedDataFile>(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_Protobuf);
			}

			if (null == mLetterDataFile)
			{
				FirstCreateData();
			}

			Debug.Log("Load Data");
			foreach (var data in mLetterDataFile.Datas)
			{
				data.Description();
			}

			retList = new List<ToDoListItemData> (mLetterDataFile.Datas);

			return retList;
		}

		/// <summary>
		/// 保存
		/// </summary>
		public static void Save(List<ToDoListItemData> itemList) {

			mLetterDataFile.Datas = itemList.ToArray ();

			if (!mUseProtobuf)
			{
				mLetterDataFile.SaveJson<ToDoListSavedDataFile>(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_JSON);
			}
			else
			{
				mLetterDataFile.SaveProtoBuff<ToDoListSavedDataFile>(Application.persistentDataPath + ToDoListSavedDataFile.FILE_NAME_Protobuf);
			}
		}




		/// <summary>
		/// 初次创建
		/// </summary>
		static void FirstCreateData()
		{
			mLetterDataFile = new ToDoListSavedDataFile();
			Save(new List<ToDoListItemData>( mLetterDataFile.Datas));
		}
			
	}
}
	