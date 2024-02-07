namespace GameCore.Puzzle.Scripts.Field.Tiles
{
    public class TileData
    {
        public BoardTile tile;
        public int x;
        public int y;
        public bool skipAnimation = false;

        public TileData(BoardTile tile, int x, int y, bool skipAnimation = false)
        {
            this.tile = tile;
            this.x = x;
            this.y = y;
            this.skipAnimation = false;
        }
    }
}