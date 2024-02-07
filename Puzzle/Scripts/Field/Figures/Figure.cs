using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Calculations;
using GameCore.Puzzle.Scripts.Field.Colors;
using GameCore.Puzzle.Scripts.Field.Tiles;
using NaughtyAttributes;
using NepixSignals;
using UnityEngine;
using UnityEngine.EventSystems;
using Color = System.Drawing.Color;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    [RequireComponent(typeof(RectTransform))]
    public class Figure : MonoBehaviour, IPointerUpHandler, IDragHandler, IPointerDownHandler
    {
        [SerializeField] private MonoPool<BoardTile> _tilePool;
        [SerializeField] private RectTransform _grapghic;
        [SerializeField] private float _pickOffset = 50;
        [SerializeField] private float _pickDuration;
        
        private Vector2 _targetPosition;
        private Vector2 _currentOffset;

        public int size { get; private set; } = 0;
        
        public ColorTemplate color { get; private set; }

        public List<BoardTile> tiles { get; private set; } = new();

        private List<object> _interactBlockers = new();
        public bool interactable => _interactBlockers.Count == 0;
        
        private RectTransform _rectCached;
        public RectTransform rect
        {
            get
            {
                if (_rectCached == null)
                    _rectCached = GetComponent<RectTransform>();
                return _rectCached;
            }
        }

        public int[,] figureCoordinates;
        private int _rotation;

        public bool placed { get; private set; } = false;

        public TheSignal<Figure> pointerUp { get; } = new();
        public TheSignal<Figure> pointerDown { get; } = new();
        public TheSignal<Figure, PointerEventData> pointerDrag { get; } = new();

        private void Awake()
        {
            color = _tilePool.Prefab.defaultColor;
            _tilePool.Initialize();
        }

        private void OnDestroy()
        {
            pointerUp.OffAll();
            pointerDown.OffAll();
            pointerDrag.OffAll();
        }

        public void ResetFigure()
        {
            placed = false;
            figureCoordinates = null;
            _rotation = 0;
            size = 0;
            foreach (var tile in tiles)
            {
                tile.Pool.Return(tile);
            }
            tiles.Clear();
        }
        
        public void InitFigure(int[,] figure, int rotation)
        {
            placed = false;
            figureCoordinates = figure.RotateMatrix(rotation);
            _rotation = rotation;
            size = 0;
            foreach (var figureCoord in figureCoordinates)
                size += figureCoord;
            int rows = figureCoordinates.GetLength(0);
            int columns = figureCoordinates.GetLength(1);

            float centerRow = (rows )/ 2f;
            float centerCol = (columns ) / 2f;

            var tempTile = _tilePool.pool[0];
            var tileRect = tempTile.rect.rect;
            Vector2 tileSize = new Vector2
            {
                x = tileRect.width,
                y = tileRect.height,
            };
            
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (figureCoordinates[j, i] == 0)
                        continue;
                    Vector2 coords = new Vector2(i-centerCol, -(j-centerRow));
                    Vector2 brickPosition = Vector2.Scale(coords, tileSize);
                    BoardTile tile = _tilePool.Get();
                    tile.rect.pivot = new Vector2(0, 1);
                    tiles.Add(tile);
                    tile.rect.anchoredPosition = brickPosition;
                }
            }
            SetColor(color);
        }
        
        
        public void SetColor(ColorTemplate colorTemplate)
        {
            color = colorTemplate;
            foreach (var tile in tiles)
            {
                tile.SetDefaultColor(colorTemplate);
            }
        }
        
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!interactable) return;
            
            pointerUp.Dispatch(this);
            DOTween.Kill(this);
            _grapghic.DOAnchorPos(Vector2.zero, _pickDuration).SetEase(Ease.InBack).SetId(this);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!interactable) return;
            
            pointerDown.Dispatch(this);
            DOTween.Kill(this);

            _grapghic.DOLocalMove(Vector3.up * _pickOffset, _pickDuration).SetEase(Ease.OutBack).SetId(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (tiles.Count == 0 || !interactable) return;

            pointerDrag.Dispatch(this, eventData);
        }
        

        public void Place()
        {
            placed = true;
        }

        [Button()]
        public void ShowPattern()
        {
            Debug.Log(_rotation);
            DebugMatrix(figureCoordinates);
        }

        private void DebugMatrix(int[,] figure)
        {
            for (int i = 0; i < figure.GetLength(0); i++)
            {
                string row = "";
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    row += $"{figure[i, j]}";
                }
                Debug.Log(row);
            }
        }
        
        

        
    }
}