using System;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace QFramework
{
    public class NetImageRes : AbstractRes, IDownloadTask
    {
        private string      mUrl;
        private string      mHashCode;
        private object      mRawAsset;
        private WWW         mWWW;

        public static NetImageRes Allocate(string name)
        {
            NetImageRes res = ObjectPool<NetImageRes>.Instance.Allocate();
            if (res != null)
            {
                res.AssetName = name;
                res.SetUrl(name.Substring(9));
            }
            return res;
        }

        public static string localPhotoFolderPath
        {
            get
            {
                return FilePath.persistentDataPath4Photo;
            }
        }

        public string localResPath
        {
            get
            {
                return string.Format("{0}{1}", localPhotoFolderPath, mHashCode);
            }
        }

        public override object RawAsset
        {
            get { return mRawAsset; }
        }

        public bool needDownload
        {
            get
            {
                return RefCount > 0;
            }
        }

        public string url
        {
            get
            {
                return mUrl;
            }
        }

        public void SetUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            mUrl = url;
            mHashCode = string.Format("Photo_{0}", mUrl.GetHashCode());
        }

        public override bool UnloadImage(bool flag)
        {
            return false;
        }

        public override bool LoadSync()
        {
            return false;
        }

        public override void LoadAsync()
        {
            if (!CheckLoadAble())
            {
                return;
            }

            if (string.IsNullOrEmpty(mAssetName))
            {
                return;
            }

            DoLoadWork();
        }

        private void DoLoadWork()
        {
            ResState = eResState.kLoading;

            OnDownLoadResult(true);

            //检测本地文件是否存在
            /*
            if (!File.Exists(LocalResPath))
            {
                ResDownloader.Instance.AddDownloadTask(this);
            }
            else
            {
                OnDownLoadResult(true);
            }
            */
        }

        protected override void OnReleaseRes()
        {
            if (mAsset != null)
            {
                GameObject.Destroy(mAsset);
                mAsset = null;
            }

            mRawAsset = null;
        }

        public override void Recycle2Cache()
        {
            ObjectPool<NetImageRes>.Instance.Recycle(this);
        }

        public override void OnCacheReset()
        {

        }

        public void DeleteOldResFile()
        {
            //throw new NotImplementedException();
        }

        public void OnDownLoadResult(bool result)
        {
            if (!result)
            {
                OnResLoadFaild();
                return;
            }

            if (RefCount <= 0)
            {
                ResState = eResState.kWaiting;
                return;
            }

            ResMgr.Instance.PostIEnumeratorTask(this);
            //ResMgr.Instance.PostLoadTask(LoadImage());
        }

        //完全的WWW方式,Unity 帮助管理纹理缓存，并且效率貌似更高
        public override IEnumerator StartIEnumeratorTask(Action finishCallback)
        {
            if (RefCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            WWW www = new WWW(mUrl);

            mWWW = www;

            yield return www;

            mWWW = null;

            if (www.error != null)
            {
                Log.e(string.Format("Res:{0}, WWW Errors:{1}", mUrl, www.error));
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            if (!www.isDone)
            {
                Log.e("NetImageRes WWW Not Done! Url:" + mUrl);
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;

                yield break;
            }

            if (RefCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;
                yield break;
            }

            //TimeDebugger dt = new TimeDebugger("Tex");
            //dt.Begin("LoadingTask");
            //这里是同步的操作
            mAsset = www.texture;
            //dt.End();

            www.Dispose();
            www = null;

            //dt.Dump(-1);

            ResState = eResState.kReady;

            finishCallback();
        }

        protected override float CalculateProgress()
        {
            if (mWWW == null)
            {
                return 0;
            }

            return mWWW.progress;
        }

        /*
        public IEnumerator StartIEnumeratorTask2(Action finishCallback)
        {
            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            WWW www = new WWW("file://" + LocalResPath);
            yield return www;
            if (www.error != null)
            {
                Log.e("WWW Error:" + www.error);
                OnResLoadFaild();
                finishCallback();
                yield break;
            }

            if (!www.isDone)
            {
                Log.e("NetImageRes WWW Not Done! Url:" + mUrl);
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;

                yield break;
            }

            if (refCount <= 0)
            {
                OnResLoadFaild();
                finishCallback();

                www.Dispose();
                www = null;
                yield break;
            }

            TimeDebugger dt = new TimeDebugger("Tex");
            dt.Begin("LoadingTask");
            Texture2D tex = www.texture;
            tex.Compress(true);
            dt.End();
            dt.Dump(-1);

            mAsset = tex;
            www.Dispose();
            www = null;

            resState = eResState.kReady;

            finishCallback();
        }
        */
    }
}
