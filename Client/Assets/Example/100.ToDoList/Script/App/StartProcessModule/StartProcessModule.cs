using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace ToDoList
{
	/// <summary>
	/// 启动进程，一些初始爱护操作
	/// </summary>
    public class StartProcessModule : AbstractStartProcess
    {

        protected override void InitExecuteContainer()
        {
            Append(new InitEnvironmentNode());
        }
    }
}
