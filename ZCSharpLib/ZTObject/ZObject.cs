using System;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;

namespace ZCSharpLib.ZTObject
{
    public abstract class ZObject : Any, IDisposable
    {
        public abstract object Clone();

        public abstract void CopyTo(object destObj);
    }
}
