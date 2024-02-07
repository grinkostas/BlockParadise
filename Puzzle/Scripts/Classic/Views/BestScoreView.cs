using DG.Tweening;
using GameCore.Puzzle.Scripts.Classic.Core;
using GameCore.Puzzle.Scripts.Level.Goals.Counters;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Views
{
    public class BestScoreView : BaseScoreView
    {
        [Inject, UsedImplicitly] public ClassicLevelController classicLevelController { get; }
        
        protected override void SetCounter()
        {
            scoreCounter.SetCount(Mathf.Max(scoreController.amount, classicLevelController.maxScore));
        }
    }
}