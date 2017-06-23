using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example {
	/// <summary>
	/// 测试IOPathEditor生成的路径
	/// </summary>
	public class IOPathEditorExample : MonoBehaviour {

		void Start () {
			Debug.Log (ExamplePathConfig.ExampleEditorPathDir);
			Debug.Log (ExamplePathConfig.ExamplePersistentPathDir);
		}

	}
}