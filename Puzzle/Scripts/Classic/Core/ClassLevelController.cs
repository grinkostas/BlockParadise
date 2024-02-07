using System;
using GameCore.CrossScene.Scripts.Saves;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Puzzle.Scripts.Level.Configs;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Classic.Core
{
    public class ClassicLevelController : LevelListener
    {
        [SerializeField] private LevelsCollection _levelsCollection;
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        
        private TheSaveProperty<int> _maxScoreProperty = new("ClassicMaxScore", 0);
        private TheSaveProperty<int> _classicRunsCountProperty = new("ClassicRunsCount", 0);
        public int maxScore => _maxScoreProperty.value;
        public static int playerMaxScore => ES3.Load("ClassicMaxScore", 0);
        
        private void OnDestroy()
        {
            if (scoreController.amount > maxScore)
                _maxScoreProperty.value = scoreController.amount;
        }
        
        protected override void OnLevelStarted()
        {
            if(_classicRunsCountProperty.value < _levelsCollection.levels.Count)
                levelController.LoadLevelPreset(_levelsCollection.levels[_classicRunsCountProperty.value]);
            _classicRunsCountProperty.value++;
        }
    }
}