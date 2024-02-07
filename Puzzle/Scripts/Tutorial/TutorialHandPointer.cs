using System;
using DG.Tweening;
using GameCore.CreativesScene.UI;
using GameCore.Puzzle.Scripts.Field.Figures;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Tutorial
{
    public class TutorialHandPointer : MonoBehaviour
    {
        [SerializeField] private Tutorial _tutorial;
        [SerializeField] private FigurePlacer _figurePlacer;
        [SerializeField] private HandPointer _handPointer;
        [SerializeField] private RectTransform _startPoint;
        [SerializeField] private RectTransform _destinationPoint;
        [SerializeField] private float _moveDuration;
        [SerializeField] private float _returnDuration;
        
        private void OnEnable()
        {
            _tutorial.selectedFigure.On(StopPointing);
            _figurePlacer.returned.On(StartPointing);
            _tutorial.startedStep.On(StartPointing);
            _tutorial.completedStep.On(StopPointing);
        }

        private void OnDisable()
        {
            _tutorial.selectedFigure.Off(StopPointing);
            _figurePlacer.returned.Off(StartPointing);
            _tutorial.startedStep.Off(StartPointing);
            _tutorial.completedStep.Off(StopPointing);
        }

        private void StartPointing()
        {
            _handPointer.gameObject.SetActive(true);
            if (DOTween.IsTweening(this))
                return;
            var startPosition = _startPoint.position;
            _handPointer.transform.position = startPosition;
            var sequence = DOTween.Sequence();
            sequence.Append(_handPointer.Press());
            sequence.Append(_handPointer.transform.DOMove(_destinationPoint.position, _moveDuration));
            sequence.Append(_handPointer.Repel());
            sequence.Join(_handPointer.transform.DOMove(startPosition, _returnDuration));
            sequence.SetId(this).SetLink(gameObject).SetLoops(-1);
        }

        private void StopPointing()
        {
            DOTween.Kill(this);
            _handPointer.gameObject.SetActive(false);
        }

    }
}