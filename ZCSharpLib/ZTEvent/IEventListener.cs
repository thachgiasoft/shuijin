﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.ZTEvent
{
    public interface IEventListener
    {
        void OnNotify(IEventArgs args);
    }
}
