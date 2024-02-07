using GameCore.Meta.Scripts.Currencies.Models;
using GameCore.Meta.Scripts.Currencies.Views.Presenters;
using Zenject;

namespace GameCore.Meta.Scripts.Currencies.Installers
{
    public class SoftCurrencyInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SoftCurrencyModel>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<SoftCurrencyPresenter>().AsSingle().NonLazy();
        }
    }
}