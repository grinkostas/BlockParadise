using DG.Tweening;
using StaserSDK.Extentions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.CrossScene.Scripts.UI
{
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private float _delay;
        
        public void Reload()
        {
            DOVirtual.DelayedCall(_delay, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }).ConfigureWithId(this, gameObject);
        }
    }
}