using GameCore.CrossScene.Scripts.Boosters.Core;
using GameCore.Puzzle.Scripts.Field.Board;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.CrossScene.Scripts.Boosters.Variants
{
    public class HammerBooster : SelectTileBooster
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [Inject, UsedImplicitly] public GameBoard gameBoard { get; }
        
        protected override void OnSelectedCoordinates(Vector2Int coordinates)
        {
            var tile = gameBoard.RemoveTile(coordinates.x, coordinates.y);
            _particleSystem.transform.position = tile.transform.position;
            _particleSystem.Play();
            tile.Pool.Return(tile);
        }
    }
}