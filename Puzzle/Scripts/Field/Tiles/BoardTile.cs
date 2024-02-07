using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Colors;
using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Field.Tiles
{
    [RequireComponent(typeof(RectTransform))]
    public class BoardTile : MonoBehaviour, IPoolItem<BoardTile>
    {
        [SerializeField] private RectTransform _graphic;
        [SerializeField] private Image _tileImage;
        [SerializeField] private CanvasGroup _tileImageCanvasGroup;
        [SerializeField] private ColorTemplate _defaultColor;
        [Space]
        [SerializeField] private Image _modifierImage;
        [SerializeField] private bool _isEmpty;
        
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

        public CanvasGroup canvasGroup => _tileImageCanvasGroup;
        public bool isEmpty => _isEmpty;
        
        [CanBeNull] private ColorTemplate _overrideColor;
        private ColorTemplate _currentColor;

        public bool hasModifier { get; private set; } = false;
        public TileModifier modifier { get; private set; } = null;
        
        public Image modifierImage => _modifierImage;
        public ColorTemplate defaultColor => _defaultColor;
        public RectTransform graphic => _graphic;

        public IPool<BoardTile> Pool { get; set; }
        public bool IsTook { get; set; }

        public void SetDefaultColor(ColorTemplate colorTemplate)
        {
            _defaultColor = colorTemplate;
            SetDefaultColor();
        }

        public void SetColor(ColorTemplate colorTemplate, bool overrideColors = false)
        {
            DOTween.Kill(this);
            
            if (overrideColors)
                _overrideColor = colorTemplate;
            else
                _currentColor = colorTemplate;

            if (overrideColors == false && _overrideColor != null)
            {
                SetSprite(_overrideColor.blockSprite);
                return;
            }
            
            SetSprite(colorTemplate.blockSprite);
        }

        public void SetHighlightColor(ColorTemplate colorTemplate)
        {
            if(_overrideColor != null)
                return;
            SetSprite(colorTemplate.blockSprite);
        }
        
        private void SetSprite(Sprite sprite)
        {
            _tileImage.sprite = sprite;
        }

        public void SetDefaultColor(bool clearOverride = false)
        {
            if(clearOverride)
                ClearOverrideColor();
            SetSprite(_overrideColor != null ? _overrideColor.blockSprite : defaultColor.blockSprite);
        }

        public void ClearOverrideColor()
        {
            _overrideColor = null;
            if (_currentColor == null)
            {
                SetColor(_defaultColor);
                return;
            }
            SetColor(_currentColor);
        }
        
        public void SetModifier(TileModifier tileModifier)
        {
            hasModifier = true;
            modifier = tileModifier;
            tileModifier.ApplyModifier(this);
            _modifierImage.gameObject.SetActive(true);
        }

        public void RemoveModifier()
        {
            _modifierImage.gameObject.SetActive(false);
            hasModifier = false;
            modifier = null;
        }
        
        public void TakeItem()
        {
            ClearOverrideColor();
        }

        public void ReturnItem()
        {
            RemoveModifier();
        }
    }
}