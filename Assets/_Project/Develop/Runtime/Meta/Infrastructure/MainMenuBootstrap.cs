using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement;
using Assets._Project.Develop.Runtime.Utilities.DataManagement.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Meta.Infrastructure
{
    public class MainMenuBootstrap : SceneBootstrap
    {
        private DIContainer _container;

        private WalletService _walletService;

        private PlayerDataProvider _playerDataProvider;

        private ICoroutinesPerformer _coroutinesPerformer;

        [SerializeField] private IconTextView _currencyView;
        private ProjectPresentersFactory _presentersFactory;
        private CurrencyPresenter _currencyPresenter;

        public override void ProcessRegistrations(DIContainer container, IInputSceneArgs sceneArgs = null)
        {
            _container = container;

            MainMenuContextRegistrations.Process(_container);
        }

        public override IEnumerator Initialize()
        {
            Debug.Log("Initialization gameplay Menu");

            _walletService = _container.Resolve<WalletService>();

            _playerDataProvider = _container.Resolve<PlayerDataProvider>();

            _coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();

            _presentersFactory = _container.Resolve<ProjectPresentersFactory>();

            yield break;
        }

        public override void Run()
        {
            Debug.Log("Start gameplay Menu");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneSwitcherService sceneSwitcherService = _container.Resolve<SceneSwitcherService>();
                ICoroutinesPerformer coroutinesPerformer = _container.Resolve<ICoroutinesPerformer>();
                coroutinesPerformer.StartPerform(sceneSwitcherService.ProcessSwitchTo(Scenes.Gameplay, new GameplayInputArgs(2)));
            }

            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                _walletService.Add(CurrencyTypes.Gold, 10);
                Debug.Log("Gold left " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
            }

            if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                if (_walletService.Enough(CurrencyTypes.Gold, 10))
                {
                    _walletService.Spend(CurrencyTypes.Gold, 10);
                    Debug.Log("Gold left " + _walletService.GetCurrency(CurrencyTypes.Gold).Value);
                }
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                _coroutinesPerformer.StartPerform(_playerDataProvider.Save());
                Debug.Log("Saving was called");
            }

            if (Input.GetKeyUp(KeyCode.G))
            {
                _currencyPresenter?.Disable();

                _currencyPresenter = _presentersFactory.CreateCurrencyPresenter(_currencyView, _walletService.GetCurrency(CurrencyTypes.Gold), CurrencyTypes.Gold);

                _currencyPresenter.Enable();
            }

            if (Input.GetKeyUp(KeyCode.D))
            {
                _currencyPresenter?.Disable();

                _currencyPresenter = _presentersFactory.CreateCurrencyPresenter(_currencyView, _walletService.GetCurrency(CurrencyTypes.Diamond), CurrencyTypes.Diamond);

                _currencyPresenter.Enable();
            }
        }
    }
}
