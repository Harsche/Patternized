using PrototypePattern.Input;
using System.Collections;
using UnityEngine;

namespace PrototypePattern.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(InputHandler))]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Components")]
        private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;
        private PlayerController _playerController;
        [Header("Movement Settings")]
        [SerializeField] private float _speed = 10f;
        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private float _baseSpeed;
        private Coroutine _speedCoroutine;

        public void StartSpeedBoost(float multiplier, float duration)
        {
            if (_baseSpeed <= 0f) _baseSpeed = _speed;

            if (_speedCoroutine != null)
                StopCoroutine(_speedCoroutine);

            _speed = _baseSpeed * multiplier;
            _speedCoroutine = StartCoroutine(SpeedBoostRoutine(duration));
        }

        private IEnumerator SpeedBoostRoutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            _speed = _baseSpeed;
            _speedCoroutine = null;
        }

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
            _playerController = GetComponent<PlayerController>();

            _baseSpeed = _speed;
        }

        private void FixedUpdate()
        {
            if (_playerController.CanMove)
            {
                _rigidBody2D.velocity = _inputHandler.MoveDirection * _speed;
            }
        }
    }
}
