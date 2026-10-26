using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class AiStateMachine : StateMachine<IUpdatableState>
    {
        public AiStateMachine(List<IDisposable> disposables) : base(disposables)
        {
        }

        public AiStateMachine() : base (new List<IDisposable>())
        {   
        }

        protected override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);

            CurrentState?.Update(deltaTime);
        }
    }
}
