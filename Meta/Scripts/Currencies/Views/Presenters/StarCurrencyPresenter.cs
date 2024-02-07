using GameCore.Meta.Scripts.Currencies.Models;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Presenters
{
    public class StarCurrencyPresenter : CurrencyPresenter
    {
        [Inject, UsedImplicitly] public StarCollectModel starCollectModel { get; }
        public override CurrencyModel model => starCollectModel;
    }
}