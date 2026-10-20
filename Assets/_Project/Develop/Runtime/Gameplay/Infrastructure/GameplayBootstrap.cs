using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using System.Collections;
using UnityEngine;
using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;


namespace Assets._Project.Develop.Runtime.Gameplay.Infrastructure
{
    public class GameplayBootstrap : SceneBootstrap
    {
        private DIContainer _container;
        private GameplayInputArgs _inputArgs;
        private WalletService _walletService;

        [SerializeField] private TestGameplay _testGameplay;
        private EntitiesLifeContext _entitiesLifeContext;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            if (sceneArgs is not GameplayInputArgs gameplayInputArgs)
                throw new ArgumentException($"{nameof(sceneArgs)} is not match with {typeof(GameplayInputArgs)} type");

            _inputArgs = gameplayInputArgs;

            GameplayContextRegistrations.Process(_container, _inputArgs);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log($"you are in Level {_inputArgs.LevelNumber}");

            Debug.Log("Initialization gameplay scene");

            _walletService = _container.Resolve<WalletService>();

            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();

            _testGameplay.Initialize(_container);

            yield break;
        }


        public override void Run()
        {
            Debug.Log("Start gameplay scene");

            _testGameplay.Run();
        }

        private void Update()
        {
            _entitiesLifeContext?.Update(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                if (_walletService.Enough(CurrencyTypes.Gold, 10))
                {
                    _walletService.Spend(CurrencyTypes.Gold, 10);
                    Debug.Log("Gold left " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
                }
                else
                {
                    Debug.Log("Not enough gold");
                }
            }
        }
    }
}
