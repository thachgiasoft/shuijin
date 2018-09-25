using System;
using System.Collections.Generic;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Features.NetWork.Interface;
using ZCSharpLib.Features.NetWork.Packet;
using ZCSharpLib.Common;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Features.NetWork.Protocol
{
    public class PacketProcess
    {
        private Dictionary<int, IPacket> PacketTable;
        private Dictionary<int, IProtocolProcess> ProtocolTable;

        public PacketProcess()
        {
            PacketTable = new Dictionary<int, IPacket>();
            ProtocolTable = new Dictionary<int, IProtocolProcess>();
            AssemblyPacket();
        }

        private void AssemblyPacket()
        {
            Type[] types = ZCommUtil.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                object[] objs = type.GetCustomAttributes(typeof(ProtocolAttribute), false);
                if (objs.Length > 0)
                {
                    // 如果包含包属性,则创建对象
                    object obj = Activator.CreateInstance(type);
                    ProtocolAttribute oAttr = objs[0] as ProtocolAttribute;
                    if (oAttr.Type == ProcessType.Receive)
                    {
                        IPacket oPacket = obj as IPacket;
                        PacketTable.Add(oAttr.ID, oPacket);
                    }
                    else if (oAttr.Type == ProcessType.Process)
                    {
                        IProtocolProcess oProtocol =  obj as IProtocolProcess;
                        ProtocolTable.Add(oAttr.ID, oProtocol);
                    }
                }
            }
        }

        public bool Process(AsyncSocketUserToken oUserToken, byte[] bytes, int offset, int count)
        {
            if (count < sizeof(int)) return false; // 判断包长度是否小于一个命令的长度
            int oPacketID = BitConverter.ToInt32(bytes, offset); // 获取命令ID
            IProtocolProcess oProtocol = null;
            if (ProtocolTable.TryGetValue(oPacketID, out oProtocol))
            {
                IPacket oPacket = null;
                if (PacketTable.TryGetValue(oPacketID, out oPacket))
                {
                    IPacketCreate oIPacket = oPacket as IPacketCreate;
                    IPacket oNewPacket = oIPacket.New() as IPacket;
                    try
                    {
                        DynamicBuffer buffer = new DynamicBuffer(bytes, offset + sizeof(int), bytes.Length - offset - sizeof(int));
                        oNewPacket.Deserialization(buffer);
                        oProtocol.ToProcess(oUserToken, oNewPacket);
                    }
                    catch(Exception e)
                    {
                        App.Logger.Error(e.Message + "\n" + e.StackTrace);
                        return false;
                    }
                    return true;
                }
                else
                {
                   return false;
                }
            }
            return false;
        }
    }


}
