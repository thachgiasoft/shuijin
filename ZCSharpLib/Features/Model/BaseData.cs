using System;
using ZCSharpLib.ZTObject;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.Model
{
    public abstract class BaseData : ZEventObject
    {
        public string Guid { get; private set; }

        public BaseData(string oGuid)
        {
            Guid = oGuid;
        }
    }
}


