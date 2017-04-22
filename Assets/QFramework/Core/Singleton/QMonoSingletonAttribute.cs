using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QMonoSingletonAttribute : System.Attribute
    {
		private string mAbsolutePath;

        public QMonoSingletonAttribute(string relativePath)
        {
            mAbsolutePath = relativePath;
        }

        public string AbsolutePath
        {
            get { return mAbsolutePath; }
        }
    }
}
