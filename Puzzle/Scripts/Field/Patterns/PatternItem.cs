using System;
using GameCore.Puzzle.Scripts.Field.Colors;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Field.Patterns
{
    public class PatternItem : MonoBehaviour
    {
        [SerializeField] private bool _isEmpty = false;
        [Header("Debug")] 
        [SerializeField] private Image _image;
        [SerializeField] private bool _colored;
        [SerializeField, ShowIf(nameof(_colored))]
        private ColorTemplate _colorTemplate;

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

        public bool colored => _colored && _colorTemplate != null;
        public ColorTemplate colorTemplate => _colorTemplate;
        public bool isEmpty => _isEmpty;
        
        
        private void OnValidate()
        {
            
            if (_isEmpty)
                _image.color = Color.gray;
            else if (_colored && _colorTemplate != null)
                _image.color = _colorTemplate.color;
            else
                _image.color = Color.white;

        }
    }
}