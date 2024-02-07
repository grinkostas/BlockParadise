using System;
using DG.Tweening;
using UnityEngine;

namespace GameCore.Puzzle.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        private void Awake()
        {
            #if UNITY_EDITOR
                DOTween.useSafeMode = false;
            #endif
        }

        private void OnDestroy()
        {
            DOTween.KillAll();
        }
    }
}      