using System.Collections.Generic;
using GameCore.CrossScene.Scripts.Calculations;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Field.Tiles;
using NepixSignals;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Field.Board
{
    public class GameBoard : MonoBehaviour
    {
        [Header("Board")]
        [SerializeField] private Vector2Int _boardSize;
        [SerializeField] private RectTransform _boardTilesParent;
        [SerializeField] private BoardTile _emptyTilePrefab;
        [SerializeField] private BoardTile _tilePrefab;

        
        public BoardTile[,] field { get; private set; } = new BoardTile[8, 8];
        public BoardTile[,] background { get; private set; } = new BoardTile[8, 8];
        

        public Vector2Int size => _boardSize;
        public RectTransform tileParent => _boardTilesParent;

        public TheSignal<TileData> addedTile { get; } = new();
        public TheSignal<TileData> removedTile { get; } = new();

        [Inject]
        private void OnInject()
        {
            InitField();
            SpawnEmptyTiles();
        }

        private void InitField()
        {
            field = new BoardTile[_boardSize.x, _boardSize.y];
            background = new BoardTile[_boardSize.x, _boardSize.y];
        }

        private void SpawnEmptyTiles()
        {
            for (int x = 0; x < _boardSize.x; x++)
            {
                for (int y = 0; y < _boardSize.y; y++)
                {
                    SpawnEmptyTile(new Vector2Int(x, y));
                }
            }
        }


        private BoardTile SpawnEmptyTile(Vector2Int coords)
        {
            var tile = Instantiate(_emptyTilePrefab, _boardTilesParent);

            tile.transform.SetParent(_boardTilesParent, false);
            tile.rect.anchorMin = Vector2.zero;
            tile.rect.anchorMax = Vector2.zero;
            tile.rect.anchoredPosition = GetBrickPosition(new Vector2(coords.x, coords.y));

            background[coords.x, coords.y] = tile;

            return tile;
        }

        public void AddTile(BoardTile tile, int x, int y, bool skipAnimation = false)
        {
            var rectTransform = tile.rect;
            tile.rect.SetParent(tileParent, true);
            Vector2 position = GetBrickPosition(new Vector2(x, y));
            field[x, y] = tile;
            if (skipAnimation)
            {
                rectTransform.anchorMin = Vector2.zero;
                rectTransform.anchorMax = Vector2.zero;
                rectTransform.anchoredPosition = position;
                return;
            }
            addedTile.Dispatch(new TileData(tile, x, y));
        }

        public bool TryRemoveTile(int x, int y, out BoardTile tile)
        {
            tile = null;
            if(x >= size.x || y >= size.y)
                return false;
            if (field[x, y] == null)
                return false;
            tile = RemoveTile(x, y);
            return true;
        }

        public BoardTile RemoveTile(int x, int y)
        {
            var tile = field[x, y];
            field[x, y] = null;
            removedTile.Dispatch(new TileData(tile, x, y));
            return tile;
        }
        
        
        
        public Vector2 GetBrickPosition(Vector2 coords)
        {
            Vector2 brickSize = GetBrickSize();
            RectTransform brickTransform = _tilePrefab.rect;

            Vector2 brickPosition = Vector2.Scale(coords, brickSize);
            brickPosition += Vector2.Scale(brickSize, brickTransform.pivot);

            return brickPosition;
        }

        public Vector2 GetBrickSize()
        {
            Rect rect = _boardTilesParent.rect;
            Vector2 brickSize = new Vector2
            {
                x = rect.width / _boardSize.x,
                y = rect.height / _boardSize.y
            };

            return brickSize;
        }
        
        public Vector2 GetWorldBrickSize()
        {
            Vector3[] worldCorners = new Vector3[4];
            tileParent.GetWorldCorners(worldCorners);
            Vector2 brickSize = new Vector2
            {
                x = (worldCorners[2].x - worldCorners[0].x) / _boardSize.x,
                y = (worldCorners[2].y - worldCorners[0].y) / _boardSize.y
            };

            return brickSize;
        }

        public bool TryPlaceFigure(List<BoardTile> tiles, out Vector2Int[] coords)
        {
            coords = new Vector2Int[tiles.Count];
            Vector2 minPosition = GetFigureMinPosition(tiles);

            Vector2 pivot = tiles[0].rect.pivot;
            Vector2Int minCoords = TilePositionToCoords(minPosition, pivot);
        
            for (int i = 0; i < tiles.Count; i++)
            {
                Vector2 localCoords = ((Vector2)tiles[i].transform.position - minPosition) / GetWorldBrickSize();
                coords[i] = Vector2Int.RoundToInt(localCoords) + minCoords;

                if (CanUseTile(coords[i]) == false)
                    return false;
            }
        
            return true;
        }

        public bool TryPlaceFigure(Figure figure, Vector2Int coordinates) => TryPlaceFigure(figure, coordinates, out _);

        public bool TryPlaceFigure(int[,] figure, Vector2Int coordinates, out List<Vector2Int> placeCoordinates,
            bool debug = false)
        {
            placeCoordinates = new();

            int figureSizeX = figure.GetLength(0);
            int figureSizeY = figure.GetLength(1);

            var boardMatrix = GetMatrix();
            
            for (int x = 0; x < figureSizeX; x++)
            {
                for (int y = 0; y < figureSizeY; y++)
                {
                    if(figure[x, y] == 0)
                        continue;
                    int newX = coordinates.y + x;
                    int newY = coordinates.x + y;
                    if (newX >= _boardSize.x || newY >= _boardSize.y)
                    {
                        if(debug) Debug.Log($"[Bounds] ({newX}, {newY})");
                        return false;
                    }
                    if(debug) Debug.Log($"[Check] ({newX}, {newY}) - {boardMatrix[newX, newY]}");
                    if (boardMatrix[newX, newY] == 1)
                        return false;
                    
                    placeCoordinates.Add(new Vector2Int(newX, newY));
                }
            }
            return true;
        }
        public bool TryPlaceFigure(Figure figure, Vector2Int coordinates, out List<Vector2Int> placeCoordinates, bool debug = false)
        {
            placeCoordinates = new();
            if (figure.placed)
            {
                if(debug) Debug.Log("AlreadyPlaced");
                return false;
            }

            return TryPlaceFigure(figure.figureCoordinates, coordinates, out placeCoordinates, debug);
        }

        public int[,] GetMatrix()
        {
            int[,] board = new int[size.x, size.y];
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    if (field[i, j] == null)
                    {
                        board[j, i] = 0;
                        continue;
                    }
                    board[j, i] = 1;
                }
            }

            return MatrixExtension.MirrorVertical(board);
        }

        public bool CanUseTile(Vector2Int coords)
        {
            if (coords.x < 0 || coords.y < 0 )
                return false;
                        
            if(coords.x >= size.x || coords.y >= size.y)
                return false;
                
            return field[coords.x, coords.y] == null;
        }
        
        private Vector2 GetFigureMinPosition(List<BoardTile> tiles)
        {
            Vector2 minPosition = tiles[0].transform.position;

            foreach (BoardTile tile in tiles)
            {
                if (tile.transform.position.x < minPosition.x)
                    minPosition.x = tile.transform.position.x;
            
                if (tile.transform.position.y < minPosition.y)
                    minPosition.y = tile.transform.position.y;
            }

            return minPosition;
        }

        public Vector2Int GetCoordinates(BoardTile tile)
        {
            return TilePositionToCoords(tile.rect.position, tile.rect.pivot);
        }
        
        public Vector2Int TilePositionToCoords(Vector3 position, Vector2 pivot)
        {
            Vector3[] worldCorners = new Vector3[4];
            tileParent.GetWorldCorners(worldCorners);

            Vector2 brickSize = GetWorldBrickSize();

            Vector2 localPoint = position - worldCorners[0] - Vector3.Scale(brickSize, pivot);
            Vector2 coords = localPoint / brickSize;

            return Vector2Int.RoundToInt(coords);
        }

        public int GetEmptyTilesCount()
        {
            int size = 0;
            foreach (var tile in field)
            {
                if(tile != null)
                    continue;
                size++;
            }

            return size;
        }

        public bool CanPlaceFigureOnBoard(int[,] figure)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    bool canPlace = TryPlaceFigure(figure, new Vector2Int(x, y), out var placeCoordinates); 
                    
                    if (!canPlace) continue;
                    
                    int[,] board = GetMatrix();
                    foreach (var coordinate in placeCoordinates)
                        board[coordinate.x, coordinate.y] = 2;

                    return true;
                }
            }
            return false;
        }
        
        public bool CanPlaceFigureOnBoard(Figure figure)
        {
            if (figure.placed)
                return false;
            
            return CanPlaceFigureOnBoard(figure.figureCoordinates);
        }
        
        
        
    }
}