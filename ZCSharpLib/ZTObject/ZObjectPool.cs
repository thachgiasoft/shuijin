using System;
using System.Collections.Generic;

namespace ZCSharpLib.ZTObject
{
    public interface IPool
    {
        void Reset();
    }

    public class ZObjectPool<T> where T : class
    {
        private Stack<T> m_pool;

        public ZObjectPool(int capacity)
        {
            m_pool = new Stack<T>(capacity);
        }

        public void Push(T item)
        {
            if (item == null)
            {
                throw new ArgumentException(string.Format( "Items added to a {0} cannot be null", typeof(T).Name));
            }
            lock (m_pool)
            {
                m_pool.Push(item);
            }
        }

        public T Pop()
        {
            lock (m_pool)
            {
                T t = m_pool.Pop();
                return t;
            }
        }

        public int Count
        {
            get { return m_pool.Count; }
        }
    }
}
