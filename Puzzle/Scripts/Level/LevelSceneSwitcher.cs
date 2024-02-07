using GameCore.CrossScene.Scripts.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Puzzle.Scripts.Level
{
    public class LevelSceneSwitcher 
    {
        private AsyncOperation _metaOperation;
        private AsyncOperation _levelOperation;
        private AsyncOperation _classicOperation;

        public LevelSceneSwitcher()
        {
            _metaOperation = null;
            _levelOperation = null;
        }
        
        public void PrepareMeta()
        {
            if(_metaOperation != null)
                return;
            _metaOperation = SceneManager.LoadSceneAsync(ScenesName.meta);
            _metaOperation.allowSceneActivation = false;
        }
        
        public void LoadMeta()
        {
            if(_metaOperation == null)
                PrepareMeta();
            _metaOperation.allowSceneActivation = true;
        }
        
        public void PrepareClassic()
        {
            if(_classicOperation != null)
                return;
            _classicOperation = SceneManager.LoadSceneAsync(ScenesName.classic);
            _classicOperation.allowSceneActivation = false;
        }

        public void PrepareLevel()
        {
            if(_levelOperation != null)
                return;
            _levelOperation = SceneManager.LoadSceneAsync(ScenesName.level);
            _levelOperation.allowSceneActivation = false;
        }

        public void LoadLevel()
        {
            if(_levelOperation == null)
                PrepareLevel();
            _levelOperation.allowSceneActivation = true;
        }
    }
}