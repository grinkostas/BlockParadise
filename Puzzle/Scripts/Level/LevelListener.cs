using System;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelListener : MonoBehaviour
    {
        [Inject, UsedImplicitly] public LevelController levelController { get; }

        protected virtual void OnEnable()
        {
            if(levelController.currentState == LevelController.LevelState.Started)
                OnLevelStarted();
            else
                levelController.started.Once(OnLevelStarted);
            
            if(levelController.currentState == LevelController.LevelState.Failed)
                OnLevelFailed();
            else
                levelController.failed.Once(OnLevelFailed);
            
            if(levelController.currentState == LevelController.LevelState.Completed)
                OnLevelCompleted();
            else
                levelController.completed.Once(OnLevelCompleted);

            if (levelController.isEnded)
                OnLevelEnded();
            else
                levelController.ended.Once(OnLevelEnded);
        }

        protected virtual void OnDisable()
        {
            levelController.started.Off(OnLevelStarted);
            levelController.failed.Off(OnLevelFailed);
            levelController.completed.Off(OnLevelCompleted);
        }

        protected virtual void OnLevelStarted()
        {
        }

        protected virtual void OnLevelFailed()
        {
        }
        
        protected virtual void OnLevelCompleted()
        {
        }

        protected virtual void OnLevelEnded()
        {
        }
    }
}