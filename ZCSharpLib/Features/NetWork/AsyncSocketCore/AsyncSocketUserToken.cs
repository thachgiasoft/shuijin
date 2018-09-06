using System;
using System.Net.Sockets;
using ZCSharpLib.Features.NetWork.Interface;
using ZCSharpLib.Features.NetWork.Protocol;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.NetWork.AsyncSocketCore
{
    public class AsyncSocketUserToken
    {
        protected Socket m_Socket;
        public Socket ConnectSocket
        {
            get
            {
                return m_Socket;
            }
            set
            {
                if (m_Socket == null) // 当有新的连接时清理缓存
                {
                    m_SendAsync = false;
                    ReceiveBuffer.Clear(ReceiveBuffer.DataCount);
                    SendBufferMgr.ClearPacket();
                }

                m_Socket = value;
                SendEventArgs.AcceptSocket = m_Socket;
                ReceiveEventArgs.AcceptSocket = m_Socket;
            }
        }

        private bool m_SendAsync; //标识是否有发送异步事件
        public ITokenOwner Owner { get; set; }
        public SocketAsyncEventArgs SendEventArgs { get; set; }
        public SocketAsyncEventArgs ReceiveEventArgs { get; set; }
        public IAsyncSocket AsyncSocket { get; set; }
        public PacketProcess PacketProcess { get; set; }

        protected DateTime m_ConnectDateTime;
        public DateTime ConnectDateTime { get { return m_ConnectDateTime; } set { m_ConnectDateTime = value; } }
        protected DateTime m_ActiveDateTime;
        public DateTime ActiveDateTime { get { return m_ActiveDateTime; } set { m_ActiveDateTime = value; } }
        public DynamicBuffer ReceiveBuffer { get; private set; }
        private AsyncSendBufferManager SendBufferMgr { get; set; }

        public AsyncSocketUserToken(int asyncReceiveBufferSize)
        {
            SendEventArgs = new SocketAsyncEventArgs();
            SendEventArgs.UserToken = this;
            //byte[] oAsyncSendBuffer = new byte[asyncReceiveBufferSize];
            //SendEventArgs.SetBuffer(oAsyncSendBuffer, 0, oAsyncSendBuffer.Length);

            ReceiveEventArgs = new SocketAsyncEventArgs();
            byte[] oAsyncReceiveBuffer = new byte[asyncReceiveBufferSize];
            ReceiveEventArgs.SetBuffer(oAsyncReceiveBuffer, 0, oAsyncReceiveBuffer.Length);
            ReceiveEventArgs.UserToken = this;

            ReceiveBuffer = new DynamicBuffer();
            SendBufferMgr = new AsyncSendBufferManager(asyncReceiveBufferSize);
        }

        public void Setup(ITokenOwner owner)
        {
            Owner = owner;
        }

        public void TokenClose()
        {
            if (Owner != null)
            {
                Owner.TokenClose();
            }
        }

        internal bool Receive(byte[] buffer, int offset, int count)
        {
            ReceiveBuffer.WriteBytes(buffer, offset, count);  // 写入数据内容
            bool result = true;
            while (ReceiveBuffer.DataCount > sizeof(int))
            {
                //按照长度分包
                int packetLength = BitConverter.ToInt32(ReceiveBuffer.Buffer, 0); //获取包长度 // 老版代码

                if ((packetLength > 10 * 1024 * 1024) | (buffer.Length > 10 * 1024 * 1024)) //最大Buffer异常保护
                {
                    ZLogger.Error("内存超出, 数据不进行操作, 关闭远程连接");
                    return false;
                }

                if ((ReceiveBuffer.DataCount - sizeof(int)) >= packetLength) //收到的数据达到包长度
                {
                    result = PacketProcess.Process(this, ReceiveBuffer.Buffer, sizeof(int), packetLength);
                    if (result)
                    {
                        ReceiveBuffer.Clear(packetLength + sizeof(int));
                    }
                    else return result;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// 异步发送包
        /// </summary>
        /// <param name="oPacket">数据包</param>
        /// <returns>是否发送成功</returns>
        public bool SendAsync(IPacket oPacket)
        {
            DynamicBuffer sendBuffer = new DynamicBuffer();
            oPacket.Serialization(sendBuffer);
            int totalLength = sizeof(int) + sendBuffer.DataCount; //获取总大小
            SendBufferMgr.StartPacket();
            SendBufferMgr.DynamicBufferManager.WriteInt32(totalLength); //写入总大小
            SendBufferMgr.DynamicBufferManager.WriteInt32(oPacket.PacketID); //写入命令
            SendBufferMgr.DynamicBufferManager.WriteBytes(sendBuffer.Buffer); //写入命令内容
            SendBufferMgr.EndPacket();

            bool result = true;
            if (!m_SendAsync)
            {
                int packetOffset = 0;
                int packetCount = 0;
                if (SendBufferMgr.GetFirstPacket(ref packetOffset, ref packetCount))
                {
                    m_SendAsync = true;
                    result = AsyncSocket.SendAsync(ConnectSocket, SendEventArgs, SendBufferMgr.DynamicBufferManager.Buffer, packetOffset, packetCount);
                }
            }
            return result;
        }

        public bool SendAsyncCompleted()
        {
            ActiveDateTime = DateTime.UtcNow;
            m_SendAsync = false;
            SendBufferMgr.ClearFirstPacket(); //清除已发送的包
            int offset = 0;
            int count = 0;
            if (SendBufferMgr.GetFirstPacket(ref offset, ref count))
            {
                m_SendAsync = true;
                return AsyncSocket.SendAsync(ConnectSocket, SendEventArgs,
                    SendBufferMgr.DynamicBufferManager.Buffer, offset, count);
            }
            else
                return SendCallback();
        }

        //发送回调函数，用于连续下发数据
        public virtual bool SendCallback()
        {
            return true;
        }
    }
}
