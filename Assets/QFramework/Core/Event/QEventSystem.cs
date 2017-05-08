using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SCFramework;
using QFramework;

namespace QFramework
{

    #region 事件接口
    public delegate void OnEvent(int key, params object[] param);
    #endregion

    public class QEventSystem : QSingleton<QEventSystem>, ICacheAble
    {
		private bool        mCacheFlag = false;
		private Dictionary<int, ListenerWrap> mAllListenerMap = new Dictionary<int, ListenerWrap>(50);
		
		public QEventSystem() {}

        public bool CacheFlag
        {
            get
            {
                return mCacheFlag;
            }

            set
            {
                mCacheFlag = value;
            }
        }

        #region 内部结构
        private class ListenerWrap
        {
			private LinkedList<OnEvent>     mEventList;

            public bool Fire(int key, params object[] param)
            {
                if (mEventList == null)
                {
                    return false;
                }

                LinkedListNode<OnEvent> next = mEventList.First;
                OnEvent call = null;
                LinkedListNode<OnEvent> nextCache = null;

                while (next != null)
                {
                    call = next.Value;
                    nextCache = next.Next;
                    call(key, param);

                    //1.该事件的回调删除了自己OK 2.该事件的回调添加了新回调OK， 3.该事件删除了其它回调(被删除的回调可能有回调，可能没有)
                    next = (next.Next == null) ? nextCache : next.Next;
                }

                return true;
            }

            public bool Add(OnEvent listener)
            {
                if (mEventList == null)
                {
                    mEventList = new LinkedList<OnEvent>();
                }

                if (mEventList.Contains(listener))
                {
                    return false;
                }

                mEventList.AddLast(listener);
                return true;
            }

            public void Remove(OnEvent listener)
            {
                if (mEventList == null)
                {
                    return;
                }

                mEventList.Remove(listener);
            }
        }
        #endregion

        #region 功能函数
        public bool Register<T>(T key, OnEvent fun) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if (!mAllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap = new ListenerWrap();
                mAllListenerMap.Add(kv, wrap);
            }

            if (wrap.Add(fun))
            {
                return true;
            }

            Log.w("Already Register Same Event:" + key);
            return false;
        }

        public void UnRegister<T>(T key, OnEvent fun) where T : IConvertible
        {
            ListenerWrap wrap;
            if (mAllListenerMap.TryGetValue(key.ToInt32(null), out wrap))
            {
                wrap.Remove(fun);
            }
        }

        public bool Send<T>(T key, params object[] param) where T : IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if (mAllListenerMap.TryGetValue(kv, out wrap))
            {
                return wrap.Fire(kv, param);
            }
            return false;
        }

        public void OnCacheReset()
        {
            mAllListenerMap.Clear();
        }

        #endregion
    }
}
