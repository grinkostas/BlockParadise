using System;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Classic.Core;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Views
{
    public class BestScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _bestScoreText;
        [SerializeField] private float _textAnimation;
        
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        [Inject, UsedImplicitly] public ClassicLevelController classicLevelController { get; }

        private int _currentScore = 0;
        
        private void OnEnable()
        {
            scoreController.changed.On(Actualize);
            Actualize();
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
            scoreController.changed.On(Actualize);
        }

        private void Actualize()
        {
            DOTween.Kill(this);
            int startScore = _currentScore;
            _bestScoreText.DoCounter(startScore, Mathf.Max(scoreController.amount, classicLevelController.maxScore),
                _textAnimation, ActualizeCurrentValue).ConfigureWithId(this, gameObject);
        }

        private void ActualizeCurrentValue(int value)
        {
            _currentScore = value;
        }
    }
}