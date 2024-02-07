using GameCore.Puzzle.Scripts.Level;
using Zenject;

namespace GameCore.Puzzle.Scripts.Installers
{
    public class LevelSwitcherInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelSceneSwitcher>().AsSingle().NonLazy();
        }
    }
}