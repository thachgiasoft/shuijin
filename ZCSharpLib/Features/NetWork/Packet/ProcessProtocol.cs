using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.Features.NetWork.Interface;

namespace ZCSharpLib.Features.NetWork.Packet
{
    public abstract class ProcessProtocol<T> : IProtocolProcess where T : class, IPacket
    {
        public void ToProcess(object sender, object obj)
        {
            Process(sender, obj as T);
        }

        public abstract void Process(object sender, T packet);
    }
}
