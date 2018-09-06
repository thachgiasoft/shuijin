using System;
using System.Collections.Generic;

namespace ZCSharpLib.ZTEvent
{
    public class EventDispatcher
    {
        private static Dictionary<string, Delegate> EventTable = new Dictionary<string, Delegate>();

        private static Delegate OnAddListenerAdding(string oEventType, Delegate linstener)
        {
            Delegate @delegate = null;
            if (!EventTable.TryGetValue(oEventType, out @delegate))
            {
                EventTable.Add(oEventType, @delegate);
            }
            if (@delegate != null && linstener.GetType() != @delegate.GetType())
            {
                throw new Exception(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", oEventType, @delegate.GetType().Name, linstener.GetType().Name));
            }
            return @delegate;
        }

        public static void AddListener(string oEventType, Action oHandler)
        {
            Action a = (Action)OnAddListenerAdding(oEventType, oHandler);
            EventTable[oEventType] = (Action)Delegate.Combine(a, oHandler);
        }

        public static void AddListener<T>(string oEventType, Action<T> oHandler)
        {
            Action<T> a = (Action<T>)OnAddListenerAdding(oEventType, oHandler);
            EventTable[oEventType] = (Action<T>)Delegate.Combine(a, oHandler);
        }

        public static void AddListener<T, U>(string oEventType, Action<T, U> oHandler)
        {
            Action<T, U> a = (Action<T, U>)OnAddListenerAdding(oEventType, oHandler);
            EventTable[oEventType] = (Action<T, U>)Delegate.Combine(a, oHandler);
        }

        public static void AddListener<T, U, V>(string oEventType, Action<T, U, V> oHandler)
        {
            Action<T, U, V> a = (Action<T, U, V>)OnAddListenerAdding(oEventType, oHandler);
            EventTable[oEventType] = (Action<T, U, V>)Delegate.Combine(a, oHandler);
        }

        public static void OnListenerRemoving(string oEventType, Delegate linstener)
        {
            Delegate @delegate = null;
            if (!EventTable.TryGetValue(oEventType, out @delegate))
            {
                throw new Exception(string.Format("Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.", oEventType));
            }
            if (@delegate == null)
            {
                throw new Exception(string.Format("Attempting to remove listener with for event type \"{0}\" but current listener is null.", oEventType));
            }
            if (@delegate != null && @delegate.GetType() != linstener.GetType())
            {
                throw new Exception(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", oEventType, @delegate.GetType().Name, linstener.GetType().Name));
            }
        }

        public static void OnListenerRemoved(string oEventType)
        {
            Delegate @delegate = null;
            if (EventTable.TryGetValue(oEventType, out @delegate) && @delegate == null)
            {
                EventTable.Remove(oEventType);
            }
        }

        public static void RemoveListener(string oEventType, Action oHandler)
        {
            OnListenerRemoving(oEventType, oHandler);
            EventTable[oEventType] = (Action)Delegate.Remove((Action)EventTable[oEventType], oHandler);
            OnListenerRemoved(oEventType);
        }

        public static void RemoveListener<T>(string oEventType, Action<T> oHandler)
        {
            OnListenerRemoving(oEventType, oHandler);
            EventTable[oEventType] = (Action<T>)Delegate.Remove((Action<T>)EventTable[oEventType], oHandler);
            OnListenerRemoved(oEventType);
        }

        public static void RemoveListener<T, U>(string oEventType, Action<T, U> oHandler)
        {
            OnListenerRemoving(oEventType, oHandler);
            EventTable[oEventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)EventTable[oEventType], oHandler);
            OnListenerRemoved(oEventType);
        }

        public static void RemoveListener<T, U, V>(string oEventType, Action<T, U, V> oHandler)
        {
            OnListenerRemoving(oEventType, oHandler);
            EventTable[oEventType] = (Action<T, U, V>)Delegate.Remove((Action<T, U, V>)EventTable[oEventType], oHandler);
            OnListenerRemoved(oEventType);
        }

        public static void Dispatch(string oEventType)
        {
            Delegate @delegate = null;
            if (EventTable.TryGetValue(oEventType, out @delegate))
            {
                Action callback = @delegate as Action;
                callback();
            }
        }

        public static void Dispatch<T>(string oEventType, T t)
        {
            Delegate @delegate = null;
            if (EventTable.TryGetValue(oEventType, out @delegate))
            {
                Action<T> callback = @delegate as Action<T>;
                callback(t);
            }
        }

        public static void Dispatch<T, U>(string oEventType, T t, U u)
        {
            Delegate @delegate = null;
            if (EventTable.TryGetValue(oEventType, out @delegate))
            {
                Action<T, U> callback = @delegate as Action<T, U>;
                callback(t, u);
            }
        }

        public static void Dispatch<T, U, V>(string oEventType, T t, U u, V v)
        {
            Delegate @delegate = null;
            if (EventTable.TryGetValue(oEventType, out @delegate))
            {
                Action<T, U, V> callback = @delegate as Action<T, U, V>;
                callback(t, u, v);
            }
        }
    }
}
