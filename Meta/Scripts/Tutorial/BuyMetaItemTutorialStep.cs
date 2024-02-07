using System;
using System.Collections.Generic;
using GameCore.CreativesScene.UI;
using GameCore.CrossScene.Scripts.UI.Popups;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Tutorial
{
    public class BuyMetaItemTutorialStep : MonoBehaviour
    {
        [SerializeField] private Popup _popup;
        [SerializeField] private List<GameObject> _objects;
        [SerializeField] private HandPointer _handPointer;
        [SerializeField] private Button _targetButton;
        [SerializeField] private HandPointer _nextLevelPointer;
        
        [Inject, UsedImplicitly] public MetaTutorial metaTutorial { get; }

        
        private void OnEnable()
        {
            Prepare();
            if (metaTutorial.isStarted)
            {
                OnTutorialStarted();
                return;
            }
            metaTutorial.started.Once(OnTutorialStarted);
        }

        private void Prepare()
        {
            _nextLevelPointer.gameObject.SetActive(false);
            _handPointer.gameObject.SetActive(false);
            foreach (var objectToChangeActive in _objects)
                objectToChangeActive.SetActive(true);
        }
        
        private void OnTutorialStarted()
        {
            foreach (var objectToChangeActive in _objects)
                objectToChangeActive.SetActive(false);
            _handPointer.gameObject.SetActive(true);
            _handPointer.StartClicking();
            _targetButton.onClick.AddListener(OnStepEnded);
            _popup.hideStarted.Once(() =>
            {
                _nextLevelPointer.gameObject.SetActive(true);
                _nextLevelPointer.StartClicking();
            });
        }

        private void OnStepEnded()
        {
            _handPointer.ResetToDefault();
            _targetButton.onClick.RemoveListener(OnStepEnded);
            Prepare();
            metaTutorial.FinishTutorial();
        }
    }
}