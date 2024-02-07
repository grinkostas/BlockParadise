using System.Collections.Generic;
using DG.Tweening;
using NepixSignals;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Fx
{
    public class LineDestroyFx : MonoBehaviour, IPoolItem<LineDestroyFx>
    {
        [Header("Images")] 
        [SerializeField] private List<Image> _images;
        [Header("Show")] 
        [SerializeField] private Vector3 _startZoom;
        [SerializeField] private Transform _background;
        [SerializeField] private float _zoomDuration;
        [SerializeField] private Ease _zoomEase;
        [Header("Effect")] 
        [SerializeField] private CanvasGroup _backgroundCanvasGroup;
        [SerializeField] private CanvasGroup _effectCanvasGroup;
        [SerializeField] private CanvasGroup _effectBackgroundCanvasGroup;
        [SerializeField] private CanvasGroup _cubesCanvasGroup;
        [SerializeField] private float _fadeDuration;
        [SerializeField] private float _cubesFadeDuration;
        [Header("Cubes")] 
        [SerializeField] private RectTransform _cubesRect;
        [SerializeField] private MonoPool<CubeFx> _cubesPool;
        [SerializeField] private int _cubesCount;
        [SerializeField] private Vector2 _zoomRange;
        [SerializeField] private Vector2 _opacityRange;
        [Header("CubeMoves")] 
        [SerializeField] private Vector3 _moveDelta;
        [SerializeField] private float _moveDuration;
        [Header("Hide")] 
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _hideDelay;
        [SerializeField] private float _hideDuration;

        private List<CubeFx> _spawnedCubes = new();
        
        public IPool<LineDestroyFx> Pool { get; set; }
        public bool IsTook { get; set; }

        public TheSignal completed { get; } = new();
        
        public void Show(Color color)
        {
           Prepare(color);
            var sequence = DOTween.Sequence();
            sequence.SetId(this);
            sequence.Append(_background.transform.DOScaleX(1, _zoomDuration).SetEase(_zoomEase));
            sequence.AppendCallback(() =>ShowEffect(color));
            sequence.AppendInterval(_hideDelay);
            sequence.Append(_effectBackgroundCanvasGroup.DOFade(0, _hideDuration));
            sequence.Append(_cubesCanvasGroup.DOFade(0, _cubesFadeDuration));
            sequence.AppendCallback(completed.Dispatch);
            sequence.SetLink(gameObject).SetUpdate(false);
        }

        private void Prepare(Color color)
        {
            _effectCanvasGroup.gameObject.SetActive(false);
            _backgroundCanvasGroup.alpha = 1;
            _effectBackgroundCanvasGroup.alpha = 1;
            _cubesCanvasGroup.alpha = 1;
            
            foreach (var image in _images)
                image.color = color;
            
            _background.transform.localScale = _startZoom;
        }

        private void ShowEffect(Color color)
        {
            _effectCanvasGroup.alpha = 0;
            _effectCanvasGroup.gameObject.SetActive(true);
            
            var sequence = DOTween.Sequence();
            sequence.Join(_background.transform.DOScaleY(1, _zoomDuration).SetEase(_zoomEase));
            sequence.Join(_effectCanvasGroup.DOFade(1, _fadeDuration).SetEase(_zoomEase));
            sequence.Join( _backgroundCanvasGroup.DOFade(0, _fadeDuration).SetEase(_zoomEase));
            sequence.SetLink(gameObject).SetId(this).SetUpdate(false);
            SpawnCubes(color);
        }

        private void SpawnCubes(Color color)
        {
            for (int i = 0; i < _cubesCount; i++)
                SpawnCube(color, i % 2 == 0 ? 1 : -1);
        }

        private void SpawnCube(Color color, int direction = 1)
        {
            var cube = _cubesPool.Get();
            _spawnedCubes.Add(cube);
            var rect = _cubesRect.rect;
            float startX = Random.Range(direction * rect.width / 2, 0);
            float startY = Random.Range(-rect.height / 2, rect.height / 2);
            Vector3 spawnPosition = new Vector3(startX, startY, 0);
            cube.transform.localPosition = spawnPosition;
            cube.transform.localScale = _zoomRange.Random() * Vector3.one;
            cube.SetColor(color);
            cube.canvasGroup.alpha = _opacityRange.Random();
            cube.transform.DOLocalMove(spawnPosition + direction * _moveDelta, _moveDuration)
                .SetId(this).SetLink(gameObject).SetUpdate(false);
        }
        
        public void TakeItem()
        {
            _canvasGroup.alpha = 1;
        }

        public void ReturnItem()
        {
            foreach (var cube in _spawnedCubes)
                cube.Pool.Return(cube);

            DOTween.Kill(this);
            _spawnedCubes.Clear();
        }
    }
}