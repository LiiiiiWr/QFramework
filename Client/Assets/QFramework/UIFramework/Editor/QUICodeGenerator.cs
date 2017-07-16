using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEngine.UI;
using QFramework;
using QFramework.Libs;

public class QUICodeGenerator
{
    [MenuItem("Assets/QUIFramework - Create UICode")]
    static public void CreateUICode()
    {
		m_Instance.CreateCode(Selection.activeGameObject);
    }

    private void CreateCode(GameObject go)
    {
        if (null != go && go.name.StartsWith("UI"))
        {
            m_SelectGameObject = go;

            PrefabType prefabType = PrefabUtility.GetPrefabType(go);
            if (PrefabType.Prefab != prefabType)
            {
                return;
            }
            GameObject clone = PrefabUtility.InstantiatePrefab(go) as GameObject;
            if (null == clone)
            {
                return;
            }
            mdicNameToFullName = new Dictionary<string, string>();
            m_dicNameToTrans = new Dictionary<string,Transform> ();

            FindAllMarkTrans(clone.transform,clone.transform,"");
            CreateUIComponentsCode();

            GameObject.DestroyImmediate(clone);
        }
    }

    private void FindAllMarkTrans(Transform rootTrans,Transform curTrans,string transFullName)
    {
        for (int i = 0; i < curTrans.childCount; i++)
        {
            Transform childTrans = curTrans.GetChild(i);
            QFramework.QUIMark uiMark = childTrans.GetComponent<QFramework.QUIMark>();
            if (null != uiMark)
            {
                if (!m_dicNameToTrans.ContainsKey(childTrans.name))
                {
                    m_dicNameToTrans.Add (childTrans.name, childTrans);
                    mdicNameToFullName.Add(childTrans.name, transFullName + childTrans.name);
                }
                else
                {
                    Debug.LogError("Repeat key: " + childTrans.name);
                }
            }

            FindAllMarkTrans(rootTrans,childTrans,transFullName + childTrans.name + "/");
        }
    }

    private void CreateUIComponentsCode()
    {
        if (null != m_SelectGameObject)
        {
            string strDlg = m_SelectGameObject.name;
			string strFilePath = string.Format("{0}/{1}.cs", UIEditorPathConfig.ScriptGeneratorPath, strDlg);
            if (File.Exists(strFilePath) == false)
            {
                StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
                StringBuilder strBuilder = new StringBuilder();

                strBuilder.AppendLine("using System;");
                strBuilder.AppendLine("using System.Collections.Generic;");
                strBuilder.AppendLine("using UnityEngine;");
                strBuilder.AppendLine("using UnityEngine.UI;");
				strBuilder.AppendLine ("using QFramework;").AppendLine();
				strBuilder.AppendFormat("public class {0}Data : QUIData", strDlg);
				strBuilder.AppendLine();
				strBuilder.AppendLine("{");
				strBuilder.Append("\t").AppendLine("// TODO: get set ");
				strBuilder.AppendLine("}");

                strBuilder.AppendFormat("public class {0} : QUIBehaviour", strDlg);
                strBuilder.AppendLine();
                strBuilder.AppendLine("{");
				strBuilder.Append("\t").AppendLine("protected override void InitUI(QUIData uiData = null)");
                strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t").Append("\t").AppendLine("mUIComponents = mIComponents as " + strDlg + "Components;");
				strBuilder.Append("\t").Append("\t").AppendLine("mData = uiData as " + strDlg + "Data;");
                strBuilder.Append("\t").Append("\t").AppendLine("//please add init code here");
                strBuilder.Append("\t").AppendLine("}");
				strBuilder.Append("\t").AppendLine("public override void ProcessMsg (QMsg msg)");
				strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t\t").AppendLine("throw new System.NotImplementedException ();");
				strBuilder.Append("\t").AppendLine("}");
                strBuilder.Append("\t").AppendLine("protected override void RegisterUIEvent()");
                strBuilder.Append("\t").AppendLine("{");
                strBuilder.Append("\t").AppendLine("}");
                strBuilder.Append("\t").AppendLine("protected override void OnShow()");
				strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t\t").AppendLine("base.OnShow();");
                strBuilder.Append("\t").AppendLine("}").AppendLine();
                strBuilder.Append("\t").AppendLine("protected override void OnHide()");
				strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t\t").AppendLine("base.OnHide();");
                strBuilder.Append("\t").AppendLine("}").AppendLine();
				strBuilder.Append("\t").AppendLine("protected override void OnClose()");
				strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t\t").AppendLine("base.OnClose();");
				strBuilder.Append("\t").AppendLine("}").AppendLine();
                strBuilder.Append("\t").AppendLine("void ShowLog(string content)");
                strBuilder.Append("\t").AppendLine("{");
				strBuilder.Append("\t\t").AppendFormat("Debug.Log(\"[ {0}:]\" + content);",strDlg).AppendLine();
                strBuilder.Append("\t").AppendLine("}").AppendLine();

                //CreateUIObjectCode(ref strBuilder); //add properties
				strBuilder.Append("\t").AppendFormat("{0}Components mUIComponents = null;", strDlg).AppendLine();
				strBuilder.Append("\t").AppendFormat("{0}Data mData = null;", strDlg).AppendLine();

                strBuilder.Append("}");

                sw.Write(strBuilder);
                sw.Flush();
                sw.Close();
            }

            CreateUIBehaviorCode(strDlg);

            CreateUIFactory();

            AssetDatabase.Refresh();
            Debug.Log("Success Create UIObject Code");
        }
    }

