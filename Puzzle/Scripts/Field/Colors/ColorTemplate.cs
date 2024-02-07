using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Colors
{
    [CreateAssetMenu(menuName = "Tiles/Color")]
    public class ColorTemplate : ScriptableObject
    {
        public Sprite blockSprite;
        public Color color;
        public Color fxColor;
        [Range(0, 1)]public float highlightOpacity;
    }
}