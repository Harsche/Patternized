using System;
using UnityEngine;

namespace PrototypePattern.Input
{
    public class InputHandler : MonoBehaviour
    {
        // Events
        public event Action OnAttack;
        public event Action OnReload;

        // Vector Properties
        public Vector2 MoveDirection { get; private set; }
        public Vector3 MousePosition { get; private set; }

        // Components
        private PlayerControls _playerControls;

        private void Awake()
        {
            _playerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerControls.Player.Aim.performed += ctx => MousePosition = ctx.ReadValue<Vector2>();

            _playerControls.Player.Move.performed += ctx => MoveDirection = ctx.ReadValue<Vector2>();
            _playerControls.Player.Move.canceled += ctx => MoveDirection = Vector2.zero;

            _playerControls.Player.Attack.performed += ctx => OnAttack?.Invoke();
            _playerControls.Player.Reload.performed += ctx => OnReload?.Invoke();

            _playerControls.Enable();
        }

        private void OnDisable()
        {
            _playerControls.Player.Aim.performed -= ctx => MousePosition = ctx.ReadValue<Vector2>();

            _playerControls.Player.Move.performed -= ctx => MoveDirection = ctx.ReadValue<Vector2>();
            _playerControls.Player.Move.canceled -= ctx => MoveDirection = Vector2.zero;

            _playerControls.Player.Attack.performed -= ctx => OnAttack?.Invoke();
            _playerControls.Player.Reload.performed -= ctx => OnReload?.Invoke();

            _playerControls.Disable();
        }

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(MousePosition);
            return worldPosition;
        }
    }
}
