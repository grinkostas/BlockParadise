using DG.Tweening;
using StaserSDK.Extentions;
using TMPro;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Level.Goals.Counters
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreGoalText;
        [SerializeField] private float _textAnimationDuration;
        [SerializeField] private Vector3 _punch;
        [SerializeField] private float _pucnhDuration;
        [SerializeField] private AudioSource _audioSource;

        public void ResetCounter()
        {
            _scoreGoalText.text = "0";
        }
        
        public void SetCount(int count)
        {
            _audioSource.Play();
            _audioSource.volume = 1;
            DOVirtual.Float(0, count, _textAnimationDuration, value => _scoreGoalText.text = $"{Mathf.RoundToInt(value)}")
                .OnComplete(()=>
                {
                    _audioSource.Stop();
                    _audioSource.volume = 0;
                    _scoreGoalText.transform.DOPunchScale(_punch, _pucnhDuration, 2);
                }).ConfigureWithId(this, gameObject);
        }
        
    }
}