using System;
using System.Collections.Generic;

namespace ZCSharpLib.Common
{
	public interface ILoggerListener
	{
		void Log(string msg);
        void Warning(string msg);
        void Error(string msg);
	};

	public class ZLogger
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

        private static List<ILoggerListener> Listeners = new List<ILoggerListener>();

        public static void AddListener(ILoggerListener listener)
        {
            Listeners.Add(listener);
        }

        public static void RemoveListener(ILoggerListener listener)
        {
            Listeners.Remove(listener);
        }

        public static void Debug(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.DEBUG, false);
        }

        public static void Warning(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.WARNING, false);
        }

        public static void Info(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.INFO, false);
        }

        public static void NetMsg(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.NETMSG, false);
        }

        public static void Error(object msg, params object[] args)
        {
            Log(string.Format(msg.ToString(), args), LoggerChannel.ERROR, false);
        }

		private static void Log(string msg, LoggerChannel channel, bool simpleMode)
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

