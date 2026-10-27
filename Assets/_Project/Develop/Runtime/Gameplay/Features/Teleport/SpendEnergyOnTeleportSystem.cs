using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleport
{
    public class SpendEnergyOnTeleportSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent _teleportEvent;

        private ReactiveVariable<float> _teleportEnergyCost;
        private ReactiveVariable<float> _currentEnergy;

        private IDisposable _teleportEventDisposable;

        public void OnInit(Entity entity)
        {
            _teleportEvent = entity.TeleportEvent;

            _teleportEnergyCost = entity.TeleportEnergyCost;
            _currentEnergy = entity.CurrentEnergy;

            _teleportEventDisposable = _teleportEvent.Subscribe(OnTeleport);
        }

        private void OnTeleport()
        {
            _currentEnergy.Value = Mathf.Max(_currentEnergy.Value - _teleportEnergyCost.Value, 0);
            Debug.Log($"Energy spent on teleport: {_currentEnergy.Value} left");
        }

        public void OnDispose()
        {
            _teleportEventDisposable.Dispose();
        }
    }
}