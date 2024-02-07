using GameCore.Puzzle.Scripts.Field.Tiles;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Boosters.Core
{
    public abstract class SelectTileBooster : Booster
    {
        [SerializeField] private bool _selectOnlyField;
        [Inject, UsedImplicitly] public TileSelector tileSelector { get; }

        public TheSignal<BoardTile> usedOnTile { get; } = new();

        protected override void OnClick()
        {
            tileSelector.StartHandlingSelection();
            tileSelector.selectedTile.On(OnSelectedTile);
        }

        private void OnSelectedTile(BoardTile tile)
        {
            if (_selectOnlyField && tile.isEmpty)
                return;
            var coordinates = tileSelector.board.GetCoordinates(tile);
            OnSelectedCoordinates(coordinates);
            usedOnTile.Dispatch(tile);
            tileSelector.selectedTile.Off(OnSelectedTile);
            Use();
        }

        protected abstract void OnSelectedCoordinates(Vector2Int coordinates);
    }
}