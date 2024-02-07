using System;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Board;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Tiles
{
    public class TilePlacer : MonoBehaviour
    {
        [SerializeField] private float _tilePlaceDuration = 0.15f;
        [SerializeField] private Vector3 _punch;
        [SerializeField] private float _punchDuration = 0.55f;
        [SerializeField] private int _punchCount = 8;
        [Inject, UsedImplicitly] public GameBoard board { get; }

        private void OnEnable()
        {
            board.addedTile.On(OnTileAdded);
        }

        private void OnDisable()
        {
            board.addedTile.Off(OnTileAdded);
        }

        private void OnTileAdded(TileData tileData)
        {
            var tile = tileData.tile;
            var rectTransform = tile.rect;

            tile.transform.localRotation = Quaternion.identity;

            Vector2 position = board.GetBrickPosition(new Vector2(tileData.x, tileData.y));
            rectTransform.DOPivot(Vector2.one * 0.5f, _tilePlaceDuration);
            rectTransform.DOAnchorMin(Vector2.zero, _tilePlaceDuration);
            rectTransform.DOAnchorMax(Vector2.zero, _tilePlaceDuration);
            rectTransform.DOAnchorPos(position, _tilePlaceDuration);
            tile.graphic.transform.DOPunchScale(_punch, _punchDuration, _punchCount).SetUpdate(false);
        }
    }
}