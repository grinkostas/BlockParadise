using System;
using GameCore.Puzzle.Scripts.Field.Figures;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score
{
    public class FigurePlacerScore : MonoBehaviour
    {
        [Inject, UsedImplicitly] public FigurePlacer figurePlacer { get; }
        [Inject, UsedImplicitly] public ScoreController scoreController { get; }

        private void OnEnable()
        {
            figurePlacer.placed.On(OnFigurePlaced);
        }

        private void OnDisable()
        {
            figurePlacer.placed.Off(OnFigurePlaced);
        }

        private void OnFigurePlaced(Figure figure)
        {
            scoreController.AddScore(figure.tiles.Count);
        }
    }
}