using System.Collections.Generic;
using ZGameLib.UnityAsset.Loader;
using ZCSharpLib.Common;
using System;

namespace ZGameLib.UnityAsset
{
    public class AssetMgr
    {
        private Dictionary<string, Asset> AssetCache { get; set; }

        public AssetQueue AssetQueue { get; private set; }

        public AssetMgr()
        {
            AssetQueue = new AssetQueue();
            AssetCache = new Dictionary<string, Asset>();
        }

        public void Open()
        {
            if (AssetQueue != null) AssetQueue.Open();
        }

        public void Close()
        {
            if (AssetQueue != null) AssetQueue.Close();
        }

        private Asset Find(string url)
        {
            Asset asset = null;
            if (!AssetCache.TryGetValue(url, out asset)) { }
            return asset;
        }

        public void Load(string url, Action<Asset> callback = null)
        {
            Asset asset = Find(url);
            if (asset == null)
            {
                asset = new Asset(url);
                asset.AddEvent(callback);
                AssetCache.Add(url, asset);
                AssetQueue.Add(asset);
            }
            else
            {
                callback(asset);
            }
        }

        public void Unload(string url)
        {
            Asset asset = Find(url);
            if (asset != null)
            {
                AssetQueue.Remove(asset);
                asset.Dispose();
            }
            AssetCache.Remove(url);
        }

        public AssetAllQueue LoadAll(string[] urls)
        {
            AssetAllQueue allLoader = new AssetAllQueue();
            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                Asset asset = Find(url);
                if (asset == null) { allLoader.AddLoad(url); }
                else
                {
                    ZLogger.Error("资源url={0}已经存在,无需重复下载!", url);
                }
            }
            return allLoader;
        }

        public void UnLoadAll()
        {
            foreach (var item in AssetCache.Values)
            {
                item.Dispose();
            }
            AssetCache.Clear();
        }
    }
}

