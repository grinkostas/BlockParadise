using System;
using GameCore.Puzzle.Scripts.Field.Lines;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Tiles.Modifiers
{
    public class TileModifiersController : MonoBehaviour
    {
        [Inject, UsedImplicitly] public LinesDestroyer linesDestroyer { get; }
        
        public TheSignal<Vector3, TileModifier> claimed { get; } = new();

        private void OnEnable()
        {
            linesDestroyer.removed.On(OnRemovedTile);
        }

        private void OnDisable()
        {
            linesDestroyer.removed.Off(OnRemovedTile);
        }

        private void OnRemovedTile(BoardTile tile)
        {
            if(tile.hasModifier == false)
                return;
            claimed.Dispatch(tile.transform.position, tile.modifier);
        }
    }
}