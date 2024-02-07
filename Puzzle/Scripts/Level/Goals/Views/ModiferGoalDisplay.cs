using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using GameCore.Puzzle.Scripts.Score.Displays;
using JetBrains.Annotations;
using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals.Views
{
    public class ModiferGoalDisplay : LevelListener
    {
        [SerializeField] private MonoPool<ModiferGoalDisplayView> _viewsPool;
        [SerializeField] private ScoreDisplay _scoreDisplay;

        [Inject, UsedImplicitly] public DiContainer container { get; }

        private LevelModifierGoal _goal;
        private List<ModiferGoalDisplayView> _spawnedViews = new();

        private void Awake()
        {
            _viewsPool.Initialize(container);
        }
        
        protected override void OnLevelStarted()
        {
            foreach (var spawnedView in _spawnedViews)
                spawnedView.Pool.Return(spawnedView);
            _spawnedViews.Clear();
            
            if (levelController.TryGetModifierGoal(out _goal) == false)
            {
                gameObject.SetActive(false);
                if(_scoreDisplay != null)
                    _scoreDisplay.gameObject.SetActive(false);
                return;
            }
            
            if(_scoreDisplay != null)
                _scoreDisplay.gameObject.SetActive(true);
            gameObject.SetActive(true);
            
            for (int i = 0; i < _goal.goal.Count; i++)
            {
                var goalData = _goal.goal[i];
                var completeData = _goal.goalCompleteData[i];
                
                var view = _viewsPool.Get();
                view.Init(_goal, goalData, completeData);
                _spawnedViews.Add(view);
            }
        }

        public bool TryGetView(TileModifier modifier, out ModiferGoalDisplayView view)
        {
            view = _spawnedViews.Find(x => x.modifier.id == modifier.id);
            return view != null;
        }
    }
}