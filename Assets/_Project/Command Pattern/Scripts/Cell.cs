using UnityEngine;

namespace CommandPattern{
    public class Cell{
        public readonly Vector2Int Position;
        public readonly bool Blocked;

        public Vector2Int Parent{ get; private set; }
        public int Cost { get; private set; }
        public int EstimatedCost { get; private set; }

        public Cell(Vector2Int position, bool blocked){
            Position = position;
            Blocked = blocked;
            Parent = default;
            Cost = int.MaxValue;
            EstimatedCost = int.MaxValue;
        }

        public void SetParentPosition(Vector2Int parent){
            Parent = parent;
        }
        
        public void SetCost(int cost){
            Cost = cost;
        }
        
        public void SetEstimatedCost(int cost){
            Cost = cost;
        }

        public void Reset(){
            Parent = default;
            Cost = int.MaxValue;
            EstimatedCost = int.MaxValue;
        }
    }
}