using DG.Tweening;
using GameCore.CrossScene.Scripts.Animations;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals.Counters
{
    public class LevelFailedScoreCounter : LevelListener
    {
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Transform _scoreCounterContent;
        [SerializeField] private float _showDelay;

        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Show();
        }

        private void Show()
        {
            if (levelController.TryGetScoreGoal(out _) == false)
                gameObject.SetActive(false);
            _scoreCounter.ResetCounter();
            _scoreCounterContent.localScale = Vector3.zero;
            _scoreCounterContent.ZoomIn().SetLink(gameObject).SetDelay(_showDelay)
                .OnComplete(()=> _scoreCounter.SetCount(scoreController.amount));
        }
        
        protected override void OnLevelStarted()
        {
            if (levelController.TryGetScoreGoal(out _) == false)
                gameObject.SetActive(false);
        }
    }
}