using Assets._Project.Develop.Runtime.Configs.Meta.Wallet;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.UI.Wallet
{
    public class CurrencyPresenter
    {
        private readonly IReadOnlyVariable<int> _currency;
        private readonly CurrencyTypes _currencyType;
        private readonly CurrencyIconConfig _currencyIconConfig;

        private readonly IconTextView _view;

        private IDisposable _disposable;

        public CurrencyPresenter(IReadOnlyVariable<int> currency, CurrencyTypes currencyType, CurrencyIconConfig currencyIconConfig, IconTextView view)
        {
            _currency = currency;
            _currencyType = currencyType;
            _currencyIconConfig = currencyIconConfig;
            _view = view;
        }

        public void Enable()
        {
            UpdateValue(_currency.Value);
            _view.SetIcon(_currencyIconConfig.GetSpriteFor(_currencyType));

            _disposable = _currency.Subscribe(OnCurrencyChanged);
        }

        public void Disable()
        {
            _disposable.Dispose();
        }

        private void OnCurrencyChanged(int arg1, int newValue) => UpdateValue(newValue);

        private void UpdateValue(int value) => _view.SetText(value.ToString());
    }
}
