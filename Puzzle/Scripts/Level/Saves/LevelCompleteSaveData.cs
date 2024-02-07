using GameCore.CrossScene.Scripts.Saves;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelCompleteSaveData : SaveData<LevelCompleteData>
    {
        public int score
        {
            get => savedValue.score;
            set
            {
                savedValue.score = value;
                Save();
            }
        }

        public int starCollected
        {
            get => savedValue.starCollected;
            set
            {
                savedValue.starCollected = value;
                Save();
            }
        }

        public bool finished
        {
            get => savedValue.finished;
            set
            {
                savedValue.finished = value;
                Save();
            }
        }
        
        public bool scoreHandled
        {
            get => savedValue.scoreHandled;
            set
            {
                savedValue.scoreHandled = value;
                Save();
            }
        }
        public bool starsHandled{
            get => savedValue.starsHandled;
            set
            {
                savedValue.starsHandled = value;
                Save();
            }
        }
        
        
        public LevelCompleteSaveData(string key, LevelCompleteData defaultValue = default) : base(key, defaultValue)
        {
        }
    }
}