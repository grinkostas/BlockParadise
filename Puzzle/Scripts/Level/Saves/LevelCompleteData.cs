using GameCore.CrossScene.Scripts.Saves;
using UnityEngine.UI;

namespace GameCore.Puzzle.Scripts.Level
{
    [ES3Serializable]
    public class LevelCompleteData
    {
        public int score;
        public int starCollected;
        public bool finished;

        public bool scoreHandled { get; set; } = false;
        public bool starsHandled { get; set; } = false;

        public LevelCompleteData() : this(0, false, 0)
        {
        }

        public LevelCompleteData(int score, bool finished, int stars = 1)
        {
            if (finished)
                starCollected = stars;
            else
                starCollected = 0;
            this.score = score;
            this.finished = finished;
        }
    }
}