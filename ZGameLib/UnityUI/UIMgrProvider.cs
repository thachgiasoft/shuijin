using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib;
using ZCSharpLib.Common.Provider;

namespace ZGameLib.UnityUI
{
    public class UIMgrProvider : IZServiceProvider
    {

        public void Register()
        {
            App.Singleton<UIMgr>();
        }

        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public void Unregister()
        {
        }
    }
}