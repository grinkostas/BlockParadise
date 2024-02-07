using System;
using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Puzzle.Scripts.Level.Goals;
using GameCore.Puzzle.Scripts.Level.Goals.Config;
using TMPro;
using UnityEngine;

namespace GameCore.Meta.Scripts.Popups.NextLevel
{
    public class NextLevelGoalView : MonoBehaviour
    {
        [SerializeField] private LevelGoalConfig _levelGoalConfig;
        [SerializeField] private MonoPool<LevelModifierView> _levelModifiersPool;
        [SerializeField] private TMP_Text _scoreText;
        
        private List<LevelModifierView> _spawnedViews = new();

        private void Awake()
        {
            _levelModifiersPool.Initialize();
        }

        private void OnEnable()
        {
            var goalItemConfig = _levelGoalConfig.GetGoalItem(LevelController.levelIndex);
            var goal = goalItemConfig.GetGoal();
            if (goal.type == LevelGoal.GoalType.Score)
            {
                _scoreText.gameObject.SetActive(true);
                _scoreText.text = "Score: " + ((LevelScoreGoal)goalItemConfig.GetGoal()).target;
                return;
            }
            _scoreText.gameObject.SetActive(false);
            var modifierGoal = ((LevelModifierGoal)goalItemConfig.GetGoal()).goal;

            foreach (var spawnedView in _spawnedViews)
                spawnedView.Pool.Return(spawnedView);
            _spawnedViews.Clear();
            
            foreach (var goalItem in modifierGoal)
            {
                var view = _levelModifiersPool.Get();
                view.Initialize(goalItem.modifier.sprite, goalItem.amount);
                _spawnedViews.Add(view);
            }

        }
    }
}