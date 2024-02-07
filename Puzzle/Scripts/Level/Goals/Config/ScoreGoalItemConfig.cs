using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Config
{
    [CreateAssetMenu(menuName = "Level/Score Goal Item Config")]
    public class ScoreGoalItemConfig : LevelGoalItemConfig
    {
        [SerializeField] private int _scoreGoalForLevel = 64;
        [SerializeField] private int _maxScoreGoal = 9999;
        
        private LevelScoreGoal _levelScoreGoal;
        
        public override LevelGoal GetGoal()
        {
            if (_levelScoreGoal == null)
                SetGoal();
            return _levelScoreGoal;
        }

        protected override void OnSetGoal()
        {
            _levelScoreGoal = new();
            _levelScoreGoal.SetGoal(Mathf.Min(_maxScoreGoal, _scoreGoalForLevel * (LevelController.levelIndex/2+1)));
        }

        public override void GenerateLevel(LevelPatternGenerator patternGenerator)
        {
            int complexity = Mathf.Clamp(LevelController.levelIndex/2, 2, 6);
            patternGenerator.GeneratePattern(LevelController.levelIndex, LevelController.levelIndex, complexity);
        }
        
        public override void CompleteGoal()
        {
            _levelScoreGoal = null;
        }
    }
}