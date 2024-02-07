namespace GameCore.Puzzle.Scripts.Score
{
    public class LineDestroyData
    {
        public int inRow;
        public int linesCount;
        public int score;

        public LineDestroyData(int inRow = 0, int linesCount = 0, int score = 0)
        {
            this.inRow = inRow;
            this.linesCount = linesCount;
            this.score = score;
        }
    }
}