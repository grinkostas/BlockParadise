using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Installers
{
    public class StarCurrencyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<StarCollectModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<StarCurrencyPresenter>().AsSingle().NonLazy();
        }
    }
}