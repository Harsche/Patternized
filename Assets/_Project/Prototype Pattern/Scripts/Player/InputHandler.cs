using System;
using System.Collections;
using PrototypePattern.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypePattern.Player
{
    public class InputHandler : MonoBehaviour
    {
        private PlayerController _player;
        private PlayerControls _playerControls;

        private InputAction _attack;
        private InputAction _move;

        private Rigidbody2D _rigidBody2D;

        [SerializeField] private Vector2 direction;
        [SerializeField] private float _speed = 20f;
        [SerializeField] private bool _isInKnockback = false;

        private void Start()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _player = GetComponent<PlayerController>();
        }
        private void FixedUpdate()
        {
            if (!_player.Invencible)
            {
                _rigidBody2D.velocity = new Vector2(direction.x * _speed, direction.y * _speed);
            }
        }

        private void OnEnable()
        {
            _playerControls = new PlayerControls();

            _move = _playerControls.Player.Move;
            _move.performed += HandleMovement;
            _move.canceled += HandleMovement;

            _attack = _playerControls.Player.Attack;
            _attack.performed += HandleAttack;

            _playerControls.Enable();
        }
        private void OnDisable()
        {
            _move.performed -= HandleMovement;
            _move.canceled -= HandleMovement;

            _attack.performed -= HandleAttack;

            _playerControls.Disable();
        }
        private void HandleMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                direction = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                direction = Vector2.zero;
            }
        }

        private void HandleAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Atacando!");
            }
        }

        public void ApplyKnockback(Vector2 knockback, float duration = 0.1f)
        {
            StartCoroutine(KnockbackCoroutine(knockback, duration));
        }

        private IEnumerator KnockbackCoroutine(Vector2 knockback, float duration)
        {
            _player.Invencible = true;
            _rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(duration);
            
            _player.Invencible = false;
        }
    }
}