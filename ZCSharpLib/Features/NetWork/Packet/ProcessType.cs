using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.NetWork.Packet
{
    public enum ProcessType
    {
        None,       
        Send,       // 发送
        Receive,    // 接收
        Process,    // 处理
    }
}
