using System;
using GameCore.Puzzle.Scripts.Field;
using GameCore.Puzzle.Scripts.Field.Lines;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score
{
    public class LinesDestroyScore : MonoBehaviour
    {
        [SerializeField] private int _lineScore;

        [Inject, UsedImplicitly] public LinesDestroyer linesDestroyer { get; }
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }

        public TheSignal<LineDestroyData> removedLines { get; } = new();

        private int _inRow = 0;
        
        private void OnEnable()
        {
            linesDestroyer.removedLines.On(OnRemovedLines);
        }

        private void OnDisable()
        {
            linesDestroyer.removedLines.Off(OnRemovedLines);
        }

        private void OnRemovedLines(int removedLinesCount)
        {
            if (removedLinesCount == 0)
            {
                _inRow = 0;
                return;
            }

            _inRow++;
            int score = _lineScore * _inRow * removedLinesCount;
            scoreController.AddScore(score);
            removedLines.Dispatch(new LineDestroyData(_inRow, removedLinesCount, score));
        }
    }
}