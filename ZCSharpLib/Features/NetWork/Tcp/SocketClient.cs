using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.NetWork.Tcp
{
    public class SocketClient : AsyncSocket
    {
        public AsyncSocketUserToken UserToken { get; private set; }

        public SocketClient() : base(1)
        {
            UserToken = AsyncSocketUserTokenPool.Pop();
            AsyncSocketUserTokenList.Add(UserToken);
        }

        public void Connect(string host, int port)
        {
            IPEndPoint oIPEndPoint = new IPEndPoint(IPAddress.Parse(host), port);
            ListenSocket = new Socket(oIPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            SocketAsyncEventArgs connectEventArgs = new SocketAsyncEventArgs() { RemoteEndPoint = oIPEndPoint, AcceptSocket = ListenSocket };
            connectEventArgs.Completed += ConnectEventArg_Completed;

            MaxNumberAccepted.WaitOne(); //获取信号量
            StartConnect(connectEventArgs);
        }

        private void StartConnect(SocketAsyncEventArgs connectEventArgs)
        {
            bool willRaiseEvent = ListenSocket.ConnectAsync(connectEventArgs);
            if (!willRaiseEvent)
            {
                ProcessConnect(connectEventArgs);
            }
        }

        private void ConnectEventArg_Completed(object sender, SocketAsyncEventArgs eventArgs)
        {
            try
            {
                ProcessConnect(eventArgs);
            }
            catch (Exception e)
            {
                ZLogger.Error("Accept client {0} error, message: {1}", eventArgs.AcceptSocket, e.Message);
                ZLogger.Error(e.StackTrace);
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs.SocketError == SocketError.SocketError) return;

            UserToken.ConnectDateTime = DateTime.Now;

            UserToken.ConnectSocket = acceptEventArgs.AcceptSocket;

            try
            {
                bool willRaiseEvent = UserToken.ConnectSocket.ReceiveAsync(UserToken.ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    lock (UserToken)
                    {
                        // TODO 处理接收到的消息
                        ProcessReceive(UserToken.ReceiveEventArgs);
                    }
                }
            }
            catch (Exception E)
            {
                ZLogger.Error("Accept client {0} error, message: {1}", UserToken.ConnectSocket, E.Message);
                ZLogger.Error(E.StackTrace);
            }
        }
    }
}
