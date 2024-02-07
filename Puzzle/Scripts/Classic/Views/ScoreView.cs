using DG.Tweening;
using GameCore.Puzzle.Scripts.Level.Goals.Counters;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Views
{
    public class ScoreView : BaseScoreView
    {
        protected override void SetCounter()
        {
            scoreCounter.SetCount(scoreController.amount);
        }
    }
}