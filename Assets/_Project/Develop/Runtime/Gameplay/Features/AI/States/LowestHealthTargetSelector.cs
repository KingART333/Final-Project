using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class LowestHealthTargetSelector : ITargetSelector
    {
        private Entity _source;

        public LowestHealthTargetSelector(Entity entity)
        {
            _source = entity;
        }

        public Entity SelectTargetFrom(IEnumerable<Entity> targets)
        {
            Entity lowestHealthTarget = null;
            float minHealth = float.MaxValue;

            foreach (Entity target in targets)
            {
                if (target == _source)
                    continue;

                if (target.HasComponent<TakeDamageRequest>() == false)
                    continue;

                if (target.HasComponent<CurrentHealth>() == false)
                    continue;

                if (target.TryGetCanApplyDamage(out ICompositeCondition canApplyDamage) && canApplyDamage.Evaluate() == false)
                    continue;

                float health = target.CurrentHealth.Value;

                if (health < minHealth)
                {
                    minHealth = health;
                    lowestHealthTarget = target;
                }
            }

            return lowestHealthTarget;
        }
    }
}