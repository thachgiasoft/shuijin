using System;

namespace ZCSharpLib.Common
{
    public class Singleton<T> where T : class, new()
    {
        private static object _object = new object();

        private static T _instance = null;
        public static T instance
        {
            get
            {
                lock (_object)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }
                return _instance;
            }
        }
    }
}
