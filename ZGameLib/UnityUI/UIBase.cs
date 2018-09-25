using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCSharpLib;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;
using ZCSharpLib.ZTUtils;
using ZGameLib.UnityAsset;
using ZGameLib.UnityUI.Element;

namespace ZGameLib.UnityUI
{
    public abstract class UIBase : UIItem
    {
        private void CreateEntity()
        {
            string assetName = GetType().Name;
            string assetPath = Global.UnityUI.CombinePath(assetName);
            ZEventHelper zEventHelper = new ZEventHelper(null);
            App.Singleton<AssetMgr>().Load(assetPath, (t) =>
            {
                if (t.IsDone && t.IsSucess)
                {
                    GameObject oPrefab = t.GetAsset(assetName) as GameObject;
                    GameObject ins = Object.Instantiate(oPrefab);
                    ins.name = oPrefab.name;
                    SetupEntity(ins);
                    SetParent(Global.UnityUI.Root2D);
                    base.Open();
                }
                else if (t.IsDone && !t.IsSucess)
                {
                    App.Logger.Error("UI资源没有下载完成, 无法创建! 资源地址: {0}", assetPath);
                }
            });
        }

        public override void Open()
        {
            if (Entity == null)
            {
                CreateEntity();
            }
            else
            {
                base.Open();
            }
        }
    }
}
