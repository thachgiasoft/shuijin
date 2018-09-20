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
        private Dictionary<string, UIItem> UIDict { get; set; }

        private Transform UIRoot { get; set; }

        private string AssetPath { get; set; }

        public UIMgr()
        {
            UIDict = new Dictionary<string, UIItem>();
        }

        public UIItem Find(string typeName)
        {
            UIItem item = null;
            if (!UIDict.TryGetValue(typeName, out item))
            {
                ZLogger.Error("没有找到当前名称[{0}]的UI", typeName);
            }
            return item;
        }

        public T Open<T>() where T : UIItem
        {
            return Open(typeof(T)) as T;
        }

        public UIItem Open(Type type)
        {
            UIItem uibase = Find(type.Name);
            if (uibase == null)
            {
                uibase = Activator.CreateInstance(type) as UIItem;
                UIDict.Add(type.Name, uibase);
            }
            Open(type.Name);
            return uibase;
        }

        private UIItem Open(string typeName)
        {
            UIItem item = Find(typeName);
            if (item != null) item.Open();
            return item;
        }

        public T Close<T>(bool isDisposable = false) where T : UIBase
        {
            return Close(typeof(T)) as T;
        }

        public UIItem Close(Type type)
        {
            UIItem item = Close(type.Name);
            return item;
        }

        private UIItem Close(string typeName)
        {
            UIItem item = Find(typeName);
            if (item != null) item.Close();
            return item;
        }
    }
}
