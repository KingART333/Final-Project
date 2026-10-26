using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class EmptyState : State, IUpdatableState
    {
        public void Update(float deltaTime)
        {
        }
    }
}
