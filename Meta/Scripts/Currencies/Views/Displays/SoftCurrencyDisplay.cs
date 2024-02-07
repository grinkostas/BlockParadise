using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Displays
{
    public class SoftCurrencyDisplay : CurrencyDisplay
    {
        [Inject, UsedImplicitly] public SoftCurrencyPresenter softCurrencyModel { get; }
        public override CurrencyPresenter currencyPresenter => softCurrencyModel;

        protected override string FormatAmount(float amount)
        {
            return Mathf.FloorToInt(amount).ToString();
        }
    }
}