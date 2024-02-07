using GameCore.Meta.Scripts.Currencies.Models;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Presenters
{
    public class SoftCurrencyPresenter : CurrencyPresenter
    {
        [Inject, UsedImplicitly] public SoftCurrencyModel softCurrencyModel { get; }
        public override CurrencyModel model => softCurrencyModel;
    }
}