using UnityEngine;

namespace CommandPattern{
    public interface ICommand{
        void Execute();
        void Undo();
    }
}