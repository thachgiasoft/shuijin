using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Core
{
    public class AppMain
    {
        private static List<IApplication> Apps = new List<IApplication>();

        public static void Startup()
        {
            ToAssembly();
            ToInitialize();
        }

        public static void ToAssembly()
        {
            Type[] types = typeof(AppMain).Assembly.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.IsClass && type.GetInterface(typeof(IApplication).Name) != null)
                {
                    IApplication app = Activator.CreateInstance(type) as IApplication;
                    Apps.Add(app);
                }
            }
        }

        public static void ToInitialize()
        {
            for (int i = 0; i < Apps.Count; i++)
            {
                IApplication app = Apps[i];
                app.Initialize();
            }
        }

        public static void ToUninitialize()
        {
            for (int i = 0; i < Apps.Count; i++)
            {
                IApplication app = Apps[i];
                app.Uninitialize();
            }
        }

        public static T Get<T>() where T : class
        {
            for (int i = 0; i < Apps.Count; i++)
            {
                IApplication app = Apps[i];
                if (app.GetType() == typeof(T))
                {
                    return app as T;
                }
            }
            return default(T);
        }
    }
}
