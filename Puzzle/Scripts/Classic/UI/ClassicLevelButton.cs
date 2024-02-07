using DG.Tweening;
using GameCore.CrossScene.Scripts.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Puzzle.Scripts.Classic.UI
{
    public class ClassicLevelButton : MonoBehaviour
    {
        [SerializeField] private float _delay = 0.35f;

        public void Retry()
        {
            DOVirtual.DelayedCall(_delay, () => SceneManager.LoadScene(ScenesName.classic));
        }

        public void Home()
        {
            DOVirtual.DelayedCall(_delay, () => SceneManager.LoadScene(ScenesName.meta));
        }
    }
}