using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Conditions;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Energy
{
    public class MaxEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class CurrentEnergy : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRegenInterval : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRegenCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class EnergyRegenPercent : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class RestoreEnergyRequest : IEntityComponent
    {
        public ReactiveEvent<float> Value;
    }

    public class CanRestoreEnergy : IEntityComponent
    {
        public ICompositeCondition Value;
    }
}