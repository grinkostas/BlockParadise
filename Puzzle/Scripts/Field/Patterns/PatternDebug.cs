using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Field.Patterns
{
    public class PatternDebug : MonoBehaviour
    {
        [SerializeField] private TilesPattern _tilesPatternPrefab;

        [Button()]
        private void Test()
        {
            Debug.Log(_tilesPatternPrefab.items.Count);
            _tilesPatternPrefab.GetPattern();
        }
    }
}