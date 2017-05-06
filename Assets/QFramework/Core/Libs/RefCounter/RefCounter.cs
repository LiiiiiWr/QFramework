using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace QFramework
{
    public interface IRefCounter
    {
        int refCount
        {
            get;
        }

        void AddRef();
        void SubRef();
    }

    public class RefCounter : IRefCounter
    {
		private int mRefCount = 0;

        public int refCount
        {
            get { return mRefCount; }
        }

        public void AddRef() { ++mRefCount; }
        public void SubRef()
        {
            --mRefCount;
            if (mRefCount == 0)
            {
                OnZeroRef();
            }
        }

        protected virtual void OnZeroRef()
        {

        }
    }
}
