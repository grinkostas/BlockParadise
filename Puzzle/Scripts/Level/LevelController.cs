 using System.Collections.Generic;
 using DG.Tweening;
using GameCore.CrossScene.Scripts.Saves;
 using GameCore.Puzzle.Scripts.Field.Tiles;
 using GameCore.Puzzle.Scripts.Level.Configs;
 using GameCore.Puzzle.Scripts.Level.Goals;
using GameCore.Puzzle.Scripts.Level.Goals.Config;
using GameCore.Puzzle.Scripts.Score;
using JetBrains.Annotations;
using NaughtyAttributes;
using NepixSignals;
 using NUnit.Framework;
 using StaserSDK.Extentions;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private bool _changeSaves = true;
        [SerializeField] private bool _haveGoal = true;
        [SerializeField]private LevelPatternGenerator _levelPatternGenerator;
        [SerializeField, ShowIf(nameof(_haveGoal))] private LevelGoalConfig _levelGoalConfig;
        [SerializeField, ShowIf(nameof(_haveGoal))] private LevelsCollection _levelsCollection;
        [SerializeField] private float _changeSceneDelay;
        [SerializeField] private bool _startOnAwake = true;
        [SerializeField, ShowIf(nameof(_haveGoal))] private float _generateLevelDelay = 1.0f;
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }
        [Inject, UsedImplicitly] public LevelSceneSwitcher sceneSwitcher { get; }
        

        public enum LevelState
        {
            WaitForInit,
            Started,
            Completed, 
            Failed
        }

        public static int levelIndex => ES3.Load("LevelIndex", 0);
        public int currentLevelIndex => _levelProperty.value;

        public bool isEnded => currentState == LevelState.Failed || currentState == LevelState.Completed;
        public bool isPlaying => currentState == LevelState.Started;
        public LevelState currentState { get; private set; } = LevelState.WaitForInit;

        private static TheSaveProperty<int> _levelProperty = new ("LevelIndex");
        
        private TheSaveProperty<int> _failedAttemptsProperty = new ("FailedAttemptsProperty", 0);

        public int failedAttempts
        {
            get => _failedAttemptsProperty.value;
            set => _failedAttemptsProperty.value = value;
        }
        
        public LevelGoal currentGoal { get; private set; }
        public TheSignal started { get; } = new();
        public TheSignal failed { get; } = new();
        public TheSignal completed { get; } = new();
        public TheSignal ended { get; } = new();

        public TheSignal<LevelConfig> onLevelPatternInitialize { get; } = new();


        private void Awake()
        {
            currentState = LevelState.WaitForInit;
        }

        private void Start()
        {
            if(_startOnAwake)
                StartGame();
        }
        
        public void StartGame()
        {
            if ( _haveGoal)
            {
                var goal = GetGoal();
                SetGoal(goal);
                SpawnBoardStartPattern(goal);
            }
            
            currentState = LevelState.Started;
            started.Dispatch();
        }
        
        private LevelGoalItemConfig GetGoal()
        {
            return _levelGoalConfig.GetGoalItem(currentLevelIndex);
        }
        
        private void SetGoal(LevelGoalItemConfig goalItem)
        {
            currentGoal = goalItem.GetGoal();
            currentGoal.completed.On(CompleteLevel);
        }

        public void LoadLevelPreset(LevelConfig config)
        {
            SpawnPreset(config);
        }
        
        private void SpawnBoardStartPattern(LevelGoalItemConfig goal)
        {
            DOVirtual.DelayedCall(_generateLevelDelay, () =>
            {
                if (TrySpawnPresetPattern(out var tiles) == false)
                    goal.GenerateLevel(_levelPatternGenerator);
                else
                    goal.ApplyGoalForBoard(tiles);
            }).ConfigureWithId(this, gameObject);
        }
        
        private bool TrySpawnPresetPattern(out List<BoardTile> tiles)
        {
            tiles = new();
            if (levelIndex >= _levelsCollection.levels.Count)
                return false;
            
            var level = _levelsCollection.levels[levelIndex];
            tiles = SpawnPreset(level);
            return true;
        }

        private List<BoardTile> SpawnPreset(LevelConfig config)
        {
            onLevelPatternInitialize.Dispatch(config);
            return _levelPatternGenerator.SpawnPattern(config.levelPattern);
        }

        private void OnDestroy()
        {
            if(_haveGoal)
                _levelGoalConfig.CompleteGoals();
        }

        public void Retry()
        {
            DOTween.KillAll();
            DOVirtual.DelayedCall(_changeSceneDelay, () =>
            {
                sceneSwitcher.LoadLevel();
            }).SetLink(gameObject).SetUpdate(false);
        }

        public void NextLevel()
        {
            DOVirtual.DelayedCall(_changeSceneDelay, () =>
            {
                sceneSwitcher.LoadMeta();
            }).SetLink(gameObject);
        }
        
        public bool TryGetModifierGoal(out LevelModifierGoal goal)
        {
            goal = null;
            if (currentGoal == null)
                return false;
            if (currentGoal.type != LevelGoal.GoalType.Modifier)
                return false;
            goal = (LevelModifierGoal)currentGoal;
            return true;
        }
        
        public bool TryGetScoreGoal(out LevelScoreGoal goal)
        {
            goal = null;
            if (currentGoal == null)
                return false;
            if (currentGoal.type != LevelGoal.GoalType.Score)
                return false;
            goal = (LevelScoreGoal)currentGoal;
            return true;
        }
        
        [Button()]
        public void GameOver()
        {
            if(currentState is LevelState.Completed or LevelState.Failed)
                return;
            
            if(_changeSaves) 
            {
                _failedAttemptsProperty.value++;
                LevelCrossSceneData.FailLevel();
            }
            
            
            
            currentState = LevelState.Failed;
            failed.Dispatch();
            ended.Dispatch();
        }

        [Button()]
        private void CompleteLevel()
        {
            if(currentState is LevelState.Completed or LevelState.Failed)
                return;
            if(_changeSaves) 
            {
                _failedAttemptsProperty.value = 0;
                LevelCrossSceneData.CompleteLevel(scoreController.amount);
                _levelProperty.value++;
            }
            currentState = LevelState.Completed;
            completed.Dispatch();
            ended.Dispatch();
        }
    }
}