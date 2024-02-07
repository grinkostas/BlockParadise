using DG.Tweening;
using GameCore.Puzzle.Scripts.Field.Figures;

namespace GameCore.Puzzle.Scripts.Tutorial
{
    public class TutorialFiguresHighlighter : FiguresHighlighter
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            _figurePlacer.returned.On(ClearHighlight);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _figurePlacer.returned.Off(ClearHighlight);
        }

        private void DelayedClearHighlight()
        {
            DOTween.Kill(this);
            DOVirtual.DelayedCall(0.15f, ClearHighlight).SetLink(gameObject).SetId(this);
        }
    }
}