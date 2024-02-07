using Cinemachine;
using GameCore.CrossScene.Scripts.Analytics;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Analytics
{
    public class LevelAnalytics : LevelListener
    {
        [InjectOptional, UsedImplicitly] public IAnalytics analytics { get; }
        
        protected override void OnLevelFailed()
        {
            analytics.SendFailed($"level_{levelController.currentLevelIndex}", "Level");
        }
        
        protected override void OnLevelCompleted()
        {
            analytics.SendComplete($"level_{Mathf.Max(0, levelController.currentLevelIndex-1)}", "Level");
        }
    }
}