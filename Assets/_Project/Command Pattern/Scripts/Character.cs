using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern{
    public class Character : MonoBehaviour{
        private static readonly int IsMovingHash = Animator.StringToHash("Is Moving");

        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private Tilemap _pathTilemap;
        [SerializeField] private Tile _pathTile;
        [SerializeField] private float _walkSpeed = 2;
        [SerializeField] private Vector2 _cellOffset;
        [SerializeField] private Animator _animator;

        public Coroutine WalkCoroutine{ get; private set; }
        private List<Vector2Int> _currentPath;

        public event Action<bool> OnWalkStateChanged;
        
        public void WalkPath(Stack<Vector2Int> path){
            WalkCoroutine = StartCoroutine(WalkPathCoroutine(path));
        }

        private IEnumerator WalkPathCoroutine(Stack<Vector2Int> path){
            OnWalkStateChanged?.Invoke(true);
            _currentPath = path.ToList();
            _animator.SetBool(IsMovingHash, true);
            UpdatePath();
            while (path.Count > 0){
                Vector2 startPosition = transform.position;
                Vector2Int targetCell = path.Pop();
                Vector2 targetPosition = (Vector2) _tilemap.CellToWorld((Vector3Int) targetCell) + _cellOffset;
                Vector2 offset = targetPosition - startPosition;
                transform.localScale = new Vector3(Mathf.Sign(offset.x), 1, 1);
                float elapsedTime = 0;
                while (elapsedTime <= 1 / _walkSpeed){
                    elapsedTime += Time.deltaTime;
                    transform.position = Vector2.Lerp(startPosition, targetPosition, elapsedTime / (1 / _walkSpeed));
                    yield return null;
                }
                transform.position = targetPosition;
                _currentPath.Remove(targetCell);
                UpdatePath();
            }
            _animator.SetBool(IsMovingHash, false);
            WalkCoroutine = null;
            OnWalkStateChanged?.Invoke(false);
        }

        private void UpdatePath(){
            _pathTilemap.ClearAllTiles();
            foreach (Vector2Int cells in _currentPath){ _pathTilemap.SetTile((Vector3Int) cells, _pathTile); }
        }
    }
}