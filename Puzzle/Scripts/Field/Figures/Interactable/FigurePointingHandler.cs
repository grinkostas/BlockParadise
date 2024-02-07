using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Puzzle.Scripts.Fx;
using GameCore.Puzzle.Scripts.Level;
using GameCore.Scripts.Sounds.SoundPools;
using Haptic;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigurePointingHandler : LevelListener
    {
        [SerializeField] private Figure _figure;
        [SerializeField] private Vector3 _defaultSize;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private float _returnDuration;

        [Inject, UsedImplicitly] public PickFigureSoundPool pickFigureSoundPool { get; }
        [Inject, UsedImplicitly] public ReturnFigureSoundPool returnFigureSoundPool { get; }
        
        [InjectOptional, UsedImplicitly] public IHapticService hapticService { get; }
        
        private bool _positionCached = false;
        private Vector2 _cachedPosition;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            _figure.pointerUp.On(OnPointerUp);
            _figure.pointerDown.On(OnPointerDown);
            _figure.transform.localScale = _defaultSize;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _figure.pointerUp.Off(OnPointerUp);
            _figure.pointerDown.Off(OnPointerDown);
        }

        protected override void OnLevelEnded()
        {
            DOTween.Kill(this);
            _figure.pointerUp.Off(OnPointerUp);
            _figure.pointerDown.Off(OnPointerDown);
        }


        private void OnPointerUp(Figure figure)
        {
            DOTween.Kill(this);
            returnFigureSoundPool.PlaySound();
            if(_figure == null)
                return;
            _figure.transform.DOScale(_defaultSize, _zoomDuration).SetId(this);
            _figure.rect.DOAnchorPos(_cachedPosition, _returnDuration).SetId(this);
        }

        private void OnPointerDown(Figure figure)
        {
            if(levelController.currentState != LevelController.LevelState.Started)
                return;
            
            pickFigureSoundPool.PlaySound();
            hapticService?.Selection();
            
            if (_positionCached == false)
            {
                _positionCached = true;
                _cachedPosition = _figure.rect.anchoredPosition;
            }
            DOTween.Kill(this);
            _figure.transform.DOScale(1, _zoomDuration).SetEase(Ease.OutBack).SetId(this);
        }
    }
}