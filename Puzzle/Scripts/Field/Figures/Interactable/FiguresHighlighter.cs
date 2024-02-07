using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FiguresHighlighter : LevelListener
    {
        [SerializeField] protected FigurePlacer _figurePlacer;
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [Inject, UsedImplicitly] public MatchChecker matchChecker { get; }        
        [InjectOptional, UsedImplicitly] public FigureSpawner figureSpawner { get; }
        
        private List<Vector2Int> _coloredCoordinates = new();
        private List<Vector2Int> _overrideFieldTileColorCoordinates = new();
        private List<Vector2Int> _overrideBackgroundTileColorCoordinates = new();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            if(figureSpawner != null) figureSpawner.spawned.On(OnFigureSpawned);
            _figurePlacer.placed.On(OnFigurePlaced);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if(figureSpawner != null) figureSpawner.spawned.Off(OnFigureSpawned);
            _figurePlacer.placed.Off(OnFigurePlaced);
        }

        protected override void OnLevelEnded()
        {
            ClearHighlight();
        }

        private void OnFigurePlaced(Figure figure)
        {
            ClearHighlight();
        }

        private void OnFigureSpawned(Figure figure)
        {
            figure.pointerDrag.On(OnFigureDrag);
        }

        private void OnFigureDrag(Figure figure, PointerEventData eventData)
        {
            if(levelController.isEnded)
                return;
            
            ClearHighlight();
            
            if (board.TryPlaceFigure(figure.tiles, out var coords) == false)
                return;

            var completedLineTiles = matchChecker.GetCompletedLines(coords).SelectMany(x=>x).ToList();
            if (completedLineTiles.Count > 0)
            {
                foreach (var tile in completedLineTiles)
                {
                    if (board.field[tile.x, tile.y] == null)
                    {
                        var bgTile = board.background[tile.x, tile.y];
                        bgTile.SetHighlightColor(figure.color);
                        bgTile.canvasGroup.alpha = figure.color.highlightOpacity;
                        _overrideBackgroundTileColorCoordinates.Add(tile);
                        continue;
                    }

                    _overrideFieldTileColorCoordinates.Add(tile);
                    board.field[tile.x, tile.y].SetColor(figure.color);
                }
            }
            
            var coordToAddHighlight = coords.Except(completedLineTiles).ToList();
            _coloredCoordinates.AddRange(coordToAddHighlight);
            foreach (var coord in coordToAddHighlight)
            {
                var tile = board.background[coord.x, coord.y];
                tile.SetHighlightColor(figure.color);
                tile.canvasGroup.alpha = figure.color.highlightOpacity;
            }

            
        }

        protected void ClearHighlight()
        {
            ClearHighlight(board.background, ref _coloredCoordinates);
            ClearHighlight(board.background, ref _overrideBackgroundTileColorCoordinates);
            ClearHighlight(board.field, ref _overrideFieldTileColorCoordinates);
        }

        private void ClearHighlight(BoardTile[,] source, ref List<Vector2Int> coloredList)
        {
            foreach (var coloredCoordinate in coloredList)
            {
                var tile = source[coloredCoordinate.x, coloredCoordinate.y];
                if(tile == null)
                    continue;
                tile.SetDefaultColor();
            }

            coloredList.Clear();
        }
    }
}