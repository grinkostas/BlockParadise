using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Analytics
{
    [CreateAssetMenu(menuName = "Analytics/Debug Analytics Installer")]
    public class DebugAnalyticsInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IAnalytics>().To<DebugAnalytics>().AsSingle().NonLazy();
        }
    }
}