using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ZCSharpLib.Common;

namespace ZGameLib.UnityLog
{
    public class UnityLogger : ILoggerListener
    {
        public void Log(string msg)
        {
            Debug.Log(msg);
        }

        public void Warning(string msg)
        {
            Debug.LogWarning(msg);
        }

        public void Error(string msg)
        {
            Debug.LogError(msg);
        }
    }
}

