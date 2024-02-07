using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using NepixSignals;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals
{
    public class LevelScoreGoal : LevelGoal
    {
        public override GoalType type => GoalType.Score;

        private float _progress = 0;
        public int target { get; private set; }

        
        public override float GetProgress()
        {
            return _progress;
        }
        
        public void SetGoal(int scoreGoal)
        {
            target = scoreGoal;
        }

        public void ActualizeGoal(ScoreController scoreController)
        {
            _progress = scoreController.amount / (float)target;
            if(scoreController.amount >= target)
                Complete();
        }
        
        
    }
}