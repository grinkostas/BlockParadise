using System;
using DG.Tweening;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Score.Views
{
    public class ScoreGoalView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private CanvasGroup _inProgressBackground;
        [SerializeField] private CanvasGroup _completedBackground;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private bool _completedAtStart;

        public TMP_Text scoreText => _scoreText;
        
        private void Start()
        {
            _inProgressBackground.alpha = Convert.ToInt32(!_completedAtStart);
            _completedBackground.alpha = Convert.ToInt32(_completedAtStart);
        }

        public void Complete()
        {
            DOTween.Kill(this);
            _completedBackground.DOFade(1, _fadeDuration).ConfigureWithId(this, gameObject);
            _inProgressBackground.DOFade(0, _fadeDuration).ConfigureWithId(this, gameObject);
        }

        public void SetInProgress()
        {
            DOTween.Kill(this);
            _completedBackground.DOFade(0, _fadeDuration).ConfigureWithId(this, gameObject);
            _inProgressBackground.DOFade(1, _fadeDuration).ConfigureWithId(this, gameObject);
        }
    }
}