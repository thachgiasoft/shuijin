using System;
using System.Collections.Generic;

namespace ZCSharpLib.Common
{
    public interface ITick
    {
        void Loop(float deltaTime);
    }

    public class Tick
    {
        private static List<ITick> TickList = new List<ITick>();

        public static void Attach(ITick tick)
        {
            if (!TickList.Contains(tick))
            {
                TickList.Add(tick);
            }
        }

        public static void Detach(ITick tick)
        {
            if (!TickList.Contains(tick))
            {
                TickList.Remove(tick);
            }
        }

        public static void Loop(float lastDuration)
        {
            for (int i = 0; i < TickList.Count; i++)
            {
                ITick oTick = TickList[i];
                if (oTick != null) TickList.RemoveAt(i);
                else oTick.Loop(lastDuration);
            }
        }
    }

    public class TickLoop : ITick
    {
        private Action<float> LoopAction { get; set; }

        public TickLoop(Action<float> action)
        {
            LoopAction = action;
        }

        public void Start()
        {
            Tick.Attach(this);
        }

        public void Close()
        {
            Tick.Detach(this);
        }

        public void Loop(float deltaTime)
        {
            LoopAction?.Invoke(deltaTime);
        }
    }
}
