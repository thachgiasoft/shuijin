using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTool.Core
{
    public class ZLogger
    {
        private enum LogType
        {
            Info,
            Warning,
            Error,
        }
        public string Content { get; private set; }

        private Queue<string> Logs = new Queue<string>();
        private Action<string> OnFinished { get; set; }

        public void Setup(Action<string> onFinished)
        {
            OnFinished = onFinished;
        }

        public void Error(string str, params object[] args)
        {
            ToLog(LogType.Error, string.Format(str, args));
        }

        public void Info(string str, params object[] args)
        {
            ToLog(LogType.Info, string.Format(str, args));
        }

        public void Warning(string str, params object[] args)
        {
            ToLog(LogType.Warning, string.Format(str, args));
        }

        private void ToLog(LogType oLogType, string str)
        {
            string content = "[" + oLogType.ToString() + "]" + "\t" + str + "\n";

            if (Logs.Count > 999)
            {
                string firstStr = Logs.Dequeue();
                Content = Content.Remove(0, firstStr.Length);
            }
            Logs.Enqueue(content);
            Content = Content + content;
            if (OnFinished != null) OnFinished(Content);
        }
    }
}
