using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class TeleportToTargetState : State, IUpdatableState
    {
        private ReactiveEvent _teleportRequest;
        private ReactiveVariable<Vector3> _teleportDestination;
        private ReactiveVariable<Entity> _currentTarget;

        public TeleportToTargetState(Entity entity)
        {
            _teleportRequest = entity.TeleportRequest;
            _teleportDestination = entity.TeleportDestination;
            _currentTarget = entity.CurrentTarget;
        }

        public override void Enter()
        {
            base.Enter();

            Entity target = _currentTarget.Value;

            if (target == null)
                return;

            _teleportDestination.Value = target.Transform.position;
            _teleportRequest.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}