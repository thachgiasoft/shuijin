using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.Http;
using ZCSharpLib.Interface;
using ZCSharpLib.Log;
using ZCSharpLib.Tick;

namespace Upgrade
{
    public class Bootstarp : IBootstrap
    {
        public Type[] GetProviders()
        {
            return new Type[]
            {
                typeof(Tick),
                typeof(ZLogger),
                typeof(HttpLoader),
            };
        }
    }
}
