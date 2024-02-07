using System.Collections.Generic;
using System.Linq;
using GameCore.CrossScene.Scripts.Calculations;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Colors;
using GameCore.Puzzle.Scripts.Field.Patterns;
using GameCore.Puzzle.Scripts.Field.Tiles;
using GameCore.Puzzle.Scripts.Level.Configs;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelPatternGenerator : MonoBehaviour
    {
        [SerializeField] private MonoPool<BoardTile> _tilePool;
        [SerializeField] private TemplateCollection _templateCollection;
        [Inject, UsedImplicitly] public GameBoard board { get; }
        [Inject, UsedImplicitly] public LevelController levelController { get; }
        [Inject, UsedImplicitly] public GameBoardData boardData { get; }
        [Inject, UsedImplicitly] public DiContainer container { get; }
        
        [Inject]
        private void OnInject()
        {
            _tilePool.Initialize(container);
        }

        [Button()]
        public void Test()
        {
            GeneratePattern(0, 3, 2);
        }

        public List<BoardTile> GeneratePattern(int levelIndex, int templatesCount, int complexity)
        {
            PatternSnapshot snapshot = GetSnapshot(levelIndex, templatesCount, complexity);
            return SpawnSnapshot(snapshot);
        }

        public PatternSnapshot GetSnapshot(int levelIndex, int templatesCount, int complexity)
        {
            PatternSnapshot snapshot;
            string saveString = $"LevelPattern_{levelIndex}";
            
            if(ES3.KeyExists(saveString) && levelController.failedAttempts < 3)
                snapshot = ES3.Load<PatternSnapshot>(saveString);
            else
            {
                levelController.failedAttempts = 0;
                snapshot = GenerateSnapshot(templatesCount, complexity);
            }

            ES3.Save(saveString, snapshot);
            return snapshot;
        }

        private PatternSnapshot GenerateSnapshot(int templatesCount, int complexity)
        {
            PatternSnapshot snapshot = new PatternSnapshot(board.size.x);
            List<int> rows = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            
            for (int templateI = 0; templateI < templatesCount; templateI++)
            {
                var i = rows.Random();
                var template = _templateCollection.templates
                    .Where(temp => board.size.y > i + temp.GetPattern().GetLength(0) - 1 && temp.complexity <= complexity)
                    .ToList()
                    .Random();
                
                if(template == null)
                    break;

                int[,] templatePattern = template.GetPattern();
                
                Vector2Int templateSize =
                    new Vector2Int(templatePattern.GetLength(0), templatePattern.GetLength(1));

                rows.Remove(i);
                for (int j = 0; j < templateSize.x - 1; j++)
                    rows.Remove(i + j);
                
                
                int spawnPosition = GetSpawnPosition(template);
                var lineItem = new PatternSnapshot.LineItem(_templateCollection.templates.IndexOf(template), spawnPosition);
                
                var rotation = GetTemplateRotate(template);
                lineItem.mirroredVertical = rotation.vertical;
                lineItem.mirroredHorizontal = rotation.horizontal;
                
                snapshot.AddItem(i, lineItem);
            }

            return snapshot;
        }
        
        
        private List<BoardTile> SpawnSnapshot(PatternSnapshot snapshot)
        {
            List<BoardTile> generatedTiles = new();
            for (int i = 0; i < snapshot.items.Length; i++)
            {
                var lineItem = snapshot.items[i];
                if(lineItem.isEmpty)
                    continue;
                
                var template = _templateCollection.templates[lineItem.templateIndex];
                int[,] templatePattern = template.GetPattern();
                
                Vector2Int templateSize =
                    new Vector2Int(templatePattern.GetLength(0), templatePattern.GetLength(1));
                
                var pattern = RotatePattern(templatePattern, lineItem.mirroredHorizontal, lineItem.mirroredVertical);
                
                int spawnPosition = snapshot.items[i].spawnPosition;
                var color = boardData.GetRandomColor();

                generatedTiles.AddRange(SpawnPattern(i, spawnPosition, pattern, color));
                if (template.maxInUnit > 1)
                {
                    pattern = MirrorHorizontal(pattern);
                    generatedTiles.AddRange(SpawnPattern(i, board.size.x - spawnPosition - templateSize.y, pattern, color));
                }

                i += templateSize.x - 1;
            }

            return generatedTiles;
        }

        public List<BoardTile> SpawnPattern(TemplatePattern template)
        {
            return SpawnPattern(0, 0, template.GetPattern(), true,
                (tile, x, y) =>
                {
                    var item = template.Get(x, y);
                    if(item.colored)
                        tile.SetDefaultColor(item.colorTemplate);
                    else
                        tile.SetDefaultColor(boardData.GetRandomColor());
                });
        }
        
        public List<BoardTile> SpawnPattern(int row, int startPosition, int[,] template, ColorTemplate colorTemplate)
        {
            return SpawnPattern(row, startPosition, template, false, 
                (tile, _, __) => tile.SetDefaultColor(colorTemplate));
        }
        
        private List<BoardTile> SpawnPattern(int row, int startPosition, int[,] template, bool invertY = false,
            UnityAction<BoardTile, int, int> onSpawnTile = null)
        {
            List<BoardTile> tiles = new();
            for (int x = 0; x < template.GetLength(0); x++)
            {
                for (int y = 0; y < template.GetLength(1); y++)
                {
                    if(template[x, y] == 0)
                        continue;
                    
                    int boardX = startPosition + y;
                    int boardY = row + x;
                    if (invertY)
                    {
                        boardY = 7 - boardY;
                    }
                    
                    if(boardX >= board.size.x || boardY >= board.size.y)
                        continue;
                    
                    if(board.field[boardX, boardY] != null)
                        continue;
                    
                    var tile = _tilePool.Get();
                    board.AddTile(tile, boardX, boardY, true);
                    tiles.Add(tile);
                    onSpawnTile?.Invoke(tile, x, y);
                }
            }
            return tiles;
        }
        
        private int GetSpawnPosition(TemplatePattern template)
        {
            if (template.maxInUnit == 1)
            {
                return (board.size.x - template.GetPattern().GetLength(1)) / 2;
            }
            int maxIndex = board.size.x / template.maxInUnit;
            int spawnPosition = Random.Range(0, maxIndex - template.GetPattern().GetLength(0));
            return spawnPosition;
        }

        private (bool horizontal, bool vertical) GetTemplateRotate(TemplatePattern template)
        {
            if (template.rotatable == false)
                return (false, false);
            bool horizontal = false;
            bool vertical = false;
            if (Random.Range(0, 2) > 0) horizontal = true;
            if (Random.Range(0, 2) > 0) vertical = true;
            
            return (horizontal, vertical);
        }
        
        private int[,] RotatePattern(int[,] pattern, bool horizontalRotate, bool verticalRotate)
        {
            if (horizontalRotate) pattern = MirrorHorizontal(pattern);
            if (verticalRotate) pattern = MirrorVertical(pattern);
            
            return pattern;
        }

        

        private int[,] MirrorHorizontal(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] mirroredMatrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    mirroredMatrix[i, j] = matrix[i, cols - 1 - j];

            return mirroredMatrix;
        }
        
        private int[,] MirrorVertical(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] mirroredMatrix = new int[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    mirroredMatrix[i, j] = matrix[rows - 1 - i, j];

            return mirroredMatrix;
        }
    }
}