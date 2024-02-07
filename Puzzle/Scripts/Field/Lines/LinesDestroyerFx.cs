using System;
using System.Collections.Generic;
using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Puzzle.Scripts.Fx;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Lines
{
    public class LinesDestroyerFx : MonoBehaviour
    {
        [SerializeField] private LinesDestroyer _linesDestroyer;
        [SerializeField] private MonoPool<LineDestroyFx> _fxPool;
        [SerializeField] private Vector3 _rotationDelta;
        [SerializeField] private float _hideDuration;
        [Inject, UsedImplicitly] public GameBoard board { get; }
        
        private void OnEnable()
        {
            _fxPool.Initialize();
            _linesDestroyer.removedLine.On(OnRemovedLine);
            _linesDestroyer.removed.On(RemoveTile);
        }

        private void OnDisable()
        {
            _linesDestroyer.removedLine.Off(OnRemovedLine);
            _linesDestroyer.removed.Off(RemoveTile);
        }

        private void OnRemovedLine(Figure figure, List<Vector2Int> line)
        {
            ShowDestroyParticle(figure, line);
        }

        private void ShowDestroyParticle(Figure figure, List<Vector2Int> line)
        {
            var fx = _fxPool.Get();
            Vector3 startPosition = GetHorizontalPosition(line);
            float rotation = 0;
            if (line[0].x == line[1].x)
            {
                rotation = 90;
                startPosition = GetVerticalPosition(line);
            }

            fx.transform.localPosition = startPosition;
            
            fx.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
            fx.completed.Once(() => fx.Pool.Return(fx));
            fx.Show(figure.color.fxColor);
        }

        private void RemoveTile(BoardTile tile)
        {
            var tileTransform = tile.transform;
            tileTransform.DOLocalRotate(_rotationDelta, _hideDuration);
            tileTransform.DOScale(0, _hideDuration);
            DOVirtual.DelayedCall(_hideDuration, () => tile.Pool.Return(tile)).SetLink(gameObject);
        }

        private Vector2 GetHorizontalPosition(List<Vector2Int> line)
        {
            var position = board.background[line[0].x, line[0].y].transform.localPosition;
            position.Scale(Vector2.up);
            return position; 
        }

        private Vector2 GetVerticalPosition(List<Vector2Int> line)
        {
            var position = board.background[line[0].x, line[0].y].transform.localPosition;
            position.Scale(Vector2.right);
            return position;
        }
    }
}