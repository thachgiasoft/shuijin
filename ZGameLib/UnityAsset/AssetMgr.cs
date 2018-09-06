using System.Collections.Generic;
using ZGameLib.UnityAsset.Loader;
using ZCSharpLib.Common;

namespace ZGameLib.UnityAsset
{
    public class AssetMgr
    {
        public AssetLoader AssetLoader { get; private set; }
        private Dictionary<string, Asset> AssetTable { get; set; }

        public AssetMgr()
        {
            AssetLoader = new AssetLoader();
            AssetTable = new Dictionary<string, Asset>();
        }

        public void Open()
        {
            if (AssetLoader != null) AssetLoader.Open();
        }

        public void Close()
        {
            if (AssetLoader != null) AssetLoader.Close();
        }

        public Asset GetAsset(string url)
        {
            Asset asset = null;
            if (!AssetTable.TryGetValue(url, out asset)) { }
            return asset;
        }

        public Asset Make(string url)
        {
            Asset asset = GetAsset(url);
            if (asset == null)
            {
                asset = new Asset(url);
                AssetTable.Add(url, asset);
                AssetLoader.Add(asset);
            }
            return asset;
        }

        public void Unmake(string url)
        {
            Asset asset = GetAsset(url);
            if (asset != null)
            {
                AssetLoader.Remove(asset);
                asset.Dispose();
            }
            AssetTable.Remove(url);
        }

        public AssetAllLoader LoadAll(string[] urls)
        {
            AssetAllLoader allLoader = new AssetAllLoader();
            for (int i = 0; i < urls.Length; i++)
            {
                string url = urls[i];
                Asset asset = GetAsset(url);
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
            foreach (var item in AssetTable.Values)
            {
                item.Dispose();
            }
            AssetTable.Clear();
        }
    }
}

