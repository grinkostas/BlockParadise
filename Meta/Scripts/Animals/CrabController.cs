using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace GameCore.Meta.Scripts.Animals
{
    public class CrabController : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private List<Transform> _path;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _waitTimeToNextPoint;
        [Space] 
        [SerializeField] private float _victorDuration;
        
        private float _previousDistance = 0;
        private float _stopTime;
        
        private int _currentTargetPathItemIndex = 0;
        private const string idleParameter = "Idle";
        private const string walkParameter = "Walk";
        private const string victoryParameter = "Victory";
        
        
        private void Start()
        {
            _agent.SetDestination(_path[0].position);
        }

        private void Update()
        {
            float distance = VectorExtentions.SqrDistance(_agent.transform.position.XZ(), _path[_currentTargetPathItemIndex].position.XZ());
            if (Mathf.Abs(_previousDistance - distance) <= 0.05f)
                _stopTime += Time.deltaTime;
            _previousDistance = distance;
            if (_waitTimeToNextPoint < _stopTime)
            {
                _stopTime = 0;
                NextPoint();
            }

            _animator.SetTrigger(_agent.velocity.sqrMagnitude > 0.25f ? walkParameter : idleParameter);
        }

        private void NextPoint()
        {
            _currentTargetPathItemIndex ++;
            if (_currentTargetPathItemIndex >= _path.Count)
                _currentTargetPathItemIndex = 0;
            _agent.SetDestination(_path[_currentTargetPathItemIndex].position);
        }

        [Button]
        public void Victory()
        {
            if(DOTween.IsTweening(this))
                return;
            float speed = _agent.speed;
            _agent.speed = 0;
            _animator.SetBool(victoryParameter, true);
            DOVirtual.DelayedCall(_victorDuration, () =>
            {
                _animator.SetBool(victoryParameter, false);
                _agent.speed = speed;
            }).SetId(this).SetUpdate(false);
        }
    }
}