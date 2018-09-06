using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZCSharpLib;
using ZCSharpLib.Common;
using ZGameLib.UnityAsset;

namespace ZGameLib.UnityUI
{
    public class UIMgr
    {
        private Dictionary<string, BaseUI> UITable { get; set; }

        private Transform UIRoot { get; set; }

        private string AssetPath { get; set; }

        public UIMgr()
        {
            UITable = new Dictionary<string, BaseUI>();
        }

        public void Setup(Transform root, string assetPath)
        {
            UIRoot = root;
            AssetPath = assetPath;
        }

        private BaseUI Open(string name)
        {
            BaseUI uibase = null;
            if (UITable.TryGetValue(name, out uibase))
            {
                uibase.Show();
            }
            return uibase;
        }

        public BaseUI Find(string name)
        {
            BaseUI uibase = null;
            if (!UITable.TryGetValue(name, out uibase))
            {
            }
            return uibase;
        }

        public T Open<T>() where T : BaseUI
        {
            return Open(typeof(T)) as T;
        }

        public BaseUI Open(Type type)
        {
            BaseUI uibase = Find(type.Name);
            if (uibase == null)
            {
                uibase = Activator.CreateInstance(type) as BaseUI;
                UITable.Add(type.Name, uibase);
                string url = AssetPath + "/" + type.Name;
                LoadUI(url);
            }
            Open(type.Name);
            return uibase;
        }

        public virtual void LoadUI(string url)
        {
            Asset oAsset = App.Singleton<AssetMgr>().Make(url);
            if (oAsset.IsDone) OnLoadedHandler(oAsset);
            else oAsset.SetEventLoaded(OnLoadedHandler);
        }

        private void OnLoadedHandler(Asset oAsset)
        {
            if (oAsset.IsSucess)
            {
                GameObject oPrefab = oAsset.GetMainAsset() as GameObject;
                GameObject ins = UnityEngine.Object.Instantiate(oPrefab) as GameObject;
                ins.name = oPrefab.name;
                Setup(ins);
            }
            else
            {
                ZLogger.Error("UI资源没有下载完成, 无法创建! 资源地址: {0}", oAsset.Url);
            }
        }

        private void UnloadUI(string url)
        {
            App.Singleton<AssetMgr>().Unmake(url);
        }

        private void Setup(GameObject obj)
        {
            BaseUI oBaseUI = null;
            if (UITable.TryGetValue(obj.name, out oBaseUI))
            {
                oBaseUI.Setup(obj as GameObject);
                oBaseUI.SetParent(UIRoot.gameObject);
                oBaseUI.Initialize();
                if (oBaseUI.IsShow)
                {
                    oBaseUI.Show();
                }
                else
                {
                    oBaseUI.Hide();
                }
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(obj);
                string url = AssetPath + "/" + obj.name;
                UnloadUI(url);
            }
        }

        public T Close<T>(bool isDisposable = false) where T : BaseUI
        {
            return Close(typeof(T)) as T;
        }

        private BaseUI Close(string name, bool isDisposable = false)
        {
            BaseUI uibase = null;
            if (!UITable.TryGetValue(name, out uibase))
            {
                ZLogger.Error("没有找到UI：{0}!", name);
            }
            if (uibase.IsShow)
            {
                uibase.Hide();
            }
            if (isDisposable)
            {
                UITable.Remove(name);
                uibase.Uninitialize();
                uibase.Close();
            }
            return uibase;
        }

        public BaseUI Close(Type type, bool isDisposable = false)
        {
            BaseUI uibase = Close(type.Name, isDisposable);
            return uibase;
        }

        public void CloseAll(params Type[] without)
        {
            CloseAndDisposeAll(false, without);
        }

        public void CloseAndDisposeAll(bool isDispose = false, params Type[] without)
        {
            foreach (var item in UITable.Keys)
            {
                bool isNeed = true;
                if (without != null)
                {
                    for (int i = 0; i < without.Length; i++)
                    {
                        if (item == without[i].Name) { isNeed = false; break; }
                    }
                }
                if (isNeed) Close(item, isDispose);
            }
        }
    }
}
