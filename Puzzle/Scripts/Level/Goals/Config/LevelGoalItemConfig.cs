using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Tiles;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Config
{
    [CreateAssetMenu(menuName = "Level/Goal Item Config")]
    public abstract class LevelGoalItemConfig : ScriptableObject
    {
        protected int level => LevelController.levelIndex;
        public abstract LevelGoal GetGoal();

        public void SetGoal()
        {
            OnSetGoal();
        }
        
        protected abstract void OnSetGoal();
        public virtual void ApplyGoalForBoard(List<BoardTile> tiles){}
        public abstract void GenerateLevel(LevelPatternGenerator patternGenerator);
        public abstract void CompleteGoal();
    }
}