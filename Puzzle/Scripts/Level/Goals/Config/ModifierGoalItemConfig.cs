using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using GameCore.Puzzle.Scripts.Level.Configs;
using GameCore.Puzzle.Scripts.Level.Goals.ModifierGoal;
using NUnit.Framework;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Config
{
    [CreateAssetMenu(menuName = "Level/Modifier Goal Item Config")]
    public class ModifierGoalItemConfig : LevelGoalItemConfig
    {
        [SerializeField] private TileModifierCollection _tileModifierCollection;
        [SerializeField] private List<ConfiguredGoalTemplate> _firstLevelTemplates;
        [SerializeField] private float _exponentCoeficient;
        [SerializeField] private int _startModifierSum;
        [SerializeField] private int _maxSum;

        private LevelModifierGoal _levelModifierGoal;

        
        private List<LevelModifierGoal.GoalData> _currentGoal = new();

        private List<GoalTemplate> _goalTemplates = new();
        private List<GoalTemplate> goalTemplates
        {
            get
            {
                if (_goalTemplates.Count > 0) return _goalTemplates;
                
                _goalTemplates.AddRange(_firstLevelTemplates);
                _goalTemplates.Add(new CalculatedGoalTemplate(_tileModifierCollection, GetExponentSum));
                return _goalTemplates;
            }
        }

        public override LevelGoal GetGoal()
        {
            if (_levelModifierGoal == null)
                SetGoal();
            return _levelModifierGoal;
        }

        public override void CompleteGoal()
        {
            _levelModifierGoal = null;
            _currentGoal = new();
            _goalTemplates = new();
        }

        public List<LevelModifierGoal.GoalData> GetCurrentGoal()
        {
            List<LevelModifierGoal.GoalData> goal;
            if (level/2 < _firstLevelTemplates.Count)
                return _firstLevelTemplates[level/2].GetGoal(level/2);
            
            
            if (ES3.KeyExists(GetSaveString()))
            {
                var savedGoal = ES3.Load<List<LevelModifierGoal.GoalSaveData>>(GetSaveString());
                goal = ConvertFromSave(savedGoal);
            }
            else
            {
                var template = goalTemplates[Mathf.Min(goalTemplates.Count - 1, level/2)];
                goal = template.GetGoal(level/2);
                ES3.Save(GetSaveString(), ConvertToSave(goal));  
            }

            return goal;
        }

        private List<LevelModifierGoal.GoalData> ConvertFromSave(List<LevelModifierGoal.GoalSaveData> goal)
        {
            return goal.Select(item =>
                new LevelModifierGoal.GoalData(_tileModifierCollection.Get(item.modifierId), item.amount)).ToList();
        }
        
        private List<LevelModifierGoal.GoalSaveData> ConvertToSave(List<LevelModifierGoal.GoalData> goal)
        {
            return goal.Select(item =>
                new LevelModifierGoal.GoalSaveData(item.modifier.id, item.amount)).ToList();
        }


        protected override void OnSetGoal()
        {
            _currentGoal = GetCurrentGoal();
            _levelModifierGoal = new LevelModifierGoal();
            _levelModifierGoal.SetGoal(_currentGoal);
        }

        private string GetSaveString()
        {
            return $"ModifierLevelGoalSaveItem_{level}";
        }
        
        private int GetExponentSum(int levelIndex)
        {
            var coefficient = Mathf.Pow(_exponentCoeficient,levelIndex);
            return Mathf.RoundToInt(Mathf.Min(_maxSum, coefficient * _startModifierSum));
        }

        public List<LevelModifierGoal.GoalData> GetGoalData()
        {
            var goalData = new List<LevelModifierGoal.GoalData>();
            foreach (var data in _currentGoal)
            {
                var newData = new LevelModifierGoal.GoalData(data);
                newData.amount = Mathf.CeilToInt(newData.amount/2.0f);
                goalData.Add(newData);
            }

            return goalData;
        }
        
        public override void ApplyGoalForBoard(List<BoardTile> tiles)
        {
            int maxModifiersCount = Mathf.Min(tiles.Count, _currentGoal.Sum(x => x.amount)/2);
            ApplyGoalForBoard(tiles.OrderBy(x => Random.Range(0, int.MaxValue)).Take(maxModifiersCount).ToList(), GetGoalData());
        }
        
        public void ApplyGoalForBoard(List<BoardTile> tiles, List<LevelModifierGoal.GoalData> goalData)
        {
            foreach (var tile in tiles)
            {
                if(goalData.Count == 0)
                    break;

                var randomGoal = goalData.Random();
                randomGoal.amount--;
                if (randomGoal.amount <= 0)
                    goalData.Remove(randomGoal);
                
                tile.SetModifier(randomGoal.modifier);
            }
        }

        public override void GenerateLevel(LevelPatternGenerator patternGenerator)
        {
            int complexity = Mathf.Clamp(LevelController.levelIndex/4, 1, 4);
            int templatesCount = Mathf.Clamp(LevelController.levelIndex/4, 2, 5);
            var tiles = patternGenerator.GeneratePattern(LevelController.levelIndex, templatesCount , complexity);
            
            ApplyGoalForBoard(tiles);
        }
    }
}