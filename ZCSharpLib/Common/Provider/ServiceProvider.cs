using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Common.Provider
{
    public class ServiceProvider
    {
        private IZServiceProvider[] ServerProviders;

        public void Register(IZServiceProvider[] serviceProviders)
        {
            ServerProviders = serviceProviders;
            if (ServerProviders == null) return;
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Register();
            }
        }

        public void Initialize()
        {
            if (ServerProviders == null) return;
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Initialize();
            }
        }

        public void Uninitialize()
        {
            if (ServerProviders == null) return;
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Uninitialize();
            }
        }

        public void Unregister()
        {
            if (ServerProviders == null) return;
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Unregister();
            }
        }
    }
}
