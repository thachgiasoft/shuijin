using System;
using System.Collections.Generic;
using UnityEngine;

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
            if (UIDict.TryGetValue(typeName, out item)){}
            return item;
        }

        public T Open<T>() where T : UIItem
        {
            return Open(typeof(T)) as T;
        }

        public UIItem Open(Type type)
        {
            UIItem item = Find(type.Name);
            if (item == null)
            {
                item = Activator.CreateInstance(type) as UIItem;
                UIDict.Add(type.Name, item);
            }
            Open(type.Name);
            return item;
        }

        private UIItem Open(string typeName)
        {
            UIItem item = Find(typeName);
            if (item != null) item.Open();
            return item;
        }

        public T Close<T>() where T : UIBase
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
