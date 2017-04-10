using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeanCloud;
using ToDoList;
using System.Threading.Tasks;
using QFramework;

/// <summary>
/// https://leancloud.cn/docs/unity_guide.html
/// </summary>
public class AVOSItemData {
	public string ClassName = "ToDoListData";
	public string ObjectId;
	public string Title;
	public string Content;
}

public class NetManager : QMgrBehaviour {

	public static NetManager Instance {
		get {
			return QMonoSingletonComponent<NetManager>.Instance;
		}
	}


	protected override void SetupMgrId ()
	{
		mMgrId = (ushort)QMgrID.Network;
	}

	protected override void ProcessMsg (int key,QMsg msg)
	{
		
	}

	public void ModifiedItemUpload(string title,ToDoListItemData itemData) {
		DeleteItemUpload (title, delegate {
			NewItemUpload(itemData);
		});
	}

	public void DeleteItemUpload(string title,System.Action callback = null) {
		new AVQuery<AVObject> ("ToDoListItemData").WhereEqualTo ("Title", title).FindAsync ().ContinueWith (t => {
			foreach(var obj in t.Result) {
				obj["Deleted"] = true;
				obj.SaveAsync().ContinueWith(delegate {
					if (null != callback) {
						callback();
					}
				});
			}
		});
	}

	/// <summary>
	/// News the item upload.
	/// </summary>
	/// <param name="itemData">Item data.</param>
	public void NewItemUpload(ToDoListItemData itemData) {
		AVObject toDoListItemData = new AVObject ("ToDoListItemData");
		toDoListItemData ["Title"] = itemData.Title;
		toDoListItemData ["Content"] = itemData.Content;
		toDoListItemData ["Complete"] = itemData.Complete;
		toDoListItemData ["Deleted"] = itemData.Deleted;
		Task saveTask = toDoListItemData.SaveAsync ();
	}


	/// <summary>
	/// 获取所有的数据
	/// </summary>
	public void Query(System.Action<List<ToDoListItemData>> queryCallback) {
		new AVQuery<AVObject> ("ToDoListItemData").WhereNotEqualTo("Deleted",true).FindAsync().ContinueWith(t=>{
			Debug.Log("---- Query Net ----");
			var list = new List<ToDoListItemData>();
			foreach(var obj in t.Result) {
				var itemData = new ToDoListItemData();
				itemData.Title = obj["Title"] as string;
				itemData.Complete = bool.Parse(obj["Complete"].ToString());
				itemData.Content = obj["Content"] as string;
				itemData.Deleted = bool.Parse(obj["Deleted"].ToString());
				list.Add(itemData);
				itemData.Description();
			}
			Debug.Log("---- Query Net End ----");
			msgQueue.Enqueue(new QueryMsg(list,queryCallback));

		});	}

	class QueryMsg {
		System.Action<List<ToDoListItemData>> queryCallback;
		List<ToDoListItemData> list;
		public void Process() {
			queryCallback (list);
		}

		public QueryMsg(List<ToDoListItemData> list ,System.Action<List<ToDoListItemData>> queryCallback) 
		{
			this.list = list;
			this.queryCallback = queryCallback;
		}
	}

	Queue <QueryMsg> msgQueue = new Queue<QueryMsg>();

	void Update() {
		if (msgQueue.Count != 0) {
			var msg = msgQueue.Dequeue ();
			msg.Process ();
		}

	}
}
