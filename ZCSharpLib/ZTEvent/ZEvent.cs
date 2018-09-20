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
        struct DelayEvent
        {
            private string CallEvent { get; set; }
            private IEventArgs EventArgs { get; set; }
            private float UseTime { get; set; }
            private float DelayTime { get; set; }

            public Action<string, IEventArgs, float> OnNotify { get; set; }

            public DelayEvent(string callEvent, IEventArgs eventArgs, 
                float delayTime, Action<string, IEventArgs, float> onNotify)
            {
                CallEvent = callEvent;
                EventArgs = eventArgs;
                DelayTime = delayTime;
                OnNotify = onNotify;
                UseTime = 0;
            }

            public void ToNotify()
            {
                App.AttachTick(Loop);
            }

            public void Loop(float deltaTime)
            {
                UseTime = MathUtil.Clamp(UseTime + deltaTime, 0, DelayTime);
                if (UseTime == DelayTime)
                {
                    App.DetachTick(Loop);
                    OnNotify?.Invoke(CallEvent, EventArgs, 0);
                }
            }
        }

        protected Dictionary<string, List<IEventListener>> EventDict { get; set; }

        public ZEvent()
        {
            EventDict = new Dictionary<string, List<IEventListener>>();
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="eventName">事件名称</param>
        /// <param name="listener">事件监听者</param>
        public void AddListener(string eventName, IEventListener listener)
        {
            List<IEventListener> listeners = null;
            if (!EventDict.TryGetValue(eventName, out listeners))
            {
                listeners = new List<IEventListener>();
                EventDict.Add(eventName, listeners);
            }
            if (!listeners.Contains(listener)) listeners.Add(listener);
        }

        /// <summary>
        /// 移除指定的事件
        /// </summary>
        /// <param name="eventName">指定的事件名</param>
        /// <param name="listener">指定的事件</param>
        public void RemoveListener(string eventName, IEventListener listener)
        {
            List<IEventListener> listeners = null;
            if (EventDict.TryGetValue(eventName, out listeners))
            {
                if (listeners.Count > 0)
                {
                    listeners.Remove(listener);
                }
                if (listeners.Count == 0)
                {
                    listeners = null;
                    EventDict.Remove(eventName);
                }
            }
        }

        /// <summary>
        /// 移除列表中所有给定名称的事件
        /// </summary>
        /// <param name="eventName">指定事件的名称</param>
        public void RemoveAllListener(string eventName)
        {
            List<IEventListener> listeners = null;
            if (EventDict.TryGetValue(eventName, out listeners))
            {
                listeners.Clear();
                listeners = null;
                EventDict.Remove(eventName);
            }
        }

        /// <summary>
        /// 移除所有事件
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var listeners in EventDict.Values)
            {
                listeners.Clear();
            }
            EventDict.Clear();
        }

        public void Notify(string callEvent, IEventArgs args, float delayTime = 0)
        {
            List<IEventListener> listeners = null;
            if (EventDict.TryGetValue(callEvent, out listeners))
            {
                if (delayTime < 0)
                {
                    for (int i = 0; i < listeners.Count; i++)
                    {
                        IEventListener listener = listeners[i];
                        try { listener.OnNotify(args); }
                        catch (Exception e) { ZLogger.Error(e); }
                    }
                }
                else
                {
                    DelayEvent oDelayEvent = new DelayEvent(callEvent, args, delayTime, Notify);
                    oDelayEvent.ToNotify();
                }
            }
        }
    }

    public struct ZEventHelper : IEventListener
    {
        private Action<IEventArgs> Callback { get; set; }

        public ZEventHelper(Action<IEventArgs> callback)
        {
            Callback = callback;
        }

        public void OnNotify(IEventArgs args)
        {
            Callback?.Invoke(args);
        }
    }
}

