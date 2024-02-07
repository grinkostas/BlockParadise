using System;
using System.Collections.Generic;
using GameCore.Puzzle.Scripts.Bindings;
using GameCore.Puzzle.Scripts.Field.Board;
using GameCore.Puzzle.Scripts.Field.Tiles;
using JetBrains.Annotations;
using NepixSignals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace GameCore.CrossScene.Scripts.Boosters
{
    public class TileSelector : MonoBehaviour
    {
        [Inject, UsedImplicitly] public MainCanvas canvas { get; }
        [Inject, UsedImplicitly] public EventSystem eventSystem { get; }
        [Inject, UsedImplicitly] public GameBoard board { get; }

        private bool _waitForSelection = false;

        public TheSignal<BoardTile> selectedTile { get; } = new();
        

        public void StartHandlingSelection()
        {
            _waitForSelection = true;
        }

        public void StopHandlingSelection()
        {
            _waitForSelection = false;
        }
        
        private void Update()
        {
            if(_waitForSelection == false)
                return;
            
            if(Input.GetMouseButtonDown(0))
                SelectTile();
        }

        private void SelectTile()
        {
            var pointerEventData = new PointerEventData(eventSystem)
            {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            canvas.raycaster.Raycast(pointerEventData, results);
            if(results.Has(x => x.gameObject.TryGetComponent<BoardTile>(out _), out var result) == false)
                return;
            
            var boardTile = result.gameObject.GetComponent<BoardTile>();
            _waitForSelection = true;
            selectedTile.Dispatch(boardTile);
        }
    }
}