using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZCSharpLib.Common;

namespace ZCSharpLib.ZTThread
{
    public class MainThread
    {
        public int ThreadID { get; private set; }
        private ThreadSync ThreadSync { get; set; }

        public MainThread()
        {
            ThreadID = Thread.CurrentThread.ManagedThreadId;
            ThreadSync = new ThreadSync();
            App.AttachTick(Loop);
        }

        ~MainThread()
        {
            App.DetachTick(Loop);
        }

        public void Loop(float deltaTime)
        {
            ThreadSync.Execute();
        }

        public void Send(SendOrPostCallback callback, object state)
        {
            ThreadSync.Send(callback, state);
        }

        public void Post(SendOrPostCallback callback, object state)
        {
            ThreadSync.Post(callback, state);
        }
    }
}
