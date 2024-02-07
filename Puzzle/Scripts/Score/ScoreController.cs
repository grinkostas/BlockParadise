using System;
using GameCore.Puzzle.Scripts.Field.Figures;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score
{
    public class ScoreController
    {
        public int amount { get; private set; } = 0;

        public TheSignal changed { get; } = new();

        public ScoreController()
        {
            amount = 0;
        }
        
        public void AddScore(int scoreToAdd)
        {
            if(scoreToAdd <= 0)
                return;
            
            amount += scoreToAdd;
            changed.Dispatch();
        }
    }
}