using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Project.Develop.Runtime.Gameplay.EntitiesCore
{
    public class EntitiesLifeContext : IDisposable
    {
        public event Action<Entity> Added;
        public event Action<Entity> Released;

        private readonly List<Entity> _entities = new();
        private readonly List<Entity> _releaseRequests = new();

        public IEnumerable<Entity> Entities => _entities;

        public void Add(Entity entity)
        {
            _entities.Add(entity);

            entity.Initialize();

            Added?.Invoke(entity);
        }

        public void Update(float deltaTime)
        {
            for(int i = 0; i < _entities.Count; i++)
                _entities[i].OnUpdate(deltaTime);

            foreach (var entity in _releaseRequests)
            {
                _entities.Remove(entity);
                entity.Dispose();
                Released?.Invoke(entity);
            }

            _releaseRequests.Clear();
        }

        public void Release(Entity entity)
        {
            if (entity == null || !_entities.Contains(entity))
                return;

            if (_releaseRequests.Contains(entity))
                return;

            _releaseRequests.Add(entity);
        }

        public void Dispose()
        {
            foreach (var entity in _entities)
                entity.Dispose();

            _entities.Clear();
            _releaseRequests.Clear();
        }
    }
}
