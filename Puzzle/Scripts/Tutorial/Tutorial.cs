using System.Collections.Generic;
using DG.Tweening;
using GameCore.CreativesScene.UI;
using GameCore.CrossScene.Scripts.Analytics;
using GameCore.CrossScene.Scripts.Scenes;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Figures;
using GameCore.Puzzle.Scripts.Field.Lines;
using GameCore.Puzzle.Scripts.Field.Patterns;
using GameCore.Puzzle.Scripts.Level;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace GameCore.Puzzle.Scripts.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private List<TutorialStep> _steps;
        [SerializeField] private float _stepDelay;
        [SerializeField] private LevelPatternGenerator _patternGenerator;
        [SerializeField] private float _sceneChangeDelay;
        
        [InjectOptional, UsedImplicitly] public IAnalytics analytics { get; }
        
        [System.Serializable]
        public class TutorialStep
        {
            public TemplatePattern boardTemplate;
            public FigurePattern figure;
            public int figureRotation;
        }
        
        [Inject, UsedImplicitly] public GameBoardData boardData { get; }
        [Inject, UsedImplicitly] public FigureSpawner figureSpawner { get; }
        [Inject, UsedImplicitly] public LinesDestroyer linesDestroyer { get; }

        public static string saveKey = "GameTutorial";
        public static bool tutorialNeeded => ES3.Load(saveKey, true);
        
        private int _currentStep = 0;
        private Figure _currentFigure;

        public TheSignal startedStep { get; } = new();
        public TheSignal completedStep { get; } = new();
        public TheSignal selectedFigure { get; } = new();
        
        private void OnEnable()
        {
            linesDestroyer.removedLines.On(OnRemoveLines);
        }

        private void OnDisable()
        {
            linesDestroyer.removedLines.Off(OnRemoveLines);
        }

        private void Start()
        {
            NextStep();
        }
        
        private void OnRemoveLines(int lines)
        {
            if(tutorialNeeded)
                analytics.SendComplete($"step_{_currentStep-1}", "Tutorial");
            completedStep.Dispatch();
            NextStep();
        }

        private void NextStep()
        {
            if (tutorialNeeded == false)
            {
                EndTutorial();
                return;
            }
            if (tutorialNeeded)
                ;
            
            DOVirtual.DelayedCall(_stepDelay, () =>
            {
                if (_currentFigure != null)
                    _currentFigure.pointerDown.Off(OnPointerDown);

                if (_currentStep >= _steps.Count)
                {
                    analytics.SendEvent($"completed", "Tutorial");
                    EndTutorial();
                    return;
                }

                analytics.SendEvent($"started:step_{_currentStep}", "Tutorial");
                startedStep.Dispatch();
                var step = _steps[_currentStep];
                _patternGenerator.SpawnPattern(0, 0, step.boardTemplate.GetPattern(), boardData.GetRandomColor());
                _currentFigure = figureSpawner.Spawn(step.figure.GetPattern(), step.figureRotation);
                _currentFigure.pointerDown.On(OnPointerDown);
                _currentStep++;
            }).SetLink(gameObject);
        }

        private void OnPointerDown(Figure figure)
        {
            selectedFigure.Dispatch();
        }

        private void EndTutorial()
        {
            var operation = SceneManager.LoadSceneAsync(ScenesName.level);
            operation.allowSceneActivation = false;
            ES3.Save(saveKey, false);
            DOVirtual.DelayedCall(_sceneChangeDelay, () => operation.allowSceneActivation = true)
                .SetUpdate(false).SetLink(gameObject);
        }
    } 
}