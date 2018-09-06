using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.NetWork.Interface
{
    public interface IPacket
    {
        int PacketID { get; }

        void Serialization(DynamicBuffer oBuffer);

        void Deserialization(DynamicBuffer oBuffer);
    }
}
