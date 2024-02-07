using System;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level.Views
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [Inject, UsedImplicitly] public LevelController levelController { get; }

        private void Awake()
        {
            _levelText.text = $"Level {levelController.currentLevelIndex+1}";
        }
    }
}