using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using QFramework;

namespace QFramework
{
    public class AbstractApplicationMgr<T> : QMonoSingleton<T> where T : QMonoSingleton<T>
    {
        protected void Start()
        {
            StartApp();
        }

        protected void StartApp()
        {
            InitThirdLibConfig();
            InitAppEnvironment();
            StartGame();
        }

        #region 子类实现

        protected virtual void InitThirdLibConfig()
        {

        }

        protected virtual void InitAppEnvironment()
        {

        }

        protected virtual void StartGame()
        {

        }

        #endregion
    }
}
