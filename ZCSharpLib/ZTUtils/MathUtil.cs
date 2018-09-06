﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.ZTUtils
{
    public class MathUtil
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }
    }
}
