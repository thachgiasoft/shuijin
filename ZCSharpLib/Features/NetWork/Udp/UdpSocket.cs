﻿using System;
using System.Net;
using System.Net.Sockets;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Features.NetWork.Interface;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.NetWork.Udp
{
    public class UdpSocket : IAsyncSocket
    {
        protected Socket ListenSocket { get; set; }

        protected int ReceiveBufferSize { get; set; }

        public AsyncSocketUserToken UserToken { get; set; }

        private bool IsServer { get; set; }

        private DaemonThread DaemonThread { get; set; }

        public UdpSocket()
        {
            DaemonThread = new DaemonThread();
            ReceiveBufferSize = 1024;
            UserToken = new AsyncSocketUserToken(ReceiveBufferSize);
            UserToken.AsyncSocket = this;
            UserToken.PacketProcess = new Protocol.PacketProcess();
            UserToken.ReceiveEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            UserToken.SendEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
        }

        public void Start(int port)
        {
            IPEndPoint listenPort = new IPEndPoint(IPAddress.Any, port);
            ListenSocket = new Socket(listenPort.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            ListenSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
            //ListenSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true); // ReuseAddress选项设置为True将允许将套接字绑定到已在使用中的地址。 
            ListenSocket.EnableBroadcast = true; // 允许Socket进行广播操作
            ListenSocket.Bind(listenPort);
            UserToken.ConnectSocket = ListenSocket;
            UserToken.ReceiveEventArgs.RemoteEndPoint = listenPort;
            UserToken.SendEventArgs.RemoteEndPoint = new IPEndPoint(IPAddress.Broadcast, port);
            UserToken.ConnectSocket.ReceiveFromAsync(UserToken.ReceiveEventArgs);
            App.Logger.Info("Start listen socket {0} success", listenPort.ToString());
        }

        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            AsyncSocketUserToken userToken = e.UserToken as AsyncSocketUserToken;
            userToken.ActiveDateTime = DateTime.Now;
            try
            {
                lock (userToken)
                {
                    if (e.LastOperation == SocketAsyncOperation.ReceiveFrom)
                        ProcessReceive(e);
                    else if (e.LastOperation == SocketAsyncOperation.SendTo)
                        ProcessSend(e);
                    else
                        throw new ArgumentException("The last operation completed on the socket was not a receive or send");
                }
            }
            catch (Exception E)
            {
                App.Logger.Error("IO_Completed {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                App.Logger.Error(E.StackTrace);
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
                            //CloseSocket(userToken);
                        }
                        else //否则投递下次接收数据请求
                        {
                            bool willRaiseEvent = userToken.ConnectSocket.ReceiveFromAsync(userToken.ReceiveEventArgs); //投递接收请求
                            if (!willRaiseEvent)
                                ProcessReceive(userToken.ReceiveEventArgs);
                        }
                    }
                    else
                    {
                        bool willRaiseEvent = userToken.ConnectSocket.ReceiveFromAsync(userToken.ReceiveEventArgs); //投递接收请求
                        if (!willRaiseEvent)
                            ProcessReceive(userToken.ReceiveEventArgs);
                    }

                }
                else
                {
                    //CloseSocket(userToken);
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
                //CloseSocket(userToken);
                return false;
            }
        }

        public bool SendAsync(Socket connectSocket, SocketAsyncEventArgs sendEventArgs, byte[] buffer, int offset, int count)
        {
            if (connectSocket == null) { return false; }

            sendEventArgs.SetBuffer(buffer, offset, count);

            bool willRaiseEvent = connectSocket.SendToAsync(sendEventArgs);

            if (!willRaiseEvent)
            {
                return ProcessSend(sendEventArgs);
            }
            else
            {
                return true;
            }
        }
    }
}
