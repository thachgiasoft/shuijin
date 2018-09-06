using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZCSharpLib.ZTThread
{
    public class ThreadSync
    {
        private Queue<WorkRequest> AsyncWorkQueue { get; set; }
        private int MainThreadID { get; set; }

        public ThreadSync()
        {
            AsyncWorkQueue = new Queue<WorkRequest>();
            MainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        public void Send(SendOrPostCallback callback, object state)
        {
            if (MainThreadID == Thread.CurrentThread.ManagedThreadId)
            {
                callback(state);
            }
            else
            {
                using (ManualResetEvent waitHandle = new ManualResetEvent(false))
                {
                    lock (AsyncWorkQueue)
                    {
                        WorkRequest workRequest = new WorkRequest(callback, state, waitHandle);
                        AsyncWorkQueue.Enqueue(workRequest);
                    }
                    waitHandle.WaitOne();
                }
            }
        }

        public void Post(SendOrPostCallback callback, object state)
        {
            lock (AsyncWorkQueue)
            {
                WorkRequest workRequest = new WorkRequest(callback, state, null);
                AsyncWorkQueue.Enqueue(workRequest);
            }
        }

        public void Execute()
        {
            lock (AsyncWorkQueue)
            {
                var workCount = AsyncWorkQueue.Count;
                for (int i = 0; i < workCount; i++)
                {
                    var work = AsyncWorkQueue.Dequeue();
                    work.Invoke();
                }
            }
        }

        private struct WorkRequest
        {
            private readonly object mState;
            private readonly SendOrPostCallback mCallback;
            private readonly ManualResetEvent mWaitHandle;

            public WorkRequest(SendOrPostCallback callback, object state, ManualResetEvent waitHandle = null)
            {
                mCallback = callback;
                mState = state;
                mWaitHandle = waitHandle;
            }

            public void Invoke()
            {
                mCallback(mState);
                if (mWaitHandle != null) mWaitHandle.Set();
            }
        }
    }
}
