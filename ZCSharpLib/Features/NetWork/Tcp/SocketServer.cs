using System;
using System.Net;
using System.Net.Sockets;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.NetWork.Tcp
{
    public class SocketServer : AsyncSocket
    {
        /// <summary>
        /// 超时设置
        /// </summary>
        public int SocketTimeOutMS { get; set; }

        /// <summary>
        /// 守护线程
        /// </summary>
        private DaemonThread DaemonThread { get; set; }

        public SocketServer(int numConnections): base(numConnections)
        {
        }

        public void Start(string host, int port)
        {
            IPEndPoint listenPoint = new IPEndPoint(IPAddress.Parse(host), port);
            Start(listenPoint);
        }

        public void Start(IPEndPoint localEndPoint)
        {
            ListenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ListenSocket.Bind(localEndPoint);
            ListenSocket.Listen(NumConnections);
            App.Logger.Info("Start listen socket {0} success", localEndPoint.ToString());
            StartAccept(null);
            SocketTimeOutMS = 60 * 1000;
            DaemonThread = new DaemonThread(this);
        }

        public void StartAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            if (acceptEventArgs == null)
            {
                acceptEventArgs = new SocketAsyncEventArgs();
                acceptEventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(AcceptEventArg_Completed);
            }
            else
            {
                acceptEventArgs.AcceptSocket = null; //释放上次绑定的Socket，等待下一个Socket连接
            }

            MaxNumberAccepted.WaitOne(); //获取信号量
            bool willRaiseEvent = ListenSocket.AcceptAsync(acceptEventArgs);
            if (!willRaiseEvent)
            {
                ProcessAccept(acceptEventArgs);
            }
        }

        void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs acceptEventArgs)
        {
            try
            {
                ProcessAccept(acceptEventArgs);
            }
            catch (Exception e)
            {
                App.Logger.Error("Accept client {0} error, message: {1}", acceptEventArgs.AcceptSocket, e.Message);
                App.Logger.Error(e.StackTrace);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs acceptEventArgs)
        {
            App.Logger.Info("Client connection accepted. Local Address: {0}, Remote Address: {1}",
                acceptEventArgs.AcceptSocket.LocalEndPoint, acceptEventArgs.AcceptSocket.RemoteEndPoint);

            AsyncSocketUserToken userToken = AsyncSocketUserTokenPool.Pop();
            AsyncSocketUserTokenList.Add(userToken); //添加到正在连接列表
            userToken.ConnectSocket = acceptEventArgs.AcceptSocket;
            userToken.ConnectDateTime = DateTime.Now;

            try
            {
                bool willRaiseEvent = userToken.ConnectSocket.ReceiveAsync(userToken.ReceiveEventArgs); //投递接收请求
                if (!willRaiseEvent)
                {
                    lock (userToken)
                    {
                        ProcessReceive(userToken.ReceiveEventArgs);
                    }
                }
            }
            catch (Exception E)
            {
                App.Logger.Error("Accept client {0} error, message: {1}", userToken.ConnectSocket, E.Message);
                App.Logger.Error(E.StackTrace);
            }

            StartAccept(acceptEventArgs); //把当前异步事件释放，等待下次连接
        }
    }
}
