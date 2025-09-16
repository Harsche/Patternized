using System;
using System.Collections;
using PrototypePattern.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypePattern.Player
{
    public class InputHandler : MonoBehaviour
    {
        [Header("Components")]
        private PlayerController _player;
        private PlayerControls _playerControls;
        private Rigidbody2D _rigidBody2D;

        [Header("Input Actions")]
        private InputAction _attack;
        private InputAction _move;

        [Header("Movement Settings")]
        [SerializeField] private Vector2 _direction;
        [SerializeField] private float _speed = 20f;

        private void Awake()
        {
            _playerControls = new PlayerControls();

            _rigidBody2D = GetComponent<Rigidbody2D>();
            _player = GetComponent<PlayerController>();
        }
        private void FixedUpdate()
        {
            if (!_player.Invincible)
            {
                _rigidBody2D.velocity = _direction * _speed;
            }
        }

        private void OnEnable()
        {

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
                _direction = context.ReadValue<Vector2>();
            }
            else if (context.canceled)
            {
                _direction = Vector2.zero;
            }
        }

        private void HandleAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Debug.Log("Attacking!");
            }
        }

        public void ApplyKnockback(Vector2 knockback, float duration = 0.1f)
        {
            StartCoroutine(KnockbackCoroutine(knockback, duration));
        }

        private IEnumerator KnockbackCoroutine(Vector2 knockback, float duration)
        {
            _player.Invincible = true;
            _rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);
            
            yield return new WaitForSeconds(duration);
            
            _player.Invincible = false;
        }
    }
}