    private void CreateUIFactory()
    {
		string strFilePath = string.Format("{0}/{1}.cs",UIEditorPathConfig.UIFactoryGeneratePath, "QUIFactory");
        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
        StringBuilder strBuilder = new StringBuilder();

        strBuilder.AppendLine("using System;");
        strBuilder.AppendLine("using QFramework;").AppendLine();

		strBuilder.AppendLine("namespace QFramework");
        strBuilder.AppendLine("{");
        strBuilder.Append("\tpublic partial class QUIFactory");
        strBuilder.AppendLine();
        strBuilder.AppendLine("\t{");

        strBuilder.Append("\t\t").AppendLine("public IUIComponents CreateUIComponentsByUIName(string uiName)");
        strBuilder.Append("\t\t").AppendLine("{");

		strBuilder.Append("\t\t\t").AppendLine("IUIComponents retComponents = null;");
		strBuilder.Append("\t\t\t").AppendLine("switch (uiName)");
        strBuilder.Append("\t\t\t").AppendLine("{");


		string[] files = Directory.GetFiles(UIEditorPathConfig.UIPrefabPath, "UI*.prefab", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            strBuilder.Append("\t\t\t\t").AppendLine("case \"" + Path.GetFileNameWithoutExtension(files[i]) + "\":");
			strBuilder.Append("\t\t\t\t\t").AppendLine("retComponents = new " + Path.GetFileNameWithoutExtension(files[i]) + "Components();");
            strBuilder.Append("\t\t\t\t\t").AppendLine("break;");
        }

        strBuilder.Append("\t\t\t").AppendLine("}");
		strBuilder.Append("\t\t\t").AppendLine("return retComponents;");
        strBuilder.Append("\t\t").AppendLine("}");

        strBuilder.AppendLine("\t}");
        strBuilder.AppendLine("}");

        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }

	private void CreateUIBehaviorCode(string behaviourName)
    {
		string strFilePath = string.Format("{0}/{1}.cs", UIEditorPathConfig.ScriptGeneratorPath , behaviourName + "Components");
        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
        StringBuilder strBuilder = new StringBuilder();

        strBuilder.AppendLine("using System;");
        strBuilder.AppendLine("using System.Collections.Generic;");
        strBuilder.AppendLine("using UnityEngine;");
        strBuilder.AppendLine("using UnityEngine.UI;");
		strBuilder.AppendLine("using QFramework;");

		strBuilder.AppendFormat("public class {0}Components : IUIComponents", behaviourName);
        strBuilder.AppendLine();
        strBuilder.AppendLine("{");

		strBuilder.Append("\t").AppendLine("public void InitUIComponents()");
        strBuilder.Append("\t").AppendLine("{");
        foreach (KeyValuePair<string, Transform> p in m_dicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.AppendFormat("\t\t\t{0}_{1} = QUIManager.Instance.Get<{2}>(\"{3}\").GetComponent<{4}>();\r\n",
                p.Key,strUIType, behaviourName, mdicNameToFullName[p.Key], strUIType);
        }
        strBuilder.Append("\t").AppendLine("}").AppendLine();

        strBuilder.Append("\t").AppendLine("public void Clear()");
        strBuilder.Append("\t").AppendLine("{");
        foreach (KeyValuePair<string, Transform> p in m_dicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.AppendFormat("\t\t{0}_{1} = null;\r\n",
				p.Key,strUIType);
        }
        strBuilder.Append("\t").AppendLine("}").AppendLine();

        foreach (KeyValuePair<string, Transform> p in m_dicNameToTrans)
        {
            string strUIType = GetUIType(p.Value);
            strBuilder.AppendFormat("\tpublic {0} {1}_{2};\r\n",
				strUIType,p.Key,strUIType);
        }

        strBuilder.AppendLine("}");

        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }

    private string GetUIType(Transform trans)
    {
        if (null != trans.GetComponent<Text>())
            return "Text";
		else if (null != trans.GetComponent<InputField>())
			return "InputField";
        else if (null != trans.GetComponent<Button>())
            return "Button";
        else if (null != trans.GetComponent<Image>())
            return "Image";
        else if (null != trans.GetComponent<RawImage>())
            return "RawImage";
        else if (null != trans.GetComponent<Toggle>())
            return "Toggle";

        else if (null != trans.GetComponent<Slider>())
            return "Slider";
        else if (null != trans.GetComponent<Scrollbar>())
            return "Scrollbar";
        else
            return "Transform";
    }

    private GameObject m_SelectGameObject = null;
        private Dictionary<string, string> mdicNameToFullName = null;
    private Dictionary<string, Transform> m_dicNameToTrans = null;
    static private QUICodeGenerator m_Instance = new QUICodeGenerator();
}
