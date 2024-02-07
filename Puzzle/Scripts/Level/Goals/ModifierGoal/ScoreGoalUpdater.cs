using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals.ModifierGoal
{
    public class ScoreGoalUpdater : LevelListener
    {
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        
        private LevelScoreGoal _goal;
        
        protected override void OnLevelStarted()
        {
            if(levelController.TryGetScoreGoal(out _goal) == false)
                return;

            scoreController.changed.On(() => _goal.ActualizeGoal(scoreController));;
        }
    }
}