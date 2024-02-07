using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Board;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelStartFx : LevelListener
    {
        [SerializeField] private float _fillDelay;
        [SerializeField] private float _clearFillDelay;
        [Inject, UsedImplicitly] public GameBoardUtilities boardUtilities { get; }
        
        protected override void OnLevelStarted()
        {
            DOVirtual.DelayedCall(_fillDelay, boardUtilities.FillBoardRandomColors).SetUpdate(false).SetLink(gameObject);
            DOVirtual.DelayedCall(_fillDelay+_clearFillDelay, boardUtilities.ClearBackgroundColors).SetLink(gameObject).SetUpdate(false);
        }
    }
}