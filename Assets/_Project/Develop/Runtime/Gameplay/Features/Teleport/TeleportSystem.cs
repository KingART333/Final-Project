using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleport
{
    public class TeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _teleportRequest;
        private ReactiveEvent _teleportEvent;
        private ICompositeCondition _canTeleport;

        private ReactiveVariable<float> _teleportRadius;
        private ReactiveVariable<Vector3> _teleportDestination;

        private Rigidbody _rigidbody;

        private IDisposable _teleportRequestDisposable;

        public void OnInit(Entity entity)
        {
            _teleportRequest = entity.TeleportRequest;
            _teleportEvent = entity.TeleportEvent;
            _canTeleport = entity.CanTeleport;

            _teleportRadius = entity.TeleportRadius;
            _teleportDestination = entity.TeleportDestination;

            _rigidbody = entity.Rigidbody;

            _teleportRequestDisposable = _teleportRequest.Subscribe(OnTeleportRequest);
        }

        private void OnTeleportRequest()
        {
            if (_canTeleport.Evaluate() == false)
            {
                Debug.Log("Cant teleport");
                return;
            }

            Vector3 position = _rigidbody.transform.position;

            Vector3 toDestination = _teleportDestination.Value - position;
            toDestination.y = 0f;

            Vector3 offset = Vector3.ClampMagnitude(toDestination, _teleportRadius.Value);

            _rigidbody.transform.position = position + offset;

            Debug.Log("Teleported");
            _teleportEvent.Invoke();
        }

        public void OnDispose()
        {
            _teleportRequestDisposable.Dispose();
        }
    }
}