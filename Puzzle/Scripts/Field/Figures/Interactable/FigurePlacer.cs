using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Scripts.Sounds.SoundPools;
using Haptic;
using JetBrains.Annotations;
using NepixSignals;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigurePlacer : MonoBehaviour
    {
        [SerializeField] private float _moveDuration;

        private Figure _selectedFigure;
        
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [InjectOptional, UsedImplicitly] public FigureSpawner figureSpawner { get; }
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public PlaceFigureSoundPool placeFigureSoundPool { get; }
        [Inject, UsedImplicitly] public BeepSoundPool beepSoundPool { get; }
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }

        public TheSignal returned { get; } = new();
        public TheSignal<Figure> placed { get; } = new();
        public TheSignal<Figure> placedCompleted { get; } = new();

        private void OnEnable()
        {
            if(figureSpawner != null)
                figureSpawner.spawned.On(OnFigureSpawned);
            levelController.ended.On(OnLevelEnded);
        }

        private void OnDisable()
        {
            if(figureSpawner != null)
                figureSpawner.spawned.Off(OnFigureSpawned);
            levelController.ended.Off(OnLevelEnded);
        }

        private void OnLevelEnded()
        {
            figureSpawner.spawned.Off(OnFigureSpawned);
            foreach (var figure in figureSpawner.spawnedFigures)
                figure.pointerUp.Off(OnFigurePointerUp);
        }
        
        private void OnFigureSpawned(Figure figure)
        {
            figure.pointerUp.On(OnFigurePointerUp);
        }

        private void OnFigurePointerUp(Figure figure)
        {
            if(levelController.currentState != LevelController.LevelState.Started)
                return;
            
            if (board.TryPlaceFigure(figure.tiles, out var coords) == false)
            {
                beepSoundPool.PlaySound();
                hapticService?.LightImpact();
                returned.Dispatch();
                return;
            }
            
            PlaceFigure(coords, figure);
        }

        protected void AddFigureOnBoard(Vector2Int[] coords, Figure figure)
        {
            for (int i = 0; i < figure.tiles.Count; i++)
            {
                var tile = figure.tiles[i];
                board.AddTile(tile, coords[i].x, coords[i].y);
            }
        }
        
        protected virtual void PlaceFigure(Vector2Int[] coords, Figure figure)
        {
            hapticService?.Selection();
            figure.pointerUp.Off(OnFigurePointerUp);
            AddFigureOnBoard(coords, figure);
            figure.Place();
            placed.Dispatch(figure);
            placeFigureSoundPool.PlaySound();
            placedCompleted.Dispatch(figure);
            
            DOVirtual.DelayedCall(_moveDuration, () =>
            {
                Destroy(figure);
            }).SetId(this).SetLink(gameObject);
        }


    }
}