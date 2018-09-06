using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.NetWork.Interface
{
    public interface IProtocolProcess
    {
        void ToProcess(object obj, object owner);
    }
}
