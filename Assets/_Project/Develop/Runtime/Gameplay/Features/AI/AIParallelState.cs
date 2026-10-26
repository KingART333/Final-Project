using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class AIParallelState : ParallelState<IUpdatableState>, IUpdatableState
    {
        public AIParallelState(params IUpdatableState[] states) : base(states)
        {
        }

        public void Update(float deltaTime)
        {
            foreach(IUpdatableState state in States)
                state.Update(deltaTime);
        }
    }
}
