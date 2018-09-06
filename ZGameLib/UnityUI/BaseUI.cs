using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCSharpLib;

namespace ZGameLib.UnityUI
{
    public enum LayerUI
    {
        None,
        First,
        Last,
    }

    public abstract class BaseUI : UIItem
    {
        protected LayerUI LayerUI { get; set; }

        public override void Setup(GameObject obj)
        {
            base.Setup(obj);
            ToLayer();
        }

        public virtual void ToLast()
        {
            LayerUI = LayerUI.Last;
            ToLayer();
        }

        public virtual void ToFirst()
        {
            LayerUI = LayerUI.First;
            ToLayer();
        }

        public virtual void ToLayer()
        {
            if (CurObj != null)
            {
                if (LayerUI == LayerUI.First) CurObj.transform.SetAsFirstSibling();
                else if (LayerUI == LayerUI.Last) CurObj.transform.SetAsLastSibling();
                LayerUI = LayerUI.None;
            }
        }
    }
}
