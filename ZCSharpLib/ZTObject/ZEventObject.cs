using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCSharpLib.ZTEvent;

namespace ZCSharpLib.ZTObject
{
    public abstract class ZEventObject : ZObject
    {
        protected ZEvent Event { get; private set; }

        protected string EventCall
        {
            get
            {
                return GetHashCode().ToString();
            }
        }

        public ZEventObject()
        {
            Event = new ZEvent();
        }

        #region 事件
        public virtual void AddListener(IEventListener listener)
        {
            Event.AddListener(EventCall, listener);
        }

        public virtual void RemoveListener(IEventListener listener)
        {
            Event.RemoveListener(EventCall, listener);
        }

        public virtual void Notify(IEventArgs args, float delayTime = 0)
        {
            Event.Notify(EventCall, args, delayTime);
        }
        #endregion

        protected override void DoManagedObjectDispose()
        {
            base.DoManagedObjectDispose();
            Event.RemoveAllListener();
        }
    }
}
