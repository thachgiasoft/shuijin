using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using ZCSharpLib;
using ZCSharpLib.Common;
using ZGameLib.UnityUI.Exts;

namespace ZGameLib.UnityUI
{
    public abstract class UIItem
    {
        protected Dictionary<string, object> ItemTable { get; set; }
        public GameObject CurObj { get; protected set; }
        public bool IsShow { get; protected set; }

        public UIItem()
        {
            ItemTable = new Dictionary<string, object>();
        }

        public virtual void Initialize()
        {
        }

        public virtual void Uninitialize()
        {
        }

        public virtual void SetParent(GameObject parent)
        {
            CurObj.transform.SetParent(parent.transform);
            CurObj.transform.localPosition = Vector3.zero;
            CurObj.transform.localEulerAngles = Vector3.zero;
            CurObj.transform.localScale = Vector3.one;
        }

        public virtual void Setup(GameObject obj)
        {
            CurObj = obj;
            AssemblyElement(CurObj.transform);
        }

        protected virtual void AssemblyElement(Transform element, bool isFirst = true)
        {
            if (!isFirst)
            {
                ZUIElement oElement = element.GetComponent<ZUIElement>();
                if (oElement != null)
                {
                    if (ItemTable.ContainsKey(oElement.name))
                    {
                        ZLogger.Error("{0}已经包含当前元素对象{1}", GetType().Name, oElement.name);
                    }
                    else
                    {
                        ItemTable.Add(oElement.name, oElement.Get());
                    }

                    if (oElement.GetType() == typeof(ZUIItem)) return;
                }
            }
            for (int i = 0; i < element.childCount; i++)
            {
                Transform child = element.GetChild(i);
                AssemblyElement(child, false);
            }
        }

        public T Get<T>(string elementName) where T : class
        {
            object obj = null;
            if (ItemTable.TryGetValue(elementName, out obj))
            {
                return obj as T;
            }
            return null;
        }

        public virtual void Close()
        {
            if (CurObj != null)
            {
                UnityEngine.Object.DestroyImmediate(CurObj);
            }
        }

        public virtual void Show()
        {
            IsShow = true;

            OnShow();
            if (CurObj != null)
            {
                CurObj.SetActive(true);
            }
        }

        public virtual void OnShow()
        {
        }

        public virtual void Hide()
        {
            IsShow = false;

            OnHide();
            if (CurObj != null)
            {
                CurObj.SetActive(false);
            }
        }

        public virtual void OnHide()
        {
        }
    }
}
