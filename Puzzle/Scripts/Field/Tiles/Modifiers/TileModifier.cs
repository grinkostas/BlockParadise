using GameCore.Puzzle.Scripts.Field.Colors;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Tiles.Modifiers
{
    [CreateAssetMenu(menuName = "Tiles/Modifier")]
    public class TileModifier : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private Sprite _modifierSprite;
        [SerializeField] private Sprite _tileSprite;
        [SerializeField] private ColorTemplate _modiferTileColor;
        [SerializeField] private Color _modifierColor;

        public string id => _id; 
        public Sprite sprite => _modifierSprite;
        public Sprite tileSpite => _tileSprite;
        public Color modifierColor => _modifierColor;

        public void ApplyModifier(BoardTile tile)
        {
            tile.SetColor(_modiferTileColor, true);
            tile.modifierImage.sprite = tileSpite;
        }
    }
}