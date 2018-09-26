using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using ZCSharpLib.Common;
using ZCSharpLib.Common.Provider;
using ZCSharpLib.Tick;
using ZCSharpLib.ZTThread;

namespace ZCSharpLib
{
    public class App
    {
        /// <summary>
        /// ʱ��ѭ��
        /// </summary>
        public static readonly Ticker Ticker = new Ticker();
        /// <summary>
        /// ��־���
        /// </summary>
        public static readonly Logger Logger = new Logger();
        /// <summary>
        /// ���߳�
        /// </summary>
        public static readonly MainThread MainThread = new MainThread();

        #region ������
        private static Dictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static T Singleton<T>(params object[] args) where T : class, new()
        {
            return Singleton(typeof(T), args) as T;
        }

        public static object Singleton(Type type, params object[] args)
        {
            lock (Instances)
            {
                object value = null;
                if (!Instances.TryGetValue(type, out value))
                {
                    value = Make(type, args);
                    Instances.Add(type, value);
                }
                return value;
            }
        }

        public static T Make<T>(params object[] args) where T : class, new()
        {
            return Make(typeof(T), args) as T;
        }

        public static object Make(Type type, params object[] args)
        {
            return GetConstructor(type, args).Invoke(args);
        }

        public static ConstructorInfo GetConstructor(Type type, params object[] args)
        {
            ConstructorInfo constructorInfo = null;
            ConstructorInfo[] constructors = type.GetConstructors();
            for (int i = 0; i < constructors.Length; i++)
            {
                ConstructorInfo constructorInfo2 = constructors[i];
                ParameterInfo[] parameters = constructorInfo2.GetParameters();
                if (parameters.Length == args.Length)
                {
                    bool flag = true;
                    for (int j = 0; j < parameters.Length; j++)
                    {
                        ParameterInfo parameterInfo = parameters[i];
                        object obj = args[i];
                        if (parameterInfo.ParameterType != obj.GetType())
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        constructorInfo = constructorInfo2;
                        break;
                    }
                }
            }
            if (constructorInfo == null)
            {
                throw new ArgumentException($"û���ҵ�����{type.Name}�Ķ�Ӧ���췽��!\n{new StackTrace()}");
            }
            return constructorInfo;
        }
        #endregion

        #region �������
        private static readonly ServiceProvider ServiceProvider = new ServiceProvider();

        public static void Register(IZServiceProvider[] serviceProviders)
        {
            ServiceProvider.Register(serviceProviders);
        }

        public static void Initialize()
        {
            ServiceProvider.Initialize();
        }

        public static void Uninitialize()
        {
            ServiceProvider.Uninitialize();
        }

        public static void Unregister()
        {
            ServiceProvider.Unregister();
        }
        #endregion

        #region ʱ��ѭ��
        public static void AttachTick(Action<float> tick)
        {
            Ticker.Attach(tick);
        }

        public static void DetachTick(Action<float> tick)
        {
            Ticker.Detach(tick);
        }

        public static void Loop(float deltaTime)
        {
            Ticker.Loop(deltaTime);
        }
        #endregion
    }
}