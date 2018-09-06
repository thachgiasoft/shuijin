using System;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;

namespace ZCSharpLib.ZTObject
{
    public abstract class ZObject : Any, IDisposable
    {
        protected ZEvent Event { get; private set; }

        protected string EventCall
        {
            get
            {
                return GetHashCode().ToString();
            }
        }

        public ZObject()
        {
            Event = new ZEvent();
        }

        #region 事件
        public void AddListener(IEventListener listener)
        {
            Event.AddListener(EventCall, listener);
        }

        public void RemoveListener(IEventListener listener)
        {
            Event.RemoveListener(EventCall, listener);
        }

        public void Notify(IEventArgs args)
        {
            Event.Notify(EventCall, args);
        }
        #endregion

        public abstract object Clone();

        public abstract void CopyTo(object destObj);

        /// <summary>
        /// 清理磁盘数据
        /// </summary>
        public virtual void Clear() { }

        ~ZObject()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            // 那么这个方法是被客户直接调用的,那么托管的,和非托管的资源都可以释放   
            if (disposing)
            {
                // 释放 托管资源   
                DoManagedObjectDispose();
            }
            //释放非托管资源   
            DoUnManagedObjectDispose();
            if (disposing) GC.SuppressFinalize(this);
        }

        protected virtual void DoManagedObjectDispose() { }

        protected virtual void DoUnManagedObjectDispose() { }
    }
}
