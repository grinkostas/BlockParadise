using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Bindings
{
    [RequireComponent(typeof(Canvas))]
    public class MainCanvas : MonoBehaviour
    {
        private Canvas _canvasCached;
        public Canvas canvas
        {
            get
            {
                if (_canvasCached == null)
                    _canvasCached = GetComponent<Canvas>();
                return _canvasCached;
            }
        }
        
        private CanvasScaler _canvasScaler;
        public CanvasScaler canvasScaler
        {
            get
            {
                if (_canvasScaler == null)
                    _canvasScaler = GetComponent<CanvasScaler>();
                return _canvasScaler;
            }
        }
        
        private GraphicRaycaster _canvasRaycaster;
        public GraphicRaycaster raycaster
        {
            get
            {
                if (_canvasRaycaster == null)
                    _canvasRaycaster = GetComponent<GraphicRaycaster>();
                return _canvasRaycaster;
            }
        }
        
        private RectTransform _rectTransformCached;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransformCached == null)
                    _rectTransformCached = GetComponent<RectTransform>();
                return _rectTransformCached;
            }
        }
        
        public float canvasScale => Screen.width / canvasScaler.referenceResolution.x;

    }
}