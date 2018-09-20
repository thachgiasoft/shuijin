using System;
using System.Collections.Generic;
using ZCSharpLib;
using ZCSharpLib.Common;

namespace ZGameLib.UnityAsset
{
    public class AssetQueue
    {
        private AssetQueueCtrl[] Ctrls;

        public AssetQueue()
        {
            Asset.AssetType[] assetTypes = Enum.GetValues(typeof(Asset.AssetType)) as Asset.AssetType[];
            Ctrls = new AssetQueueCtrl[assetTypes.Length];
            for (int i = 0; i < Ctrls.Length; i++)
            {
                Ctrls[i] = new AssetQueueCtrl(assetTypes[i]);
            }
        }

        public void Open()
        {
            App.AttachTick(Loop);
        }

        public void Close()
        {
            App.DetachTick(Loop);
        }

        public void Loop(float deltaTime)
        {
            for (int i = 0; i < Ctrls.Length; i++)
            {
                Ctrls[i].Tick(deltaTime);
            }
        }

        public AssetQueueCtrl GetLoaderCtrl(Asset loader)
        {
            AssetQueueCtrl oCtrl = null;
            for (int i = 0; i < Ctrls.Length; i++)
            {
                AssetQueueCtrl ctrl = Ctrls[i];
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
            AssetQueueCtrl ctrl = GetLoaderCtrl(loader);
            if (ctrl != null) ctrl.Add(loader);
        }

        public void Remove(Asset loader)
        {
            AssetQueueCtrl ctrl = GetLoaderCtrl(loader);
            if (ctrl != null) ctrl.Remove(loader);
        }
    }

    public class AssetQueueCtrl
    {
        public Asset.AssetType AssetType { get; private set; }
        private Asset CurLoader { get; set; }
        private List<Asset> ReadyLoaders { get; set; }

        public AssetQueueCtrl(Asset.AssetType assetType)
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
