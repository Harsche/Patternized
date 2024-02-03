using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CommandPattern{
    public class Pathfinder{
        private bool _foundGoal;
        private Vector2Int _goal;
        private Vector2Int _origin;
        private readonly Cell[,] _cellsData;

        private readonly SortedSet<(int, (int, int))> _searchingCells = new();
        private readonly HashSet<Cell> _searchedCells = new();
        private readonly Stack<Vector2Int> _path = new();


        public Pathfinder(Vector2Int firstCell, Vector2Int finalCell, ICollection<Vector2Int> blockedCells){
            int sizeX = finalCell.x - firstCell.x + 1;
            int sizeY = finalCell.y - firstCell.y + 1;
            _cellsData = new Cell[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++){
                for (int j = 0; j < sizeY; j++){
                    Vector2Int position = new(firstCell.x + i, firstCell.y + j);
                    bool isBlocked = blockedCells.Contains(position);
                    Cell cell = new(position, isBlocked);
                    _cellsData[i, j] = cell;
                    _searchedCells.Add(cell);
                }
            }
        }

        public void SetGoal(Vector2Int startPosition, Vector2Int targetPosition){
            _origin = startPosition;
            _goal = targetPosition;
        }

        public Stack<Vector2Int> Search(){
            _path.Clear();
            _searchingCells.Clear();
            _searchingCells.Add((0, (_origin.x, _origin.y)));
            _searchedCells.Clear();
            foreach (Cell cell in _cellsData){ cell.Reset(); }
            _foundGoal = false;

            SearchThroughCells();
            if (_foundGoal){ return _path; }
            Debug.LogError("COULDN'T FIND GOAL");
            return null;
        }

        private bool IsValid(Vector2Int position){
            return _cellsData.Cast<Cell>().Any(cell => cell.Position == position);
        }

        private bool IsGoal(Vector2Int cellPosition){
            return cellPosition == _goal;
        }

        private int CalculateHeuristic(Vector2Int position){
            return Mathf.Abs(position.x - _goal.x) + Mathf.Abs(position.y - _goal.y);
        }

        private void TracePath(){
            Cell currentCell = _cellsData.Cast<Cell>().First(cell => cell.Position == _goal);
            _path.Push(currentCell.Position);
            while (currentCell.Position != _origin){
                _path.Push(currentCell.Parent);
                currentCell = _cellsData.Cast<Cell>().First(cell => cell.Position == currentCell.Parent);
            }
        }

        private void GenerateSuccessor(Vector2Int cellPosition, Cell parentCell){
            if (!IsValid(cellPosition)){ return; }
            Cell cell = _cellsData.Cast<Cell>().First(cell => cell.Position == cellPosition);

            if (IsGoal(cellPosition)){
                cell.SetParentPosition(parentCell.Position);
                TracePath();
                _foundGoal = true;
                return;
            }

            if (_searchedCells.Contains(cell) || cell.Blocked){ return; }
            int g = parentCell.Cost + 1;
            int h = CalculateHeuristic(cellPosition);
            int f = g + h;
            if (cell.EstimatedCost != int.MaxValue && cell.EstimatedCost <= f){ return; }
            _searchingCells.Add((f, (cellPosition.x, cellPosition.y)));
            cell.SetEstimatedCost(f);
            cell.SetParentPosition(parentCell.Position);
            cell.SetCost(g);
        }

        private void SearchThroughCells(){
            while (_searchingCells.Count > 0){
                // Get cell heuristic and position
                (int f, (int x, int y)) = _searchingCells.Min;
                Cell currentCell = _cellsData.Cast<Cell>().First(cell => cell.Position == new Vector2Int(x, y));

                _searchingCells.Remove((f, (x, y)));
                _searchedCells.Add(currentCell);

                GenerateSuccessor(currentCell.Position + Vector2Int.down, currentCell);
                if (_foundGoal){ return; }
                GenerateSuccessor(currentCell.Position + Vector2Int.left, currentCell);
                if (_foundGoal){ return; }
                GenerateSuccessor(currentCell.Position + Vector2Int.right, currentCell);
                if (_foundGoal){ return; }
                GenerateSuccessor(currentCell.Position + Vector2Int.up, currentCell);
                if (_foundGoal){ return; }
            }
        }
    }
}