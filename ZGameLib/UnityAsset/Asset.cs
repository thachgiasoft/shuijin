using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using ZCSharpLib.ZTObject;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;
using ZCSharpLib;

namespace ZGameLib.UnityAsset
{
    public class Asset : Any
    {
        public enum AssetType
        {
            UI,         // UI界面
            Scene,      // 场景资源
            Item,       // 动态交互物件
            Unknow,      // 其他(icon, txt, xml, audio etc..)
        }
        public string Url { get; protected set; }
        public string FileName { get; private set; }
        public bool IsDone { get; protected set; }
        public bool IsSucess { get; protected set; }
        public float Progress { get; protected set; }
        public string Error { get; protected set; }
        public AssetType ThisType { get; protected set; }
        private WWW Loader { get; set; }
        private Action<Asset> Callback { get; set; }

        private AssetBundle mAssetBundle;

        private const string ASSETAUDIO = "Audio";
        private const string ASSETIMAGE = "Image";
        private const string ASSETTEXT = "Text";
        private const string ASSETBYTE = "Byte";
        private const string ASSETBUNDLE = "AssetBundle";
        private const string MAINASSETBUNDLE = "MainAssetBundle";

        private Dictionary<string, System.Object> CacheTable { get; set; }

        public Asset(string url)
        {
            Url = url;
            Parse();
            CacheTable = new Dictionary<string, System.Object>();
        }

        public AssetType Parse()
        {
            string fileName = Path.GetFileNameWithoutExtension(Url).ToLower();
            string sExt = Path.GetExtension(Url).ToLower();
            if (sExt.Equals("asset"))
            {
                string[] strs = fileName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length > 2)
                {
                    string sType = strs[1];
                    if (sType.Equals("scene")) ThisType = AssetType.Scene;
                    else if (sType.Equals("ui")) ThisType = AssetType.UI;
                    else if (sType.Equals("item")) ThisType = AssetType.Item;
                }
            }
            return ThisType;
        }

        public void Get()
        {
            Loader = new WWW(Url);
            StartLoop();
        }

        private void StartLoop()
        {
            App.AttachTick(Loop);
        }

        private void CloseLoop()
        {
            App.DetachTick(Loop);
        }

        protected virtual void Loop(float deltaTime)
        {
            bool isDone = Loader.isDone; // www 是异步操作, 所以这里用变量来记录是否下载完成，防止在同一个方法里面多次使用www.isDone获取的值不同
            IsDone = isDone;
            if (IsDone)
            {
                if (string.IsNullOrEmpty(Loader.error)) IsSucess = true;
                else { IsSucess = false; Error = Loader.error; }
                CloseLoop();
            }
            Progress = Loader.progress;
            Callback?.Invoke(this);
        }

        public string[] GetAllSceneNames()
        {
            return mAssetBundle.GetAllScenePaths();
        }

        public string[] GetAllAssetNames()
        {
            return mAssetBundle.GetAllAssetNames();
        }

        private AssetBundle GetBundle()
        {
            if (Loader.isDone && string.IsNullOrEmpty(Loader.error) && mAssetBundle == null)
            {
                mAssetBundle = Loader.assetBundle;
            }
            return mAssetBundle;
        }

        public UnityEngine.Object GetAsset(string name)
        {
            List<UnityEngine.Object> list = null;
            if (!CacheTable.ContainsKey(ASSETBUNDLE))
            {
                list = new List<UnityEngine.Object>();
                CacheTable.Add(ASSETBUNDLE, list);
            }
            AssetBundle bundle = GetBundle();
            UnityEngine.Object retObj = null;
            if (bundle != null)
            {
                retObj = list.Find((t) => { return t.name.Equals(name); });
                if (retObj == null)
                {
                    retObj = bundle.LoadAsset(name);
                    if (retObj == null)
                    {
                        App.Logger.Error("当前资源：{0} AssetBundle中没有找到对应名称 name={1} 的资源!", Url, name);
                    }
                    else { list.Add(retObj); }
                }
            }
            return retObj;
        }

        public UnityEngine.Object GetMainAsset()
        {
            System.Object obj = null;
            if (!CacheTable.TryGetValue(MAINASSETBUNDLE, out obj))
            {
                AssetBundle bundle = GetBundle();
                obj = bundle.mainAsset;
                CacheTable.Add(MAINASSETBUNDLE, obj);
            }
            if (obj != null) return obj as UnityEngine.Object;
            else return null;
        }

        public AudioClip GetAudioClip()
        {
            System.Object obj = null;
            if (!CacheTable.TryGetValue(ASSETAUDIO, out obj))
            {
                obj = Loader.GetAudioClip();
                CacheTable.Add(ASSETAUDIO, obj);
            }
            if (obj != null) return obj as AudioClip;
            else return null;
        }

        public Texture2D GetTexture()
        {
            System.Object obj = null;
            if (!CacheTable.TryGetValue(ASSETIMAGE, out obj))
            {
                obj = Loader.texture;
                CacheTable.Add(ASSETIMAGE, obj);
            }
            if (obj != null) return obj as Texture2D;
            else return null;
        }

        public string GetText()
        {
            System.Object obj = null;
            if (!CacheTable.TryGetValue(ASSETTEXT, out obj))
            {
                obj = Loader.text;
                CacheTable.Add(ASSETTEXT, obj);
            }
            if (obj != null) return obj.ToString();
            else return string.Empty;
        }

        public byte[] GetBytes()
        {
            System.Object obj = null;
            if (!CacheTable.TryGetValue(ASSETBYTE, out obj))
            {
                obj = Loader.bytes;
                CacheTable.Add(ASSETBYTE, obj);
            }
            if (obj != null) return obj as byte[];
            else return null;
        }

        public Asset AddEvent(Action<Asset> callback)
        {
            Callback = callback;
            return this;
        }

        protected override void DoManagedObjectDispose()
        {
            base.DoManagedObjectDispose();
            Loader.Dispose();
        }

        protected override void DoUnManagedObjectDispose()
        {
            base.DoUnManagedObjectDispose();
            List<string> keys = new List<string>(CacheTable.Keys);
            for (int i = 0; i < keys.Count; i++)
            {
                string key = keys[i];
                System.Object obj = CacheTable[key];
                if (key.Equals(ASSETIMAGE)) UnityEngine.Object.DestroyImmediate(obj as Texture2D);
                else if (key.Equals(ASSETAUDIO)) UnityEngine.Object.DestroyImmediate(obj as AudioClip);
            }
            if (mAssetBundle != null) { mAssetBundle.Unload(true); }
        }
    }
}