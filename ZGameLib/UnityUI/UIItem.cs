using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCSharpLib;
using ZGameLib.UnityRuntime;
using ZGameLib.UnityUI.Element;

namespace ZGameLib.UnityUI
{
    public abstract class UIItem : IDisposable
    {
        private UnityBehaviour mBehaviour;
        private UnityBehaviour Behaviour
        {
            get
            {
                if (mBehaviour == null)
                {
                    if (Entity != null)
                    {
                        mBehaviour = Entity.AddComponent<UnityBehaviour>();
                    }
                }
                return mBehaviour;
            }
        }

        public bool IsOpened { get; protected set; }
        public GameObject Entity { get; protected set; }
        protected Dictionary<string, object> ElementDict { get; set; }

        public UIItem()
        {
            ElementDict = new Dictionary<string, object>();
        }

        public virtual void Initialize() { }
        public virtual void OnInitialize() { }
        public virtual void Uninitialize() { }
        public virtual void OnUninitialize() { }

        public virtual void SetupEntity(GameObject obj)
        {
            Entity = obj;
            AssemblyElement(Entity.transform);
            ToLayer(LayerUI);
            Initialize();
            OnInitialize();
        }

        protected virtual void AssemblyElement(Transform element, bool isFirst = true)
        {
            if (!isFirst)
            {
                ZUIElement oElement = element.GetComponent<ZUIElement>();
                if (oElement != null)
                {
                    if (ElementDict.ContainsKey(oElement.name))
                    {
                        App.Logger.Error("{0}已经包含当前元素对象{1}", GetType().Name, oElement.name);
                    }
                    else
                    {
                        ElementDict.Add(oElement.name, oElement.Get());
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

        public object FindElement(string name)
        {
            object obj = null;
            if (!ElementDict.TryGetValue(name, out obj))
            {
                App.Logger.Error("{0}没有找到元素 name={1}", GetType(), name);
            }
            return obj;
        }

        public virtual void SetParent(GameObject parent)
        {
            Entity.transform.SetParent(parent.transform);
            Entity.transform.localPosition = Vector3.zero;
            Entity.transform.localEulerAngles = Vector3.zero;
            Entity.transform.localScale = Vector3.one;
        }

        protected LayerUI LayerUI { get; set; }

        public virtual void ToLast()
        {
            ToLayer(LayerUI.Last);
        }

        public virtual void ToFirst()
        {
            ToLayer(LayerUI.First);
        }

        public virtual void ToLayer(LayerUI layer)
        {
            LayerUI = layer;
            if (Entity != null)
            {
                StartCoroutine(IEToLayer());
            }
        }

        private IEnumerator IEToLayer()
        {
            yield return null;
            if (LayerUI == LayerUI.First)
            {
                Entity.transform.SetAsFirstSibling();
            }
            else if (LayerUI == LayerUI.Last)
            {
                Entity.transform.SetAsLastSibling(); 
            }
            GameObject uroot = Global.UnityUI.Root2D;
            if (uroot != null) uroot.SendMessage("OnLayerChanged", this, SendMessageOptions.DontRequireReceiver);
        }

        public virtual void StartCoroutine(IEnumerator enumerator)
        {
            if (Behaviour != null)
            {
                Behaviour.StartCoroutine(enumerator);
            }
        }

        public virtual void StopCoroutine(IEnumerator enumerator)
        {
            if (Behaviour != null)
            {
                Behaviour.StopCoroutine(enumerator);
            }
        }

        public virtual void StopAllCoroutine()
        {
            if (Behaviour != null)
            {
                Behaviour.StopAllCoroutines();
            }
        }

        public virtual void Open()
        {
            IsOpened = true;
            Show();
        }

        protected virtual void Show()
        {
            if (Entity != null)
            {
                Entity.SetActive(true);
            }
            OnShow();
        }

        public virtual void OnShow()
        {
        }

        public virtual void Close()
        {
            IsOpened = false;
            Hide();
        }

        protected virtual void Hide()
        {
            if (Entity != null)
            {
                Entity.SetActive(false);
            }
            OnHide();
        }

        public virtual void OnHide()
        {
        }

        public void Dispose()
        {
            if (Entity != null)
            {
                UnityEngine.Object.DestroyImmediate(Entity);
            }
        }
    }
}
