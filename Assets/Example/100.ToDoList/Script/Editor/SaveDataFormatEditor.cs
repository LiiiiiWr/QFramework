using UnityEditor;

/// <summary>
/// 编辑器下选择用数据存储格式：json/protobuf
/// </summary>
public static class SaveDataFormatEditor
{
	private const string MENU_PROTOBUF = "QFramework/Example/100.TodoList/SaveDataFormat/Protobuf";
	private const string MENU_JSON = "QFramework/Example/100.TodoList/SaveDataFormat/Json";
	private const string KEY_USEPROTOBUF = "UseProtobuf";

	[MenuItem(SaveDataFormatEditor.MENU_JSON, false, 1)]
	public static void SaveDataJson()
	{
		EditorPrefs.SetBool("UseProtobuf", false);
	}

	[MenuItem(SaveDataFormatEditor.MENU_JSON, true)]
	public static bool SaveDataJsonValidate()
	{
		bool useProtobuf = EditorPrefs.GetBool(SaveDataFormatEditor.KEY_USEPROTOBUF, false);

		Menu.SetChecked(SaveDataFormatEditor.MENU_JSON, !useProtobuf);
		Menu.SetChecked(SaveDataFormatEditor.MENU_PROTOBUF, useProtobuf);

		return true;
	}

	[MenuItem(SaveDataFormatEditor.MENU_PROTOBUF, false, 2)]
	public static void SaveDataProtobuf()
	{
		EditorPrefs.SetBool("UseProtobuf", true);
	}

	[MenuItem(SaveDataFormatEditor.MENU_PROTOBUF, true)]
	public static bool SaveDataProtobufValidate()
	{
		bool useProtobuf = EditorPrefs.GetBool(SaveDataFormatEditor.KEY_USEPROTOBUF, false);

		Menu.SetChecked(SaveDataFormatEditor.MENU_JSON, !useProtobuf);
		Menu.SetChecked(SaveDataFormatEditor.MENU_PROTOBUF, useProtobuf);
		return true;
	}
}
