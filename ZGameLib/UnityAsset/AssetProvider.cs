using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib;
using ZCSharpLib.Common.Provider;

namespace ZGameLib.UnityAsset
{
    public class AssetProvider : IZServiceProvider
    {
        public void Register()
        {
            App.Singleton<AssetMgr>();
        }

        public void Initialize()
        {
            App.Singleton<AssetMgr>().Open();
        }

        public void Uninitialize()
        {
            App.Singleton<AssetMgr>().Close();
        }

        public void Unregister()
        {
        }
    }
}
