using System;
using System.Collections.Generic;

namespace ZCSharpLib.Tick
{
    public class Ticker
    {
        private List<Action<float>> TickList;

        public Ticker()
        {
            TickList = new List<Action<float>>();
        }

        public void Attach(Action<float> tick)
        {
            if (!TickList.Contains(tick))
            {
                TickList.Add(tick);
            }
        }

        public void Detach(Action<float> tick)
        {
            if (!TickList.Contains(tick))
            {
                TickList.Remove(tick);
            }
        }

        public void Loop(float lastDuration)
        {
            for (int i = 0; i < TickList.Count; i++)
            {
                Action<float> oTick = TickList[i];
                if (oTick == null) TickList.RemoveAt(i);
                else oTick(lastDuration);
            }
        }
    }
}
