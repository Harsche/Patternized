using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern{
    public class GridMovement : MonoBehaviour{
        [SerializeField] private Camera _camera;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Character _character;

        private Pathfinder _pathfinder;
        private int _currentCommandIndex;
        private readonly List<ICommand> _commands = new();

        public bool CanUndo => _currentCommandIndex >= 0;
        public bool CanRedo => _currentCommandIndex < _commands.Count - 1;

        private void Awake(){
            BoundsInt tilemapCellBounds = _tilemap.cellBounds;
            var minCell = (Vector2Int) tilemapCellBounds.min;
            var maxCell = (Vector2Int) tilemapCellBounds.max;
            _pathfinder = new Pathfinder(minCell, maxCell, Array.Empty<Vector2Int>());
        }

        private void Update(){
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);
            Vector3Int cellPosition = _tilemap.WorldToCell(mouseWorldPosition);

            if (_character.WalkCoroutine != null || !Input.GetMouseButtonDown(0) || !_tilemap.HasTile(cellPosition)){
                return;
            }

            var startCell = (Vector2Int) _tilemap.WorldToCell(_character.transform.position);
            _pathfinder.SetGoal(startCell, (Vector2Int) cellPosition);

            AddWalkCommand();
        }

        private void AddWalkCommand(){
            if (_currentCommandIndex < _commands.Count - 1){ ClearRedoCommands(); }
            WalkCommand walkCommand = new(_pathfinder.Search(), _character);
            _commands.Add(walkCommand);
            _currentCommandIndex = _commands.Count - 1;
            walkCommand.Execute();
        }

        public void RedoWalkCommand(){
            _currentCommandIndex++;
            _commands[_currentCommandIndex].Execute();
        }

        public void UndoWalkCommand(){
            _commands[_currentCommandIndex].Undo();
            _currentCommandIndex--;
        }

        private void ClearRedoCommands(){
            if (_currentCommandIndex < -1 || _currentCommandIndex > _commands.Count - 1){
                return;
            }

            int removeCount = _commands.Count - _currentCommandIndex - 1;
            _commands.RemoveRange(_currentCommandIndex + 1, removeCount);
        }
    }
}