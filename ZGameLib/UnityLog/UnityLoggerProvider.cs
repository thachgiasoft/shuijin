using ZCSharpLib;
using ZCSharpLib.Common;
using ZCSharpLib.Common.Provider;

namespace ZGameLib.UnityLog
{
    public class UnityLoggerProvider : IZServiceProvider
    {
        private UnityLogger Logger { get; set; }

        public void Register()
        {
            Logger = App.Singleton<UnityLogger>();
        }

        public void Initialize()
        {
            if(Logger != null) ZLogger.AddListener(Logger);
        }

        public void Uninitialize()
        {
            if (Logger != null) ZLogger.RemoveListener(Logger);
        }

        public void Unregister()
        {
        }
    }
}
