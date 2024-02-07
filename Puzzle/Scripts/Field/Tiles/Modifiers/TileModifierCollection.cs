using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Tiles.Modifiers
{
    [CreateAssetMenu(menuName = "Collections/Modifiers")]
    public class TileModifierCollection : ScriptableObject
    {
        public List<TileModifier> modifiers;

        public TileModifier Get(string id)
        {
            return modifiers.Find(x => x.id == id);
        }
    }
}