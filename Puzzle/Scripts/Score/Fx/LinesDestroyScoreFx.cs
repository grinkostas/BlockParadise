using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameCore.CrossScene.Scripts.Sounds;
using GameCore.CrossScene.Scripts.Sounds.SoundPools;
using GameCore.Scripts.Sounds.SoundPools;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace GameCore.Puzzle.Scripts.Score.Fx
{
    public class LinesDestroyScoreFx : MonoBehaviour
    {
        [SerializeField] private LinesDestroyScore _linesDestroyScore;
        [SerializeField] private MonoPool<ScoreText> _scoreTextPool;
        [SerializeField] private MonoPool<ScoreComboText> _scoreComboTextPool;
        [SerializeField] private float _showTime;
        [SerializeField] private List<ScoreDescription> _scoreDescriptions;
        [SerializeField] private float _playSoundDelay = 0.15f;
        [SerializeField] private List<AudioSource> _comboSounds;
        [Header("Debug")] 
        [SerializeField] private int _combo;
        
        [System.Serializable]
        public class ScoreDescription
        {
            public int lineCount;
            public string text;
            public SoundPool sound;
        }
        
        private void Awake()
        {
            _scoreTextPool.Initialize();
            _scoreComboTextPool.Initialize();
        }

        private void OnEnable()
        {
            _linesDestroyScore.removedLines.On(OnRemovedLines);
        }

        private void OnDisable()
        {
            _linesDestroyScore.removedLines.Off(OnRemovedLines);
        }

        private void OnRemovedLines(LineDestroyData data)
        {
            var sequence = DOTween.Sequence();
            if (data.inRow > 1)
            {
                sequence.AppendCallback(()=>ShowCombo(data.inRow));
                sequence.AppendInterval(_showTime);
            }
            sequence.AppendCallback(() => ShowScore(data));
            sequence.SetLink(gameObject);
        }

        private void ShowScore(LineDestroyData data)
        {
            var text = _scoreTextPool.Get();
            string description = "";
            if (data.linesCount > 1)
            {
                var scoreDescription = GetDescription(data.linesCount);
                description = scoreDescription.text;
                DOVirtual.DelayedCall(_playSoundDelay, scoreDescription.sound.PlaySound);
            }
            text.Show(data.score, description);
            DOVirtual.DelayedCall(_showTime, () => text.Pool.Return(text)).SetLink(gameObject);
        }

        private ScoreDescription GetDescription(int destroyedLines)
        {
            int maxLineCount = _scoreDescriptions.Max(x => x.lineCount);
            var scoreDescription =
                _scoreDescriptions.Find(x => x.lineCount == Mathf.Min(maxLineCount, destroyedLines));
            return scoreDescription;
        }
      
        
        [Button()]
        private void ShowComboDebug()
        {
            ShowCombo(_combo);
        }
        private void ShowCombo(int combo)
        {
            var comboText = _scoreComboTextPool.Get();
            int soundIndex = Mathf.Clamp(combo - 2, 0, _comboSounds.Count);
            var soundItem = _comboSounds[soundIndex];
            soundItem.Play();
            comboText.Show(combo);
            DOVirtual.DelayedCall(_showTime, () => comboText.Pool.Return(comboText)).SetLink(gameObject);
            
        }
        
    }
}