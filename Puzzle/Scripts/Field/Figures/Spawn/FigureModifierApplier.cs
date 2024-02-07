using System;
using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures.Spawn
{
    public class FigureModifierApplier : MonoBehaviour
    {
        [SerializeField] private FigureSpawner _figureSpawner;
        [SerializeField] private int _maxDelta = 3;
        
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public GameBoard board { get; }
        
        private Dictionary<string, int> _modifiersOnBoard = new ();
        private List<TileModifier> _cachedModifiers = new();

        private void OnEnable()
        {
            _figureSpawner.spawned.On(OnFigureSpawned);
            board.addedTile.On(OnAddTile);
            board.removedTile.On(OnRemoveTile);
        }

        private void OnDisable()
        {
            _figureSpawner.spawned.Off(OnFigureSpawned);
            board.addedTile.Off(OnAddTile);
            board.removedTile.Off(OnRemoveTile);
        }

        private void OnAddTile(Tiles.TileData tileData)
        {
            var tile = tileData.tile;
            if(tile.hasModifier == false)
                return;
            var modifier = tile.modifier;
            _modifiersOnBoard.TryAdd(modifier.id, 0);
            
            if (_cachedModifiers.Has(x => x.id == modifier.id, out var cachedModifier))
                _cachedModifiers.Remove(cachedModifier);
            
            _modifiersOnBoard[modifier.id]++;
        }

        private void OnRemoveTile(Tiles.TileData tileData)
        {
            var tile = tileData.tile;
            if(tile.hasModifier == false)
                return;
            var modifier = tile.modifier;
            if (_modifiersOnBoard.ContainsKey(modifier.id) == false)
                return;

            _modifiersOnBoard[modifier.id]--;
        }


        private void OnFigureSpawned(Figure figure)
        {
            if (levelController.TryGetModifierGoal(out var goal) == false)
                return;

            var availableModifiers = goal.goal.FindAll(goalData =>
            {
                int reminder = goal.GetGoalReminder(goalData.modifier);
                if (_modifiersOnBoard.TryGetValue(goalData.modifier.id, out int onBoardCount) == false)
                    onBoardCount = 0;
                onBoardCount += _cachedModifiers.FindAll(x => x.id == goalData.modifier.id).Count;
                return reminder > 0 && reminder + _maxDelta > onBoardCount;
            });
            
            if(availableModifiers.Count == 0)
                return;
            
            var modifier = availableModifiers.Random().modifier;
            _cachedModifiers.Add(modifier);
            figure.tiles.Random().SetModifier(modifier);
        }
    }
}