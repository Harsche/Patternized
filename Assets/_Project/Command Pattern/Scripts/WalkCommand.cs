using System.Collections.Generic;
using UnityEngine;

namespace CommandPattern{
    public class WalkCommand : ICommand{
        private readonly Character _character;
        private readonly Stack<Vector2Int> _path;

        public WalkCommand(Stack<Vector2Int> path, Character character){
            _path = path.Clone();
            _character = character;
        }

        public void Execute(){
            Stack<Vector2Int> path = _path.Clone();
            path.Pop();
            _character.WalkPath(path);
        }

        public void Undo(){
            Stack<Vector2Int> reversePath = _path.Reverse();
            reversePath.Pop();
            _character.WalkPath(reversePath);
        }
    }
}