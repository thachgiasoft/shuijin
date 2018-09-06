using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.Features.NetWork.Interface;

namespace ZCSharpLib.Features.NetWork.Packet
{
    public abstract class BasePacket<T> : IPacket, IPacketCreate where T : IPacket, new()
    {
        protected int mPacketID;

        public int PacketID
        {
            get
            {
                if (mPacketID == 0)
                {
                    object[] objs = GetType().GetCustomAttributes(typeof(ProtocolAttribute), false);
                    if (objs.Length > 0)
                    {
                        ProtocolAttribute oAttr = objs[0] as ProtocolAttribute;
                        mPacketID = oAttr.ID;
                    }
                }
                return mPacketID;
            }
        }

        public virtual void Deserialization(DynamicBuffer oBuffer) { }

        public virtual void Serialization(DynamicBuffer oBuffer) { }

        public virtual object New()
        {
            return new T();
        }
    }
}
