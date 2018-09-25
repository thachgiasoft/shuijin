using System;
using System.Collections.Generic;
using ZCSharpLib;
using ZCSharpLib.Common;

namespace ZGameLib.UnityAsset.Loader
{
    /// <summary>
    /// 所以资源一起加载的类
    /// </summary>
    public class AssetAllQueue
    {
        public int LoadCount { get; protected set; }
        public int LoadIndex { get; protected set; }
        public float SingleProgress { get; protected set; }
        public float FinalProgress { get; protected set; }
        public bool IsDone { get; protected set; }

        private float mTotalProgress;
        private List<Asset> mItems;
        private Action<AssetAllQueue> mAllEvent;
        private Action<Asset> mSingleEvent;

        private Asset mCurItem;
        private bool mIsStart;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="allLoading">所有资源正在加载中的回掉方法</param>
        /// <param name="allLoaded">所有资源加载完成后的回掉方法</param>
        public AssetAllQueue()
        {
            mItems = new List<Asset>();
        }

        public void SetEventAllLoading(Action<AssetAllQueue> allLoading)
        {
            mAllEvent = allLoading;
        }

        public void SetEventSingleLoading(Action<Asset> loading)
        {
            mSingleEvent = loading;
        }

        /// <summary>
        /// 添加加载目标
        /// </summary>
        /// <param name="path">加载地址</param>
        /// <param name="item">资源加载完成后的回调</param>
        public void AddLoad(string path)
        {
            LoadCount = LoadCount + 1;
            Asset loader = new Asset(path);
            loader.AddEvent(OnSingleLoadingHandler);
            mItems.Add(loader);
        }

        private void OnSingleLoadingHandler(Asset loader)
        {
            mSingleEvent?.Invoke(loader);
            AllLoading(loader);
        }

        private void AllLoading(Asset loader)
        {
            SingleProgress = loader.Progress;
            mTotalProgress = LoadIndex + SingleProgress;
            FinalProgress = mTotalProgress / LoadCount;
            if (loader.IsDone) LoadIndex = LoadIndex + 1;
            if (LoadIndex == LoadCount) IsDone = true;
            mAllEvent?.Invoke(this);
        }

        /// <summary>
        /// 开始执行加载
        /// </summary>
        public void Start()
        {
            mIsStart = true;
            // 当加载对象为0时直接处理回调
            if (mItems.Count == 0)
            {
                mAllEvent?.Invoke(this);
            }
            else
            {
                App.AttachTick(Loop);
            }
        }

        public void Close()
        {
            App.DetachTick(Loop);
        }

        public void Loop(float deltaTime)
        {
            if (!mIsStart) return;

            if (mCurItem == null)
            {
                if (mItems.Count > 0)
                {
                    mCurItem = mItems[0];
                    mItems.RemoveAt(0);
                    mCurItem.Get();
                }
            }
            if (mCurItem != null)
            {
                if (mCurItem.IsDone)
                {
                    mCurItem = null;
                }
            }
        }
    }
}