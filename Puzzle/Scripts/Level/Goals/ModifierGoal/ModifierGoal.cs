using System;
using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameCore.Puzzle.Scripts.Level.Goals.ModifierGoal
{
    public abstract class GoalTemplate
    {
        public abstract List<LevelModifierGoal.GoalData> GetGoal(int levelIndex);
    }

    [Serializable]
    public class ConfiguredGoalTemplate : GoalTemplate
    {
        [SerializeField] private List<LevelModifierGoal.GoalData> _data;
        public override List<LevelModifierGoal.GoalData> GetGoal(int levelIndex) => _data;
    }

    public class CalculatedGoalTemplate : GoalTemplate
    {
        private Func<int, int> _calculateSumFunc;
        private TileModifierCollection _modifierCollection;
        public override List<LevelModifierGoal.GoalData> GetGoal(int levelIndex)
        {
            List<LevelModifierGoal.GoalData> goal = new();
            int modifiersCount = Mathf.Min(levelIndex, 5);
            if (modifiersCount <= 1)
            {
                goal.Add(GetGoal(Random.Range(0, _modifierCollection.modifiers.Count), _calculateSumFunc(levelIndex)));
                return goal;
            }
            int sum = _calculateSumFunc(levelIndex);
            int perModifier = sum / modifiersCount;
            int firstModifierCount = sum - perModifier * (modifiersCount - 1);
            
            goal.Add(GetGoal(0, firstModifierCount));
            for (int i = 1; i < modifiersCount-1; i++)
                goal.Add(GetGoal(i, perModifier));

            return goal;
        }
        
        private LevelModifierGoal.GoalData GetGoal(int index, int amount)
        {
            return new LevelModifierGoal.GoalData(_modifierCollection.modifiers[index], amount);
        }
        
        public CalculatedGoalTemplate(TileModifierCollection modifierCollection, Func<int, int> calculateSumFunc)
        {
            _calculateSumFunc = calculateSumFunc;
            _modifierCollection = modifierCollection;
        }
    }
}