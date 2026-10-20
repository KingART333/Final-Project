using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyRegenSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _currentEnergy;
        private ReactiveVariable<float> _regenInterval;
        private ReactiveVariable<float> _regenCurrentTime;
        private ReactiveVariable<float> _regenPercent;
        private ReactiveVariable<bool> _isDead;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.MaxEnergy;
            _currentEnergy = entity.CurrentEnergy;
            _regenInterval = entity.EnergyRegenInterval;
            _regenCurrentTime = entity.EnergyRegenCurrentTime;
            _regenPercent = entity.EnergyRegenPercent;
            _isDead = entity.IsDead;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isDead.Value)
                return;

            _regenCurrentTime.Value += deltaTime;

            if (_regenCurrentTime.Value < _regenInterval.Value)
                return;

            _regenCurrentTime.Value -= _regenInterval.Value;

            _currentEnergy.Value = Mathf.Min(_maxEnergy.Value, _currentEnergy.Value + _maxEnergy.Value * _regenPercent.Value);

            Debug.Log($"Energy restored: {_currentEnergy.Value}");
        }
    }
}