namespace GameCore.Puzzle.Scripts.Level
{
    [ES3Serializable]
    public class PatternSnapshot
    {
        public LineItem[] items;
            
        [ES3Serializable]
        public class LineItem
        {
            public int templateIndex;
            public int spawnPosition;
            public bool mirroredHorizontal = false;
            public bool mirroredVertical = false;
            public bool isEmpty;

            public LineItem()
            {
                isEmpty = true;
            }

            public LineItem(int templateIndex, int spawnPosition)
            {
                this.templateIndex = templateIndex;
                this.spawnPosition = spawnPosition;
                isEmpty = false;
            }
        }

        public PatternSnapshot() : this(0){}
        public PatternSnapshot(int size)
        {
            items = new LineItem[size];
            for (int i = 0; i < size; i++)
                items[i] = new LineItem();
        }

        public void AddItem(int index, LineItem lineItem)
        {
            items[index] = lineItem;
        }
    }
}