using System;
using UnityEngine;
using UnityEngine.UI;

namespace CommandPattern{
    public class UI : MonoBehaviour{
        [SerializeField] private Button _undoButton;
        [SerializeField] private Button _redoButton;
        [SerializeField] private GridMovement _gridMovement;

        private void Awake(){
            Character.OnWalkStateChanged += isWalking => {
                if (isWalking){
                    _undoButton.interactable = false;
                    _redoButton.interactable = false;
                }
                else{
                    _undoButton.interactable = _gridMovement.CanUndo;
                    _redoButton.interactable = _gridMovement.CanRedo;
                }
            };
        }
    }
}