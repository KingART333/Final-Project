using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class RestoreEnergySystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveEvent<float> _restoreRequest;

        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _maxEnergy;

        private ICompositeCondition _canRestoreEnergy;

        private IDisposable _restoreRequestDisposable;

        public void OnInit(Entity entity)
        {
            _restoreRequest = entity.RestoreEnergyRequest;

            _currentEnergy = entity.CurrentEnergy;
            _maxEnergy = entity.MaxEnergy;

            _canRestoreEnergy = entity.CanRestoreEnergy;

            _restoreRequestDisposable = _restoreRequest.Subscribe(OnRestoreRequest);
        }

        private void OnRestoreRequest(float energy)
        {
            if (energy < 0)
                throw new ArgumentOutOfRangeException(nameof(energy));

            if (_canRestoreEnergy.Evaluate() == false)
                return;

            _currentEnergy.Value = Mathf.Min(_currentEnergy.Value + energy, _maxEnergy.Value);
            Debug.Log($"Energy restored: {_currentEnergy.Value}");
        }

        public void OnDispose()
        {
            _restoreRequestDisposable.Dispose();
        }
    }
}