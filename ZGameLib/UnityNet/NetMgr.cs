using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.Features.NetWork.Interface;
using ZCSharpLib.Features.NetWork.Tcp;

namespace ZGameLib.UnityNet
{
    public class NetMgr
    {
        private SocketClient SocketClient { get; set; }

        public NetMgr()
        {
            SocketClient = new SocketClient();
        }

        public void Connect(string host, int port)
        {
            SocketClient.Connect(host, port);
        }

        public void Send(IPacket packet)
        {

        }
    }
}
