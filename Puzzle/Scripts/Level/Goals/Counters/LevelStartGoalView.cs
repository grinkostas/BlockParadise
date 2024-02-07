using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Counters
{
    public class LevelStartGoalView : LevelListener
    {
        [SerializeField] private ScoreCounter _scoreCounter;

        protected override void OnLevelStarted()
        {
            if (levelController.TryGetScoreGoal(out var goal) == false)
            {
                gameObject.SetActive(false);
                return;
            }
            _scoreCounter.SetCount(goal.target);
        }
    }
}