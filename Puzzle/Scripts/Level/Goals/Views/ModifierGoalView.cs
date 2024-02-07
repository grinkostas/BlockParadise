using GameCore.Puzzle.Scripts.Field.Tiles.Modifiers;
using GameCore.Puzzle.Scripts.Fx;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Level.Goals.Fx
{
    public class ModifierGoalView : MonoBehaviour, IPoolItem<ModifierGoalView>
    {
        [SerializeField] private Image _modifierImage;
        [SerializeField] private ColoredParticle _coloredParticle;

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
        public IPool<ModifierGoalView> Pool { get; set; }
        public bool IsTook { get; set; }

        public void Init(TileModifier modifier)
        {
            _modifierImage.sprite = modifier.sprite;
            _coloredParticle.SetColor(modifier.modifierColor);
        }
        
        public void TakeItem()
        {
        }

        public void ReturnItem()
        {
        }
    }
}