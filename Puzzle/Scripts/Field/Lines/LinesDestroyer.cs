using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Scripts.Sounds.SoundPools;
using Haptic;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Lines
{
    public class LinesDestroyer : MonoBehaviour
    {
        [SerializeField] private FigurePlacer _figurePlacer;
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [Inject, UsedImplicitly] public MatchChecker matchChecker { get; }
        [Inject, UsedImplicitly] public LineDestroySoundPool lineDestroySound { get; }
        
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }

        public TheSignal<BoardTile> removed = new();
        public TheSignal<Figure, List<Vector2Int>> removedLine = new();
        public TheSignal<int> removedLines { get; } = new();

        private void OnEnable()
        {
            _figurePlacer.placed.On(OnPlaced);
        }

        private void OnDisable()
        {
            _figurePlacer.placed.Off(OnPlaced);
        }

        private void OnPlaced(Figure figure)
        {
            var completedLines = matchChecker.GetCompletedLines(out int linesCount);
            removedLines.Dispatch(linesCount);
            
            if(completedLines.Count == 0)
                return;
            
            hapticService?.Selection();
            foreach (var lineCoords in completedLines)
            {
                removedLine.Dispatch(figure, lineCoords);
                foreach (var tileCoords in lineCoords)
                {
                    if(board.TryRemoveTile(tileCoords.x, tileCoords.y, out var tile) == false)
                        continue;
                    tile.SetColor(figure.color);
                    removed.Dispatch(tile);
                }
                lineDestroySound.PlaySound();
            }
        }
    }
}