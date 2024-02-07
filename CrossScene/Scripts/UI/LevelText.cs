using System;
using GameCore.Puzzle.Scripts.Level;
using TMPro;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.UI
{
    public class LevelText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void OnEnable()
        {
            _text.text = $"Level {LevelController.levelIndex + 1}";
        }
    }
}