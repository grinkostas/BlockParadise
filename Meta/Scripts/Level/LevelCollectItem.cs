using GameCore.CrossScene.Scripts.Animations;
using GameCore.CrossScene.Scripts.Fx;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Puzzle.Scripts.Level;
using Haptic;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Meta.Scripts.Level
{
    public abstract class LevelCollectItem : MonoBehaviour
    {
        [Inject, UsedImplicitly] public CompleteEffect completeEffect { get; }

        [Inject, UsedImplicitly] public FlyAnimation flyAnimation { get; }[InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        [Inject, UsedImplicitly] public ReturnFigureSoundPool returnFigureSound { get; }
        
        public LevelCompleteSaveData completeData => LevelCrossSceneData.completeData;

        public bool isPlaying { get; protected set; } = false;
        
        public TheSignal started { get; } = new();
        public TheSignal ended { get; } = new();

        public abstract bool NeedToShow();
    }
}