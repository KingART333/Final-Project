using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleport
{
    public class TeleportAreaDamageSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly CollidersRegistryService _collidersRegistryService;

        private Entity _entity;

        private ReactiveEvent _teleportEvent;
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<float> _damageRadius;

        private Buffer<Collider> _contacts;
        private LayerMask _mask;

        private Transform _transform;

        private List<Entity> _processedEntities;

        private IDisposable _teleportEventDisposable;

        public TeleportAreaDamageSystem(CollidersRegistryService collidersRegistryService)
        {
            _collidersRegistryService = collidersRegistryService;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _teleportEvent = entity.TeleportEvent;
            _damage = entity.TeleportDamage;
            _damageRadius = entity.TeleportDamageRadius;

            _contacts = entity.ContactCollidersBuffer;
            _mask = entity.ContactsDetectingMask;

            _transform = entity.Rigidbody.transform;

            _processedEntities = new List<Entity>(_contacts.Items.Length);

            _teleportEventDisposable = _teleportEvent.Subscribe(OnTeleport);
        }

        private void OnTeleport()
        {
            _processedEntities.Clear();
            Physics.SyncTransforms();

            _contacts.Count = Physics.OverlapSphereNonAlloc(
                _transform.position,
                _damageRadius.Value,
                _contacts.Items,
                _mask,
                QueryTriggerInteraction.Collide);

            for (int i = 0; i < _contacts.Count; i++)
            {
                Entity contactEntity = _collidersRegistryService.GetBy(_contacts.Items[i]);

                if (contactEntity == null || contactEntity == _entity)
                    continue;

                if (_processedEntities.Contains(contactEntity))
                    continue;

                _processedEntities.Add(contactEntity);

                if (contactEntity.HasComponent<TakeDamageRequest>())
                    contactEntity.TakeDamageRequest.Invoke(_damage.Value);
            }
        }

        public void OnDispose()
        {
            _teleportEventDisposable.Dispose();
        }
    }
}