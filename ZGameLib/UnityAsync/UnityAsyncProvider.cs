using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib;
using ZCSharpLib.Common;
using ZCSharpLib.Common.Provider;

namespace ZGameLib.UnityAsync
{
    public class UnityAsyncProvider : IZServiceProvider
    {
        private UnityAsync Async { get; set; }

        public void Register()
        {
            Async = App.Singleton<UnityAsync>();
        }

        public void Initialize()
        {
        }

        public void Uninitialize()
        {
        }

        public void Unregister()
        {
            Async.Dispose();
        }
    }
}
