using UnityEngine;

namespace GameCore.CrossScene.Scripts.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasAnimateItem : MonoBehaviour, IPoolItem<CanvasAnimateItem>
    {
        
        private RectTransform _rectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }
        
        private CanvasGroup _canvasGroup;
        public CanvasGroup canvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        
        public IPool<CanvasAnimateItem> Pool { get; set; }
        public bool IsTook { get; set; }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
            canvasGroup.alpha = 1.0f;
        }
    }
}