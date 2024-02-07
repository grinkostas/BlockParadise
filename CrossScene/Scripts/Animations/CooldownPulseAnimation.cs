using NaughtyAttributes;
using UnityEngine;

namespace GameCore.CrossScene.Scripts.Animations
{
    public class CooldownPulseAnimation : MonoBehaviour
    {
        [SerializeField] private bool _useOwnTransformAsTarget = true;
        [SerializeField, HideIf(nameof(_useOwnTransformAsTarget))] private Transform _target;

        [SerializeField] private float _pulseScaleDelta = 0.1f;
        [SerializeField] private float _decreaseSpeed = 0.5f;
        [SerializeField, Min(1)] private float _maxScale = 1.25f;
        [SerializeField] private float _minDelayBetweenPulse = 0.1f;
        
        private float _scale = 1;
        private float _lastTimeScaleAdd;

        private Transform target => _useOwnTransformAsTarget ? transform : _target;

        public void Pulse()
        {
            if (Time.unscaledTime - _lastTimeScaleAdd > _minDelayBetweenPulse)
            {
                _scale += _pulseScaleDelta;
                _lastTimeScaleAdd = Time.unscaledTime;
            }
        }
    
        private void Update()
        {
            if (_scale <= 1) return;
        
            _scale -= _decreaseSpeed * Time.unscaledDeltaTime;
            _scale = Mathf.Clamp(_scale, 1f, _maxScale);
            target.localScale = Vector3.one * _scale;
        }
        
    }
}