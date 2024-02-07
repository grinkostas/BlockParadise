using GameCore.Puzzle.Scripts.Score;
using Zenject;

namespace GameCore.Puzzle.Scripts.Installers
{
    public class ScoreInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ScoreController>().AsSingle().NonLazy();
        }
    }
}