using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Field.Patterns
{
    public class TilesPattern : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayout;

        private List<PatternItem> _itemsCached = new();
        public List<PatternItem> items
        {
            get
            {
                if (_itemsCached.Count == 0)
                {
                    Transform itemsParent = _gridLayout.transform;
                    for (int i = 0; i < itemsParent.childCount; i++)
                    {
                        _itemsCached.Add(itemsParent.GetChild(i).GetComponent<PatternItem>());
                    }
                }

                return _itemsCached;
            }
        }

        public PatternItem Get(int x, int y)
        {
            return items[x * _gridLayout.constraintCount + y];
        }
        
        public int[,] GetPattern()
        {
            int columnsCount = _gridLayout.constraintCount;
            int rows = Mathf.CeilToInt((float)items.Count / columnsCount);
            int[,] pattern = new int[rows, columnsCount];
            
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columnsCount; y++)
                {
                    pattern[x, y] = 0;
                    int index = x * columnsCount + y;
                    if (index < items.Count && items[index].isEmpty == false)
                        pattern[x, y] = 1;
                }
            }


            return pattern;
        }
    }
}