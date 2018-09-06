using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCSharpLib.Features.FSM
{
    public abstract class FSMState
    {
        public abstract void Enter(object entity);


        public abstract void Execute(object entity);


        public abstract void Exit(object entity);
    }
}
