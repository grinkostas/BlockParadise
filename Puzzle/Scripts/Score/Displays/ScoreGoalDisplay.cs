using GameCore.Puzzle.Scripts.Level;
using GameCore.Puzzle.Scripts.Level.Goals;
using GameCore.Puzzle.Scripts.Score.Views;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score.Displays
{
    public class ScoreGoalDisplay : ScoreDisplay
    {
        [SerializeField] private SimpleSlider _simpleSlider;
        [SerializeField] private ScoreGoalView _scoreGoalView;
        
        [Inject, UsedImplicitly] public LevelController levelController { get; }

        private LevelScoreGoal _goal;
        
        protected override void OnEnable()
        {
            levelController.started.On(OnLevelStarted);
        }

        private void OnLevelStarted()
        {
            if(levelController.TryGetScoreGoal(out _goal) == false)
            {
                gameObject.SetActive(false);
                return;
            }

            _scoreGoalView.scoreText.text = ((LevelScoreGoal)levelController.currentGoal).target.ToString();
            base.OnEnable();

            levelController.currentGoal.completed.Once(_scoreGoalView.Complete);
        }

        protected override void ActualizeScore()
        {
            base.ActualizeScore();
            _simpleSlider.value = levelController.currentGoal.GetProgress();
        }
    }
}