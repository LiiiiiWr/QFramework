using System.Collections.Generic;

namespace QFramework
{
    public class ExecuteNodeContainer
    {
        #region Event
        public QVoidDelegate.WithGeneric<float>     OnExecuteScheduleEvent;
        public QVoidDelegate.WithGeneric<string>    OnExecuteTipsEvent;
        public QVoidDelegate.WithVoid               OnExecuteContainerBeginEvent;
        public QVoidDelegate.WithVoid               OnExecuteContainerEndEvent;
        #endregion

        #region 属性&字段
		private List<ExecuteNode>   mNodeList;
		private int                 mCurrentIndex;
		private ExecuteNode         mCurrentNode;

		private float               mTotalSchedule = 0;
        #endregion

        public float TotalSchedule
        {
            get { return mTotalSchedule; }
        }

        public ExecuteNode CurrentNode
        {
            get
            {
                return mCurrentNode;
            }
        }

        public void Append(ExecuteNode item)
        {
            if (mNodeList == null)
            {
                mNodeList = new List<ExecuteNode>();
                mCurrentIndex = -1;
            }

            mNodeList.Add(item);
        }

        public void Start()
        {
            mCurrentIndex = -1;
            MoveToNextUpdateFunc();
        }

        public void Update()
        {
            if (mCurrentNode != null)
            {
                mCurrentNode.OnExecute();

                float schedule = mCurrentNode.Progress;

                mTotalSchedule = mCurrentIndex * (1.0f / mNodeList.Count) + schedule / mNodeList.Count;

                if (OnExecuteScheduleEvent != null)
                {
                    OnExecuteScheduleEvent(mTotalSchedule);
                }

                if (mCurrentNode.Finish)
                {
                    MoveToNextUpdateFunc();
                }
            }
        }

        private void MoveToNextUpdateFunc()
        {
            if (mCurrentNode != null)
            {
                mCurrentNode.OnEnd();
            }

            ++mCurrentIndex;
            if (mCurrentIndex >= mNodeList.Count)
            {
                mTotalSchedule = 1.0f;
                mCurrentNode = null;

                if (OnExecuteContainerEndEvent != null)
                {
                    OnExecuteContainerEndEvent();

                    OnExecuteContainerEndEvent = null;
                }
            }
            else
            {
                mCurrentNode = mNodeList[mCurrentIndex];
                mCurrentNode.OnBegin();

                if (mCurrentIndex == 0)
                {
                    if (OnExecuteContainerBeginEvent != null)
                    {
                        OnExecuteContainerBeginEvent();
                    }
                }

                if (OnExecuteTipsEvent != null)
                {
                    OnExecuteTipsEvent(mCurrentNode.Tips);
                }
            }
        }

    }

}
