using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Features.NetWork.Interface;
using ZCSharpLib.Features.NetWork.Protocol;
using ZCSharpLib.Common;
using ZCSharpLib.ZTGeneric;
using ZCSharpLib.ZTObject;

namespace ZCSharpLib.Features.NetWork.Tcp
{
    public abstract class AsyncSocket : IAsyncSocket
    {
        protected Socket ListenSocket { get; set; }

        /// <summary>
        /// 每个连接接收缓存大小
        /// </summary>
        protected int ReceiveBufferSize { get; set; }

        /// <summary>
        /// 最大支持连接个数
        /// </summary>
        protected int NumConnections { get; set; }

        /// <summary>
        /// 限制访问接收连接的线程数，用来控制最大并发数
        /// </summary>
        protected Semaphore MaxNumberAccepted { get; set; }

        /// <summary>
        /// 连接对象数据池
        /// </summary>
        public ZObjectPool<AsyncSocketUserToken> AsyncSocketUserTokenPool { get; protected set; }
        public ZObjectList<AsyncSocketUserToken> AsyncSocketUserTokenList { get; protected set; }

        protected ProtocolType ProtocolType { get; set; }

        public AsyncSocket(int numConnections)
        {
            NumConnections = numConnections;
            ReceiveBufferSize = 1024 * 10; // IOCP接收数据缓存大小，设置过小会造成事件响应增多，设置过大会造成内存占用偏多
            AsyncSocketUserTokenPool = new ZObjectPool<AsyncSocketUserToken>(numConnections);
            AsyncSocketUserTokenList = new ZObjectList<AsyncSocketUserToken>();
            MaxNumberAccepted = new Semaphore(numConnections, numConnections);
            AsyncSocketUserToken userToken;
            PacketProcess oPacketProcess = new PacketProcess();
            for (int i = 0; i < NumConnections; i++) //按照连接数建立读写对象
            {
                userToken = new AsyncSocketUserToken(ReceiveBufferSize);
                userToken.AsyncSocket = this;
                userToken.PacketProcess = oPacketProcess;
                userToken.ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                userToken.SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                AsyncSocketUserTokenPool.Push(userToken);
            }
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs oEventArgs)
        {
            AsyncSocketUserToken userToken = oEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveDateTime = DateTime.Now;
            try
            {
                lock (userToken)
                {
                    if (oEventArgs.LastOperation == SocketAsyncOperation.Receive)
                        ProcessReceive(oEventArgs);
                    else if (oEventArgs.LastOperation == SocketAsyncOperation.Send)
                        ProcessSend(oEventArgs);
                    else
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception e)
            {
                ZLogger.Error("IO_Completed {0} error, message: {1}", userToken.ConnectSocket, e.Message);
                ZLogger.Error(e.StackTrace);
            }
        }

        public void ProcessReceive(SocketAsyncEventArgs receiveEventArgs)
        {
            AsyncSocketUserToken userToken = receiveEventArgs.UserToken as AsyncSocketUserToken;
            if (userToken.ConnectSocket == null)
                return;

            lock (userToken)
            {
                userToken.ActiveDateTime = DateTime.Now;
                if (userToken.ReceiveEventArgs.BytesTransferred > 0 && userToken.ReceiveEventArgs.SocketError == SocketError.Success)
                {
                    int offset = userToken.ReceiveEventArgs.Offset;
                    int count = userToken.ReceiveEventArgs.BytesTransferred;

                    if (count > 0) //处理接收数据
                    {
                        if (!userToken.Receive(userToken.ReceiveEventArgs.Buffer, offset, count)) //如果处理数据返回失败，则断开连接
                        {
                            CloseSocket(userToken);
                        }
                        else //否则投递下次接收数据请求
                        {
                            // willRaiseEvent 
                            // true I/O操作处于挂起状态(会引发SocketAsyncEventArgs.Completed 上的事件e), 
                            // false I/O操作同步完成(不会引发会引发SocketAsyncEventArgs.Completed 上的事件e，可能是直接返回操作结果)
                            bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                            if (!willRaiseEvent)
                                ProcessReceive(userToken.ReceiveEventArgs);
                        }
                    }
                    else
                    {
                        bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive(userToken.ReceiveEventArgs);
                    }

                }
                else
                {
                    CloseSocket(userToken);
                }
            }
        }

        private bool ProcessSend(SocketAsyncEventArgs sendEventArgs)
        {
            AsyncSocketUserToken userToken = sendEventArgs.UserToken as AsyncSocketUserToken;
            userToken.ActiveDateTime = DateTime.Now;
            if (sendEventArgs.SocketError == SocketError.Success)
                return userToken.SendAsyncCompleted(); //调用子类回调函数
            else
            {
                CloseSocket(userToken);
                return false;
            }
        }

        public bool SendAsync(Socket connectSocket, SocketAsyncEventArgs sendEventArgs, byte[] buffer, int offset, int count)
        {
            if (connectSocket == null) { return false; }
  
            sendEventArgs.SetBuffer(buffer, offset, count);

            bool willRaiseEvent = connectSocket.SendAsync(sendEventArgs);

            if (!willRaiseEvent)
            {
                return ProcessSend(sendEventArgs);
            }
            else
            {
                return true;
            }
        }

        public void CloseSocket(AsyncSocketUserToken userToken)
        {
            if (userToken.ConnectSocket == null)
                return;
            string socketInfo = string.Format("Local Address: {0} Remote Address: {1}", userToken.ConnectSocket.LocalEndPoint,
                userToken.SendEventArgs.RemoteEndPoint);
            ZLogger.Info("Connection disconnected. {0}", socketInfo);
            try
            {
                userToken.ConnectSocket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception E)
            {
                ZLogger.Info("CloseSocket Disconnect {0} error, message: {1}", socketInfo, E.Message);
            }
            userToken.ConnectSocket.Close();
            userToken.ConnectSocket = null; //释放引用，并清理缓存，包括释放协议对象等资源
            userToken.TokenClose();

            MaxNumberAccepted.Release();
            AsyncSocketUserTokenPool.Push(userToken);
            AsyncSocketUserTokenList.Remove(userToken);
        }

        public void CloseAll()
        {
            AsyncSocketUserToken[] oUserTokens = null;
            AsyncSocketUserTokenList.CopyList(ref oUserTokens);
            for (int i = 0; i < oUserTokens.Length; i++)
            {
                AsyncSocketUserToken oUserToken = oUserTokens[i];
                CloseSocket(oUserToken);
            }
        }
    }
}
