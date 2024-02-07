using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Config
{
    [CreateAssetMenu(menuName = "Level/Goal Config")]
    public class LevelGoalConfig : ScriptableObject
    {
        [SerializeField] private List<LevelGoalItemConfig> _goalsData;

        public LevelGoalItemConfig GetGoalItem(int level)
        {
            return (level+1) % 2 == 0 ? _goalsData[1] : _goalsData[0];
        }

        public LevelGoal.GoalType GetNextGoalType(int level)
        {
            return GetGoalItem(level).GetGoal().type;
        }

        public void CompleteGoals()
        {
            foreach (var goalItemConfig in _goalsData)
            {
                goalItemConfig.CompleteGoal();
            }
        }
        
    }
}