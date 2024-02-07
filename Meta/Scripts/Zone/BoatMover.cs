using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Meta.Scripts.Zone
{
    public class BoatMover : MonoBehaviour
    {
        [SerializeField] private Transform _boat;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _destinationPoint;
        [SerializeField] private float _duration;

        private void OnEnable()
        {
            var sequence = DOTween.Sequence();
            sequence.AppendCallback(() => _boat.position = _startPoint.position);
            sequence.Append(_boat.DOMove(_destinationPoint.position, _duration));
            sequence.Join(_boat.DOLookAt(_destinationPoint.position, _duration));
            sequence.SetLoops(-1, LoopType.Restart);
        }
    }
}