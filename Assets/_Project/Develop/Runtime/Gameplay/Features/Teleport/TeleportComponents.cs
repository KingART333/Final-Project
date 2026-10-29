using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Teleport
{
    public class TeleportRequest : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class TeleportEvent : IEntityComponent
    {
        public ReactiveEvent Value;
    }

    public class CanTeleport : IEntityComponent
    {
        public ICompositeCondition Value;
    }

    public class TeleportRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportEnergyCost : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportDamage : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportDamageRadius : IEntityComponent
    {
        public ReactiveVariable<float> Value;
    }

    public class TeleportDestination : IEntityComponent
    {
        public ReactiveVariable<Vector3> Value;
    }
}