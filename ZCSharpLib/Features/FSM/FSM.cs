using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.FSM
{
    public class FSM
    {
        private FSMState CurState;
        private FSMState PreviousState;
        private FSMState GlobalCurState;
        private FSMState GlobalPreviousState;

        private object Entity;

        public FSM(object entity)
        {
            Entity = entity;
        }

        public void ChangeState(FSMState state)
        {
            if (CurState != null)
            {
                CurState.Exit(Entity);
            }
            PreviousState = CurState;
            CurState = state;
            if (CurState != null)
            {
                CurState.Enter(Entity);
            }
        }

        public void ChangeGlobalState(FSMState state)
        {
            if (GlobalCurState != null)
            {
                GlobalCurState.Exit(Entity);
            }
            GlobalPreviousState = GlobalCurState;
            GlobalCurState = state;
            if (GlobalCurState != null)
            {
                GlobalCurState.Enter(Entity);
            }
        }

        public FSMState GetState()
        {
            return CurState;
        }

        public FSMState GetGlobalState()
        {
            return GlobalCurState;
        }

        public void Tick()
        {
            if (CurState != null)
            {
                CurState.Execute(Entity);
            }

            if (GlobalCurState != null)
            {
                GlobalCurState.Execute(Entity);
            }
        }
    }
}
