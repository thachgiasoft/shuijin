using System;
using System.Collections.Generic;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.Web
{
    public class WebAllLoader
    {
        public class WebAssetDir
        {
            public string Url { get; set; }
            public string SavePath { get; set; }
        }

        public int LoadIndex { get; private set; }
        public int LoadNum { get; private set; }
        public string[] Urls { get; set; }
        public string[] SavePaths { get; set; }
        public float SingleProgress { get; private set; }
        public float FinalProgress { get; private set; }
        public bool IsDone { get; private set; }
        public bool IsStart { get; set; }
        private List<WebLoader> WebLoaders { get; set; }
        private List<WebLoader> AlreadyLoaders { get; set; }
        private Action<WebAllLoader> OnAllLoaded { get; set; }
        private Action<WebAllLoader> OnAllLoading { get; set; }
        private Action<WebAllLoader> OnError { get; set; }
        private Action<WebLoader> OnSingleLoaded { get; set; }
        private Action<WebLoader> OnSingleLoading { get; set; }

        private WebLoader mCurLoader;
        private float mTotalProgress;

        public WebAllLoader()
        {
            WebLoaders = new List<WebLoader>();
            AlreadyLoaders = new List<WebLoader>();
        }

        public void SetEventSingleLoaded(Action<WebLoader> onSingleLoaded)
        {
            OnSingleLoaded = onSingleLoaded;
        }

        public void SetEventSingleLoading(Action<WebLoader> onSingleLoading)
        {
            OnSingleLoading = onSingleLoading;
        }

        public void SetEventAllLoaded(Action<WebAllLoader> onAllLoaded)
        {
            OnAllLoaded = onAllLoaded;
        }

        public void SetEventAllLoading(Action<WebAllLoader> onAllLoading)
        {
            OnAllLoading = onAllLoading;
        }

        public void SetEventOnError(Action<WebAllLoader> onError)
        {
            OnError = onError;
        }

        public void LoadAll(WebAssetDir[] dirs)
        {
            LoadNum = dirs.Length;
            Urls = new string[LoadNum];
            SavePaths = new string[LoadNum];
            for (int i = 0; i < dirs.Length; i++)
            {
                WebAssetDir dir = dirs[i];
                Urls[i] = dir.Url;
                SavePaths[i] = dir.SavePath;
                WebLoader oLoader = new WebLoader(Urls[i], SavePaths[i]);
                oLoader.SetEventLoading(SingleLoading);
                oLoader.SetEventLoaded(SingleLoaded);
                WebLoaders.Add(oLoader);
            }
        }

        public void UnloadAll()
        {
            WebLoaders.Clear();
            for (int i = 0; i < AlreadyLoaders.Count; i++)
            {
                WebLoader oLoader = AlreadyLoaders[i];
                if(oLoader.IsStart) oLoader.Close();
                oLoader.Clear();
            }
        }

        public WebLoader GetLoader(string url, string savePath)
        {
            WebLoader oLoader = WebLoaders.Find((t)=> { return t.Url == url && t.SavePath == savePath; });
            return oLoader;
        }

        private void SingleLoading(WebLoader loader)
        {
            OnSingleLoading?.Invoke(loader);
            AllLoading(loader);
        }

        private void SingleLoaded(WebLoader loader)
        {
            if (!string.IsNullOrEmpty(loader.Error))
            {
                SingleError(loader);
                return;
            }
            OnSingleLoaded?.Invoke(loader);
        }

        private void SingleError(WebLoader loader)
        {
            loader.Close();
            for (int i = 0; i < WebLoaders.Count; i++)
            {
                WebLoader oLoader = WebLoaders[i];
                oLoader.Clear();
            }
            WebLoaders.Clear();
            Close();
            OnError?.Invoke(this);
        }


        private void AllLoading(WebLoader loader)
        {
            SingleProgress = loader.Progress;
            mTotalProgress = LoadIndex + SingleProgress;
            FinalProgress = mTotalProgress / LoadNum;
            if (loader.IsDone) LoadIndex = LoadIndex + 1;
            if (LoadIndex == LoadNum) IsDone = true;
            OnAllLoading?.Invoke(this);
            if (IsDone)
            {
                OnAllLoaded?.Invoke(this);
            }
        }

        public void Start()
        {
            IsStart = true;
            App.AttachTick(Loop);
        }

        public void Close()
        {
            IsStart = false;
            App.DetachTick(Loop);
        }

        public void Loop(float deltaTime)
        {
            if (mCurLoader == null)
            {
                if (WebLoaders.Count > 0)
                {
                    mCurLoader = WebLoaders[0];
                    WebLoaders.RemoveAt(0);
                    mCurLoader.Start();
                    AlreadyLoaders.Add(mCurLoader);
                }
            }
            if (mCurLoader != null)
            {
                if (mCurLoader.IsDone)
                {
                    mCurLoader.Close();
                    mCurLoader = null;
                }
            }
        }
    }
}