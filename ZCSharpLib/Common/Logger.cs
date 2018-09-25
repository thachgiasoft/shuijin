using System;
using System.Collections.Generic;
using ZCSharpLib.ZTUtils;

namespace ZCSharpLib.Common
{
	public interface ILoggerListener
	{
		void Log(string msg);
        void Warning(string msg);
        void Error(string msg);
	};

	public class Logger
    {
        public enum LoggerChannel
        {
            DEFAULT = 0,
            DEBUG,
            INFO,
            NETMSG, // Õ¯¬Áœ˚œ¢œ‘ æ
            WARNING,
            ERROR,
            NUM
        }

        private List<ILoggerListener> Listeners;

        public Logger()
        {
            Listeners = new List<ILoggerListener>();
            Type[] types = ZCommUtil.GetTypes();
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (type.IsClass && type.GetInterface(typeof(ILoggerListener).Name) != null)
                {
                    ILoggerListener listener = Activator.CreateInstance(type) as ILoggerListener;
                    Listeners.Add(listener);
                }
            }
        }

        public void Debug(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.DEBUG, false);
        }

        public void Warning(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.WARNING, false);
        }

        public void Info(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.INFO, false);
        }

        public void NetMsg(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.NETMSG, false);
        }

        public void Error(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.ERROR, false);
        }

		private void Log(string msg, LoggerChannel channel, bool simpleMode)
		{
            string outputMsg = string.Empty;
			if(simpleMode){
				outputMsg = msg;
			} else {
                outputMsg = "[" + channel.ToString() + "][" + DateTime.Now.ToString() + "]  " + msg;
			}
            foreach (ILoggerListener listener in Listeners)
            {
                switch (channel)
                {
                    case LoggerChannel.DEFAULT:
                    case LoggerChannel.DEBUG:
                    case LoggerChannel.INFO:
                    case LoggerChannel.NETMSG:
                        listener.Log(outputMsg);
                        break;
                    case LoggerChannel.WARNING:
                        listener.Warning(outputMsg);
                        break;
                    case LoggerChannel.ERROR:
                        listener.Error(outputMsg);
                        break;
                }
            }
			Console.WriteLine(outputMsg);
		}
    };
}

