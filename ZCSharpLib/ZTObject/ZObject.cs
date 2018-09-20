using System;
using ZCSharpLib.Common;
using ZCSharpLib.ZTEvent;

namespace ZCSharpLib.ZTObject
{
    public abstract class ZObject : Any, IDisposable
    {
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

        public virtual void Dispose()
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
