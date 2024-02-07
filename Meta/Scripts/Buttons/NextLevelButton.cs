using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Scenes;
using GameCore.Puzzle.Scripts.Level;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Meta.Scripts.Buttons
{
    public class NextLevelButton : MonoBehaviour
    {
        [SerializeField] private float _changeSceneDelay;
        [SerializeField] private Button _button;

        private AsyncOperation _operation;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void RunNextScene()
        {
            DOTween.KillAll();
            SceneManager.LoadScene(ScenesName.level);
        }
        
        private void OnClick()
        {
            DOVirtual.DelayedCall(_changeSceneDelay, RunNextScene).SetUpdate(false).SetLink(gameObject);
        }
    }
}