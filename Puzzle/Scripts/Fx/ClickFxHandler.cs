using System;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.Puzzle.Scripts.Bindings;
using GameCore.Scripts.Sounds;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameCore.Puzzle.Scripts.Fx
{
    public class ClickFxHandler : MonoBehaviour
    {
        [SerializeField] private MonoPool<ClickFx> _clickFxPool;
        [SerializeField] private SoundPool _soundPool;

        [Inject, UsedImplicitly] public MainCamera mainCamera { get; }
        [Inject, UsedImplicitly] public MainCanvas mainCanvas { get; }

        private bool _injected = false;
        private void Awake()
        {
            _clickFxPool.Initialize();
        }

        [Inject]
        public void OnInject()
        {
            _injected = true;
        }
        
        public void ShowClick()
        {
            if(_injected == false)
                return;
            _soundPool.PlaySound();
            var fx = _clickFxPool.Get();
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.rectTransform,Input.mousePosition, mainCamera.cam,out var position);
            fx.rectTransform.anchoredPosition = position;
            
            fx.showComplete.Once(() => fx.Pool.Return(fx));
            fx.Show();
        }
    }
}