using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Common.Provider
{
    public interface IZServiceProvider
    {
        void Register();

        void Initialize();

        void Uninitialize();

        void Unregister();
    }
}
