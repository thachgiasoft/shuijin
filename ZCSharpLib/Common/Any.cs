using System;

namespace ZCSharpLib.Common
{
    public class Any : System.Object, System.IDisposable
    {
        public T As<T>() where T : Any
        {
            return (T)this;
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
