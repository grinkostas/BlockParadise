using System;
using DG.Tweening;
using GameCore.CrossScene.Scripts.UI.Popups;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace GameCore.Puzzle.Scripts.Game
{
    public class GameOverPopup : TypeInjectablePopup
    {
        [SerializeField] private AlphaView _alphaView;
        [Header("Restart Button")]
        [SerializeField] private Button _restartButton;
        [SerializeField] private float _buttonShowDelay;
        [SerializeField] private float _buttonZoomDuration;
        [Header("Text")] 
        [SerializeField] private TMP_Text _title;
        [SerializeField] private Vector3 _startPosition;
        [SerializeField] private float _titleZoomDuration;
        [SerializeField] private float _titleZoomDelay;
        [SerializeField] private Vector3 _moveDelta;
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _startMoveDelay;
        protected override Type popupType => typeof(GameOverPopup);

        [NaughtyAttributes.Button]
        public override void Show()
        {
            DOTween.Kill(this);
            _alphaView.Show();
            _restartButton.transform.localScale = Vector3.zero;
            _restartButton.transform.DOScale(1, _buttonZoomDuration).SetEase(Ease.OutBack).SetDelay(_buttonShowDelay).SetId(this);

            _title.transform.localPosition = _startPosition;
            _title.transform.localScale = Vector3.zero;
            
            _title.transform.DOScale(1, _titleZoomDuration).SetEase(Ease.OutBack).SetDelay(_titleZoomDelay);
            _title.transform.DOLocalMove(_startPosition + _moveDelta, _moveDuration).SetEase(Ease.OutBack).SetDelay(_startMoveDelay);
        }

        public override void Hide()
        {
            _alphaView.Hide();
        }
    }
}