using System;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score.Displays
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private float _animateDuration;
        
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }

        private int _currentScore = 0;
        
        protected virtual void OnEnable()
        {
            scoreController.changed.On(ActualizeScore);
            _currentScore = scoreController.amount;
            ActualizeScore();
        }

        private void OnDisable()
        {
            scoreController.changed.Off(ActualizeScore);
        }

        protected virtual void ActualizeScore()
        {
            DOTween.Kill(this);
            int startScore = _currentScore;
            DOVirtual.Float(0, 1, _animateDuration, value =>
            {
                _currentScore = Mathf.FloorToInt(Mathf.Lerp(startScore, scoreController.amount, value));
                _scoreText.text = _currentScore.ToString();
            }).SetId(this);
        }
    }
}