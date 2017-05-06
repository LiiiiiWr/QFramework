using UnityEngine;
using System.Collections;

namespace QFramework
{
    //带进度回调的执行节点
	public class ExecuteNode
	{
        protected string        mTips = "Default";
        private float           mProgress;
		private bool            mIsFinish = false;

        public virtual float progress
        {
			get { return mProgress; }
            set { mProgress = value; }
        }

        public virtual string tips
        {
            get { return mTips; }
            set { mTips = value; }
        }

        public bool isFinish
        {
            get { return mIsFinish; }
            protected set { mIsFinish = value; }
        }

        public virtual void OnBegin() { }
        public virtual void OnExecute() { }
        public virtual void OnEnd() { }
    }
}