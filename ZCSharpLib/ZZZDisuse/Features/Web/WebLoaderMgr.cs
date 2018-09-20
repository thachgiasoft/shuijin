using System.Collections.Generic;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.Web
{
    public class WebLoaderMgr
    {
        private List<WebLoader> Loaders { get; set; }
        private WebLoader CurLoader { get; set; }

        public WebLoaderMgr()
        {
            Loaders = new List<WebLoader>();
        }

        public void Open()
        {
            App.AttachTick(Loop);
        }

        public void Close()
        {
            App.DetachTick(Loop);
        }

        public void ClearAll()
        {
            for (int i = 0; i < Loaders.Count; i++)
            {
                WebLoader load = Loaders[i];
                load.Close();
            }
        }

        public WebLoader Load(string url, string savePath)
        {
            WebLoader loader = new WebLoader(url, savePath);
            Load(loader);
            return loader;
        }

        public void Load(WebLoader loader)
        {
            Loaders.Add(loader);
        }

        public void Unload(string url, string savePath)
        {
            WebLoader loader = GetLoader(url, savePath);
            if (loader != null)
            {
                Loaders.Remove(loader);
            }
            if (CurLoader != null && loader == CurLoader)
            {
                loader.Close();
                loader.Clear();
                loader = null;
            }
        }

        public WebLoader GetLoader(string url, string savePath)
        {
            WebLoader loader = Loaders.Find((t) => 
            {
                return (t.Url == url) && (t.SavePath == savePath);
            });
            return loader;
        }

        public void UnLoadAll()
        {
            if (CurLoader != null)
            {
                CurLoader.Close();
            }
            for (int i = 0; i < Loaders.Count; i++)
            {
                WebLoader oLoader = Loaders[i];
                oLoader.Close();
                oLoader.Clear();
            }
            Loaders.Clear();
        }

        public void Loop(float lastDuration)
        {
            if (CurLoader == null)
            {
                if (Loaders.Count > 0)
                {
                    CurLoader = Loaders[0];
                    CurLoader.Start();
                    Loaders.RemoveAt(0);
                }
            }

            if (CurLoader != null)
            {
                if (CurLoader.IsDone)
                {
                    CurLoader.Close();
                    CurLoader = null;
                }
            }
        }
    }
}