using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace ToDoList {

	public class UIEditPanelData {
		
		public string Title;

		public string Content;
	
	}

	public enum ToDoListEvent {
		Began = (ushort)QMgrID.Data,
		ModifiedItem,
		CreateNewItem,
		DeleteItem,
		End
	}

	public class CreateNewItemMsg :QMsg {
		public ToDoListItemData NewItemData;
		public CreateNewItemMsg(ushort msgId,ToDoListItemData newItemData):base(msgId) {
			this.NewItemData = newItemData;
		}
	}

	public class ModifiedItemMsg : QMsg {
		public string SrcTitle;
		public ToDoListItemData ItemData;
		public ModifiedItemMsg(ushort msgId,string srcTitle,ToDoListItemData itemData):base(msgId) {
			this.SrcTitle = srcTitle;
			this.ItemData = itemData;
		}
	}

	public class DeleteItemMsg : QMsg {
		public string Title;
		public DeleteItemMsg(ushort msgId,string title):base(msgId) {
			this.Title = title;
		}
	}

	public class ToDoListManager : QMgrBehaviour,ISingleton {

		Dictionary<string,ToDoListItemData> mCachedData = new Dictionary<string, ToDoListItemData> ();

		public Dictionary<string,ToDoListItemData> CurCachedData {
			get {
				return mCachedData;
			}
		}

		protected override void SetupMgrId ()
		{
			mMgrId = (ushort)QMgrID.Data;
		}
			
		protected override void ProcessMsg (int key,QMsg msg)
		{
			switch (msg.msgId) {
				case (ushort)ToDoListEvent.ModifiedItem:
					ModifiedItemMsg modifiedMsg = msg as ModifiedItemMsg;
					mCachedData.Remove (modifiedMsg.SrcTitle);
					modifiedMsg.ItemData.Description ();
					mCachedData.Add (modifiedMsg.ItemData.Id, modifiedMsg.ItemData);
					this.SendMsg (new QMsg ((ushort)UIEvent.UpdateView));
					break;
				case (ushort)ToDoListEvent.CreateNewItem:
					CreateNewItemMsg newItemMsg = msg as CreateNewItemMsg;
					newItemMsg.NewItemData.Description ();
					mCachedData.Add (newItemMsg.NewItemData.Id,newItemMsg.NewItemData);
					this.SendMsg (new QMsg ((ushort)UIEvent.UpdateView));
					break;
				case (ushort)ToDoListEvent.DeleteItem:
					DeleteItemMsg deleteItemMsg = msg as DeleteItemMsg;
					mCachedData.Remove (deleteItemMsg.Title);
					this.SendMsg (new QMsg ((ushort)UIEvent.UpdateView));
					break;
			}
		}


		public static ToDoListManager Instance {
			get {
				return QMonoSingletonProperty<ToDoListManager>.Instance;
			}
		}

		public void OnSingletonInit()
		{

		}

		private ToDoListManager() {

		}

		void Awake() {
			Debug.Log ("ToDoList Manager Awake");

			LoadData ();

			RegisterEvent(ToDoListEvent.CreateNewItem);
			RegisterEvent(ToDoListEvent.ModifiedItem);
			RegisterEvent(ToDoListEvent.DeleteItem);
		}
			
		void LoadData() 
		{
			Debug.Log ("---- LoadData ----");
			var list = SaveManager.Load ();
			Debug.Log (list.Length);
			mCachedData.Clear ();

			for (int i = 0; i < list.Length; i++) 
			{
				var itemData = list [i];
				if (string.Equals("Default",itemData.Id)) {
					continue;
				}
				if (!mCachedData.ContainsKey (itemData.Id)) {
					mCachedData.Add (itemData.Id, itemData);
					Debug.Log (itemData.Id);
				}
				else {
					Debug.LogWarning (itemData.Id + ": Exists");
				}
			}
			Debug.Log ("-------------------");
		}


		public void UpdateData(ToDoListItemData itemData) {
			if (mCachedData.ContainsKey (itemData.Id)) 
			{
				mCachedData [itemData.Id] = itemData;
			}
		}

		void OnDestroy() {
			SaveManager.Save (new List<ToDoListItemData>(mCachedData.Values));
		}

		void OnApplicationQuit() {
			SaveManager.Save (new List<ToDoListItemData>(mCachedData.Values));
		}

		void OnApplicationPause(bool pause) {

			if(pause) {
				SaveManager.Save (new List<ToDoListItemData>(mCachedData.Values));
			}
		}

		void OnApplicationFocus(bool focus) {
			if (!focus) {
				SaveManager.Save (new List<ToDoListItemData>(mCachedData.Values));
			}
		}
	}

}