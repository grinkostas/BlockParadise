using GameCore.CrossScene.Scripts.Analytics;
using GameCore.CrossScene.Scripts.Saves;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using Zenject;

namespace GameCore.Puzzle.Analytics
{
    public class LevelClassicAnalytics : LevelListener
    {
        [InjectOptional, UsedImplicitly] public IAnalytics analytics { get; }

        private TheSaveProperty<bool> _firstStart = new("Classic_first_open", false);

        protected override void OnLevelStarted()
        {
            if (_firstStart.value)
                return;
            _firstStart.value = true;
            analytics.SendEvent($"first_open", "Classic");
        }
    }
}