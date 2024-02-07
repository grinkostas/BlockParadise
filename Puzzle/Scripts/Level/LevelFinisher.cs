using DG.Tweening;
using GameCore.CrossScene.Scripts.Calculations;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Field.Tiles;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelFinisher : MonoBehaviour
    {
        [SerializeField] private float _checkDelay;
        [SerializeField] private float _endGameDelay = 1.0f;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private bool _deepDebug;
        [SerializeField] private int _figureIndex;
        [SerializeField] private Vector2Int _coordinatesToCheck;
        
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [InjectOptional, UsedImplicitly] public FigureSpawner figureSpawner { get; }

        private bool _isFinished = false;
        
        private void OnEnable()
        {
            board.addedTile.On(OnFieldChanged);
            board.removedTile.On(OnFieldChanged);
            if(figureSpawner != null)
                figureSpawner.spawnedVariations.On(Check);
        }

        private void OnDisable()
        {
            board.addedTile.Off(OnFieldChanged);
            board.removedTile.Off(OnFieldChanged);
            if(figureSpawner != null)
                figureSpawner.spawnedVariations.Off(Check);
        }

        private void OnFieldChanged(TileData data) => Check();
        

        [Button()]
        private void Check()
        {
            if(_isFinished)
                return;
            if (figureSpawner.spawnedFigures.Count <= 0)
                return;
            DOTween.Kill(this);
            DOTween.Kill(_endGameDelay);
            DOVirtual.DelayedCall(_checkDelay, () =>
            {
                foreach (var figure in figureSpawner.spawnedFigures)
                    if (board.CanPlaceFigureOnBoard(figure) && _isFinished == false)
                    {
                        DOTween.Kill(_endGameDelay);
                        return;
                    }
                NoSpaceLeft();
            }).SetLink(gameObject);
        }

        [Button()]
        private void DebugCheck()
        {
            var result = board.TryPlaceFigure(figureSpawner.spawnedFigures[_figureIndex], _coordinatesToCheck, out _, true);
            Debug.Log($"Can Place {result}");
        }
        
        private void NoSpaceLeft()
        {
            DOTween.Kill(_endGameDelay);
            DOVirtual.DelayedCall(_endGameDelay, () =>
            {
                DOTween.Kill(_endGameDelay);
                DOTween.Kill(this);
                _isFinished = true;
                levelController.GameOver();
            }).SetId(_endGameDelay).SetLink(gameObject);
            
        }
    }
}