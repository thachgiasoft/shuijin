using System;
using System.Collections.Generic;
using ZCSharpLib.Common;

namespace ZGameLib.UnityAsset.Loader
{
    public class AssetLoader : ITick
    {
        private AssetLoaderCtrl[] Ctrls;

        public AssetLoader()
        {
            Asset.AssetType[] assetTypes = Enum.GetValues(typeof(Asset.AssetType)) as Asset.AssetType[];
            Ctrls = new AssetLoaderCtrl[assetTypes.Length];
            for (int i = 0; i < Ctrls.Length; i++)
            {
                Ctrls[i] = new AssetLoaderCtrl(assetTypes[i]);
            }
        }

        public void Open()
        {
            ZCSharpLib.Common.Tick.Attach(this);
        }

        public void Close()
        {
            ZCSharpLib.Common.Tick.Detach(this);
        }

        public void Loop(float deltaTime)
        {
            for (int i = 0; i < Ctrls.Length; i++)
            {
                Ctrls[i].Tick(deltaTime);
            }
        }

        public AssetLoaderCtrl GetLoaderCtrl(Asset loader)
        {
            AssetLoaderCtrl oCtrl = null;
            for (int i = 0; i < Ctrls.Length; i++)
            {
                AssetLoaderCtrl ctrl = Ctrls[i];
                if (ctrl.AssetType == loader.ThisType)
                {
                    oCtrl = ctrl;
                    break;
                }
            }
            if (oCtrl == null) ZLogger.Error("没有找到资源url={0}对应的资源类型type={1}", loader.Url, loader.ThisType);
            return oCtrl;
        }

        public void Add(Asset loader)
        {
            AssetLoaderCtrl ctrl = GetLoaderCtrl(loader);
            if (ctrl != null) ctrl.Add(loader);
        }

        public void Remove(Asset loader)
        {
            AssetLoaderCtrl ctrl = GetLoaderCtrl(loader);
            if (ctrl != null) ctrl.Remove(loader);
        }
    }

    public class AssetLoaderCtrl
    {
        public Asset.AssetType AssetType { get; private set; }
        private Asset CurLoader { get; set; }
        private List<Asset> ReadyLoaders { get; set; }

        public AssetLoaderCtrl(Asset.AssetType assetType)
        {
            AssetType = assetType;
            ReadyLoaders = new List<Asset>();
        }

        public void Add(Asset loader)
        {
            ReadyLoaders.Add(loader);
        }

        public void Remove(Asset loader)
        {
            string url = loader.Url;
            int index = -1;
            for (int i = 0; i < ReadyLoaders.Count; i++)
            {
                if (ReadyLoaders[i].Url == url) { index = i; break; }
            }
            if (index != -1)
            {
                Asset oLoader = ReadyLoaders[index];
                ReadyLoaders.RemoveAt(index);
            }
            if (CurLoader != null && CurLoader.Url == url)
            {
                CurLoader.Dispose();
                CurLoader = null;
            }
        }

        public void Tick(float deltaTime)
        {
            if (CurLoader == null)
            {
                if (ReadyLoaders.Count > 0)
                {
                    CurLoader = ReadyLoaders[0];
                    ReadyLoaders.RemoveAt(0);
                    CurLoader.StartLoad();
                }
            }
            if (CurLoader != null)
            {
                if (CurLoader.IsDone)
                {
                    CurLoader.CloseLoad();
                    CurLoader = null;
                }
            }
        }
    }
}
