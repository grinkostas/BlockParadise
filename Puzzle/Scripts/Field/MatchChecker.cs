using System.Collections.Generic;
using System.Linq;
using GameCore.Puzzle.Scripts.Field.Board;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field
{
    public class MatchChecker : MonoBehaviour
    {
        [Inject, UsedImplicitly] public GameBoard board { get; }

        public List<List<Vector2Int>> GetCompletedLines(Vector2Int[] additionalTiles = null)
        {
            return GetCompletedLines(out _, additionalTiles);
        }
        
        public List<List<Vector2Int>> GetCompletedLines(out int linesCount, Vector2Int[] additionalTiles = null)
        {
            linesCount = 0;
            var completedLines = new  List<List<Vector2Int>>();

            for (int x = 0; x < board.size.x; x++)
            {
                bool line = true;

                for (int y = 0; y < board.size.y; y++)
                {
                    if (board.field[x, y] != null)
                        continue;

                    int cachedX = x;
                    int cachedY = y;
                    if(additionalTiles != null && additionalTiles.Has(tile => tile.x == cachedX && tile.y == cachedY))
                        continue;
                    
                    line = false;
                }

                if (!line)
                    continue;
                List<Vector2Int> lineCoordinates = new();
                linesCount++;
                for (int y = 0; y < board.size.y; y++)
                    lineCoordinates.Add(new Vector2Int(x, y));
                completedLines.Add(lineCoordinates);
            }

            for (int y = 0; y < board.size.y; y++)
            {
                bool line = true;

                for (int x = 0; x < board.size.x; x++)
                {
                    if (board.field[x, y] != null)
                        continue;

                    if(additionalTiles != null && additionalTiles.Has(tile => tile.x == x && tile.y == y))
                        continue;
                    
                    line = false;
                    break;
                }

                if (!line)
                    continue;

                List<Vector2Int> lineCoordinates = new();
                linesCount++;
                for (int x = 0; x < board.size.y; x++)
                    lineCoordinates.Add(new Vector2Int(x, y));
                completedLines.Add(lineCoordinates);
            }

            return completedLines.Distinct().ToList();
        }

    }
}