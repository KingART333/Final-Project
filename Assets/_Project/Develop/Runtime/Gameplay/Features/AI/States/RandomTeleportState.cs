using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomTeleportState : State, IUpdatableState
    {
        private ReactiveEvent _teleportRequest;
        private ReactiveVariable<Vector3> _teleportDestination;
        private ReactiveVariable<float> _teleportRadius;
        private Transform _transform;

        public RandomTeleportState(Entity entity)
        {
            _teleportRequest = entity.TeleportRequest;
            _teleportDestination = entity.TeleportDestination;
            _teleportRadius = entity.TeleportRadius;
            _transform = entity.Transform;
        }

        public override void Enter()
        {
            base.Enter();

            Vector2 offset = Random.insideUnitCircle * _teleportRadius.Value;

            _teleportDestination.Value = _transform.position + new Vector3(offset.x, 0f, offset.y);
            _teleportRequest.Invoke();
        }

        public void Update(float deltaTime)
        {
        }
    }
}