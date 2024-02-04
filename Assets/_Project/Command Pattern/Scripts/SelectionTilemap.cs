using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CommandPattern{
    public class SelectionTilemap : MonoBehaviour{
        [SerializeField] private Camera _camera;
        [SerializeField] private Tilemap _selectionTilemap;
        [SerializeField] private Character _character;
        [SerializeField, Range(0, 1)] private float _showTime = 1f;

        private List<SelectionTile> _tiles = new();

        private void Awake(){
            BoundsInt tilemapCellBounds = _selectionTilemap.cellBounds;
            var minCell = (Vector2Int) tilemapCellBounds.min;
            var maxCell = (Vector2Int) tilemapCellBounds.max;
            int sizeX = maxCell.x - minCell.x + 1;
            int sizeY = maxCell.y - minCell.y + 1;

            for (int i = 0; i < sizeX; i++){
                for (int j = 0; j < sizeY; j++){
                    _tiles.Add(new SelectionTile(new Vector3Int(minCell.x + i, minCell.y + j)));
                }
            }

            _character.OnWalkStateChanged += isWalking => {
                float targetAlpha = isWalking ? 0 : 1;
                StartCoroutine(FadeCoroutine(targetAlpha, 0.2f));
            };
        }

        private void Update(){
            Vector2 mouseScreenPosition = Input.mousePosition;
            Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);
            Vector3Int cellPosition = _selectionTilemap.WorldToCell(mouseWorldPosition);

            if (_selectionTilemap.HasTile(cellPosition)){
                SelectionTile mouseOverTile = _tiles.First(tile => tile.Position == cellPosition);
                mouseOverTile.MouseOverTime = Time.time;
            }

            foreach (SelectionTile tile in _tiles){ tile.UpdateColor(_selectionTilemap, _showTime); }
        }

        private IEnumerator FadeCoroutine(float targetAlpha, float time){
            float elapsedTime = 0f;
            float startAlpha = _selectionTilemap.color.a;
            while (elapsedTime < time){
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / time);
                _selectionTilemap.color = new Color(1, 1, 1, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _selectionTilemap.color = new Color(1, 1, 1, targetAlpha);
        }

        private class SelectionTile{
            public float MouseOverTime = -10;
            public Vector3Int Position{ get; }

            public SelectionTile(Vector3Int position){
                Position = position;
            }

            public void UpdateColor(Tilemap tilemap, float showTime){
                float alpha = Remap(
                    MouseOverTime,
                    new Vector2(Time.time - showTime, Time.time),
                    new Vector2(0, 1)
                );
                tilemap.SetColor(Position, new Color(1f, 1f, 1f, alpha));
            }

            private float Remap(float value, Vector2 inMinMax, Vector2 outMinMax){
                float lerp = (value - inMinMax.x) / (inMinMax.y - inMinMax.x);
                return outMinMax.x + (outMinMax.y - outMinMax.x) * lerp;
            }
        }
    }
}