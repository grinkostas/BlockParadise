using System;
using GameCore.Puzzle.Scripts.Bindings;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Figures
{
    public class FigureMover : MonoBehaviour
    {
        [SerializeField] private Figure _figure;

        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public MainCanvas mainCanvas { get; }
        
        private void OnEnable()
        {
            _figure.pointerDrag.On(OnPointerDrag);
        }

        private void OnDisable()
        {
            _figure.pointerDrag.Off(OnPointerDrag);
        }

        private void OnPointerDrag(Figure figure, PointerEventData eventData)
        {
            if(levelController.currentState != LevelController.LevelState.Started)
                return;
            figure.rect.anchoredPosition += eventData.delta / mainCanvas.canvasScale;
        }
        
        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPoint)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _figure.transform.parent.GetComponent<RectTransform>(),
                screenPoint,
                Camera.main, 
                out var position
            );

            float yMin = float.MaxValue;

            foreach (var tile in _figure.tiles)
            {
                Vector2 tilePosition = transform.localRotation * tile.rect.anchoredPosition;
                yMin = Mathf.Min(yMin, tilePosition.y);
            }

            position.y -= yMin;

            return position;
        }
    }
}