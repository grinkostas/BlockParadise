using GameCore.CrossScene.Scripts.UI.Popups;
using Zenject;

namespace GameCore.CrossScene.Scripts.Installers
{
    public class PopupManagerInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PopupManager>().AsSingle().NonLazy();
        }
    }
}