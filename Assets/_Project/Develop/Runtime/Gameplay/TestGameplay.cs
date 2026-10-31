using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay
{
    public class TestGameplay : MonoBehaviour
    {
        [SerializeField] private bool _useSmartTeleporter;

        private DIContainer _container;
        private EntitiesFactory _entitiesFactory;
        private BrainsFactory _brainsFactory;

        private Entity _entity;
        private Entity _teleporter;

        private bool _isRunning;

        public void Initialize(DIContainer container)
        {
            _container = container;
            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
        }

        public void Run()
        {
            _entity = _entitiesFactory.CreateHero(Vector3.zero);
            _entitiesFactory.CreateGhost(Vector3.zero + Vector3.forward * 5);
            _teleporter = _entitiesFactory.CreateTeleporter(Vector3.zero + Vector3.right * 5);

            _brainsFactory.CreateMainHeroManualBrain(_entity);

            if (_useSmartTeleporter)
                _brainsFactory.CreateTeleporterSmartBrain(_teleporter, 3f);
            else
                _brainsFactory.CreateTeleporterRandomBrain(_teleporter, 3f);

            _isRunning = true;
        }

        private void Update()
        {
            if (_isRunning == false)
                return;

            //for testing
            if (Input.GetKeyDown(KeyCode.Space))
                _entity.TakeDamageRequest.Invoke(50);

            //for testing
            if (Input.GetKeyDown(KeyCode.Y))
                _teleporter.TakeDamageRequest.Invoke(50);
        }
    }
}