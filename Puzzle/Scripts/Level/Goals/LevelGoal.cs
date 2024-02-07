using DG.Tweening;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Goals
{
    public abstract class LevelGoal
    {
        public abstract GoalType type { get; }
        public enum GoalType
        {
            Score,
            Modifier
        }
        
        public TheSignal completed { get; } = new();
        
        protected void Complete()
        {
            completed.Dispatch();
        }

        public abstract float GetProgress();

    }
}