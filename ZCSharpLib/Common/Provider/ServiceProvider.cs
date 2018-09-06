using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Common.Provider
{
    public class ServiceProvider
    {
        private List<IZServiceProvider> ServerProviders = new List<IZServiceProvider>();

        public void Register(IZServiceProvider[] oProviders)
        {
            ServerProviders.AddRange(oProviders);
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Register();
            }
        }

        public void Initialize()
        {
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Initialize();
            }
        }

        public void Uninitialize()
        {
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Uninitialize();
            }
        }

        public void Unregister()
        {
            foreach (IZServiceProvider serverProvider in ServerProviders)
            {
                serverProvider.Unregister();
            }
        }
    }
}
