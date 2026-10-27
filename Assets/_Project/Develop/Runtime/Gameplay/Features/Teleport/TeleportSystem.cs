using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleport
{
    public class TeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _teleportRequest;
        private ReactiveEvent _teleportEvent;
        private ICompositeCondition _canTeleport;

        private ReactiveVariable<float> _teleportRadius;

        private Rigidbody _rigidbody;

        private IDisposable _teleportRequestDisposable;

        public void OnInit(Entity entity)
        {
            _teleportRequest = entity.TeleportRequest;
            _teleportEvent = entity.TeleportEvent;
            _canTeleport = entity.CanTeleport;

            _teleportRadius = entity.TeleportRadius;

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

            Vector2 offset = Random.insideUnitCircle * _teleportRadius.Value;

            _rigidbody.transform.position += new Vector3(offset.x, 0f, offset.y);

            Debug.Log("Teleported");
            _teleportEvent.Invoke();
        }

        public void OnDispose()
        {
            _teleportRequestDisposable.Dispose();
        }
    }
}