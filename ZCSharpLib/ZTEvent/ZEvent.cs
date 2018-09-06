using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZCSharpLib.Common;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.ZTEvent
{
    public class ZEvent
    {
        protected Dictionary<string, List<IEventListener>> ListenerTable { get; set; }

        public ZEvent()
        {
            ListenerTable = new Dictionary<string, List<IEventListener>>();
        }

        private List<IEventListener> GetListeners(string callEvent)
        {
            List<IEventListener> listeners = null;
            if (ListenerTable.TryGetValue(callEvent, out listeners))
            {
                listeners = new List<IEventListener>();
                ListenerTable.Add(callEvent, listeners);
            }
            return listeners;
        }

        public void AddListener(string callEvent, IEventListener listener)
        {
            List<IEventListener> listeners = GetListeners(callEvent);
            listeners.Add(listener);
        }

        public void RemoveListener(string callEvent, IEventListener listener)
        {
            List<IEventListener> listeners = GetListeners(callEvent);
            if (listeners.Count > 0) listeners.Remove(listener);
            if (listeners.Count == 0)
            {
                listeners = null;
                ListenerTable.Remove(callEvent);
            }
        }

        public void RemoveAllListener(string callEvent)
        {
            List<IEventListener> listeners = GetListeners(callEvent);
            listeners.Clear(); listeners = null;
            ListenerTable.Remove(callEvent);
        }

        public void ClearAll()
        {
            foreach (var listeners in ListenerTable.Values)
            {
                listeners.Clear();
            }
            ListenerTable.Clear();
        }

        public void Notify(string callEvent, IEventArgs args)
        {
            List<IEventListener> listeners = GetListeners(callEvent);
            for (int i = 0; i < listeners.Count; i++)
            {
                IEventListener listener = listeners[i];
                try
                {
                    listener.OnEventCall(args);
                }
                catch (Exception e)
                {
                    ZLogger.Error(e);
                }
            }
        }

        public void DelayNotify(string callEvent, IEventArgs args, float delayTime)
        {
            DelayNotifyWarper oEventTick = new DelayNotifyWarper(callEvent, args, delayTime);
            oEventTick.Open();
        }

        private void TickEventCall(DelayNotifyWarper eventTick)
        {
            eventTick.Close();
            Notify(eventTick.CallEvent, eventTick.EventArgs);
        }

        protected class DelayNotifyWarper : ITick
        {
            public string CallEvent { get; private set; }
            public IEventArgs EventArgs { get; private set; }
            private float UseTime { get; set; }
            private float DelayTime { get; set; }
            private Action<DelayNotifyWarper> TickCall { get; set; }

            public DelayNotifyWarper(string callEvent, IEventArgs eventArgs, float delayTime)
            {
                CallEvent = callEvent;
                EventArgs = eventArgs;
                DelayTime = delayTime;
            }

            public void SetEventTickCall(Action<DelayNotifyWarper> tickCall)
            {
                TickCall = tickCall;
            }

            public void Open()
            {
                Tick.Attach(this);
            }

            public void Close()
            {
                Tick.Detach(this);
            }

            public void Loop(float deltaTime)
            {
                UseTime = MathUtil.Clamp(UseTime + deltaTime, 0, DelayTime);
                if (UseTime == DelayTime) TickCall?.Invoke(this);
            }
        }
    }

    public class DelayCaller : IEventListener
    {
        public DelayCaller Listener { get; private set; }

        private Action<IEventArgs> EventCall { get; set; }

        public DelayCaller(Action<IEventArgs> onEventCall)
        {
            EventCall = onEventCall;
        }

        public void OnEventCall(IEventArgs args)
        {
            EventCall?.Invoke(args);
        }
    }
}

