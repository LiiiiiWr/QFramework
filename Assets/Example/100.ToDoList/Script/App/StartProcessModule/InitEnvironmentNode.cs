using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using SCFramework;

namespace ToDoList
{
    public class InitEnvironmentNode : ExecuteNode
    {
        public override void OnBegin()
        {
            Log.i("ExecuteNode:" + GetType().Name);

			Application.targetFrameRate = 30;
			Application.runInBackground = true;
			ResMgr.Instance.InitResMgr ();

			QUIManager.Instance.SetResolution (640, 1136);
			QUIManager.Instance.SetMatchOnWidthOrHeight (1);

            Finish = true;
        }
    }
}
