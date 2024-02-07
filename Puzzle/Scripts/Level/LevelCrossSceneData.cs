using GameCore.CrossScene.Scripts.Saves;

namespace GameCore.Puzzle.Scripts.Level
{
    public static class LevelCrossSceneData
    {
        private static readonly string saveKey = "LastLevelCompleteData";

        public static LevelCompleteSaveData completeData { get; } = new (saveKey, new LevelCompleteData());

        public static void Clear()
        {
            ES3.DeleteKey(saveKey);
        }

        public static void CompleteLevel(int score)
        {
            completeData.savedValue = new LevelCompleteData(score, true);
        }

        public static void FailLevel()
        {
            completeData.savedValue = new LevelCompleteData(0, false);
        }
    }
}