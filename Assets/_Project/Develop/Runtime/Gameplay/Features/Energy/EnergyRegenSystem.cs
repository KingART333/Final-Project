using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class EnergyRegenSystem : IInitializableSystem, IUpdatableSystem
    {
        private ReactiveVariable<float> _maxEnergy;
        private ReactiveVariable<float> _regenInterval;
        private ReactiveVariable<float> _regenCurrentTime;
        private ReactiveVariable<float> _regenPercent;

        private ReactiveEvent<float> _restoreRequest;

        public void OnInit(Entity entity)
        {
            _maxEnergy = entity.MaxEnergy;
            _regenInterval = entity.EnergyRegenInterval;
            _regenCurrentTime = entity.EnergyRegenCurrentTime;
            _regenPercent = entity.EnergyRegenPercent;

            _restoreRequest = entity.RestoreEnergyRequest;
        }

        public void OnUpdate(float deltaTime)
        {
            _regenCurrentTime.Value += deltaTime;

            if (_regenCurrentTime.Value < _regenInterval.Value)
                return;

            _regenCurrentTime.Value -= _regenInterval.Value;

            _restoreRequest.Invoke(_maxEnergy.Value * _regenPercent.Value);
        }
    }
}