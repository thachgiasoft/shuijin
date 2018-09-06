using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZGameLib.UnityUI.Exts
{
    [RequireComponent(typeof(ZItem))]
    public class ZUIItem : ZUIElement
    {
        public override object Get()
        {
            return gameObject;
        }
    }
}