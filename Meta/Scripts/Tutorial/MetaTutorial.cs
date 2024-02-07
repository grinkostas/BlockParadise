using System;
using System.Collections.Generic;
using GameCore.CreativesScene.UI;
using GameCore.CrossScene.Scripts.Saves;
using NepixSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Meta.Scripts.Tutorial
{
    public class MetaTutorial : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttonsToDisable;
        
        private TheSaveProperty<bool> _tutorialCompleted = new("TutorialFinished", false);

        public bool isCompleted => _tutorialCompleted.value;
        public bool isStarted { get; private set; } = false;
        public TheSignal started { get; } = new();
        public TheSignal ended { get; } = new();
        
        private void Awake()
        {
            if (_tutorialCompleted.value)
                return;
            
            StartTutorial();
        }

        private void StartTutorial()
        {
            isStarted = true;
            started.Dispatch();
            foreach (var button in _buttonsToDisable)
                button.interactable = false;
        }

        public void FinishTutorial()
        {
            ended.Dispatch();
            foreach (var button in _buttonsToDisable)
                button.interactable = true;
            _tutorialCompleted.value = true;
        }
    }
}