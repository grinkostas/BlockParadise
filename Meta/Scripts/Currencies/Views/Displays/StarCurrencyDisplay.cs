using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Views.Displays
{
    public class StarCurrencyDisplay : CurrencyDisplay
    {
        [Inject, UsedImplicitly] public StarCurrencyPresenter presenter { get; }
        
        public override CurrencyPresenter currencyPresenter => presenter;
    }
}