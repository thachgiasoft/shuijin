﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZCSharpLib.Features.NetWork.AsyncSocketCore;
using ZCSharpLib.Common;

namespace ZCSharpLib.Features.NetWork.Tcp
{
    /// <summary>
    /// 守护线程, 用于超时连接判断
    /// </summary>
    class DaemonThread : Object
    {
        private Thread m_thread;
        private SocketServer m_asyncSocketServer;

        public DaemonThread(SocketServer asyncSocketServer)
        {
            m_asyncSocketServer = asyncSocketServer;
            m_thread = new Thread(DaemonThreadStart);
            m_thread.Start();
        }

        public void DaemonThreadStart()
        {
            while (m_thread.IsAlive)
            {
                AsyncSocketUserToken[] userTokenArray = null;
                m_asyncSocketServer.AsyncSocketUserTokenList.CopyList(ref userTokenArray);
                for (int i = 0; i < userTokenArray.Length; i++)
                {
                    if (!m_thread.IsAlive)
                        break;
                    try
                    {
                        if ((DateTime.Now - userTokenArray[i].ActiveDateTime).Milliseconds > m_asyncSocketServer.SocketTimeOutMS) //超时Socket断开
                        {
                            lock (userTokenArray[i])
                            {
                                m_asyncSocketServer.CloseSocket(userTokenArray[i]);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        App.Logger.Error("Daemon thread check timeout socket error, message: {0}", e.Message);
                        App.Logger.Error(e.StackTrace);
                    }
                }

                for (int i = 0; i < 60 * 1000 / 10; i++) //每分钟检测一次
                {
                    if (!m_thread.IsAlive)
                        break;
                    Thread.Sleep(10);
                }
            }
        }

        public void Close()
        {
            m_thread.Abort();
            m_thread.Join();
        }
    }
}
