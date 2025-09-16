using PrototypePattern.Input;
using UnityEngine;

namespace PrototypePattern.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float _speed = 10f;

        [Header("Components")]
        private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;
        private PlayerController _playerController;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
            _playerController = GetComponent<PlayerController>();
        }

        private void FixedUpdate()
        {
            if (!_playerController.Invincible)
            {
                _rigidBody2D.velocity = _inputHandler.MoveDirection * _speed;
            }
        }
    }
}
