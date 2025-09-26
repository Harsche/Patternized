using System;
using System.Collections;
using PrototypePattern.Input;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(InputHandler))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private Color? _originalColor = null;
        private int _twinkleId = 0;
        [Header("Health Settings")]
        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth = 100f;

        [Header("Knockback Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float _knockbackDuration = 0.1f;

        [Header("Status Settings")]
        [SerializeField] private bool _targetable = true;
        [SerializeField] private bool _invincible = false;
        [SerializeField] private bool _canMove = true;

        [Header("Components")]
        [SerializeField] private TilemapCollider2D _limitsTilemap;
        private Collider2D _physicsCollider;
        private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;

        public event Action<float> OnHealthChanged;

        [SerializeField] private GameObject _shield;
        [SerializeField] private GameManager _gameManager;

        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Clamp(value, 0f, _maxHealth);
                if (_health <= 0f)
                {
                    Targetable = false;
                    OnDeath();
                }

                OnHealthChanged?.Invoke(NormalizedHealth);
            }
        }

        public float NormalizedHealth => Mathf.Approximately(_maxHealth, 0f) ? 0f : Mathf.Clamp01(_health / _maxHealth);
        public float MaxHealth => _maxHealth;


        public bool Targetable
        {
            get => _targetable;
            private set
            {
                _targetable = value;
            }
        }

        public bool Invincible
        {
            get => _invincible;
            set
            {
                _invincible = value;
                _shield.SetActive(_invincible);
            }
        }

        public bool CanMove
        {
            get => _canMove;
            private set => _canMove = value;
        }

        private void Awake()
        {
            _physicsCollider = GetComponent<Collider2D>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
            _canMove = true;
        }
        public void OnHit(int damage, Vector2 knockback)
        {
            if (Targetable && !Invincible)
            {
                Health -= damage;
                StartCoroutine(ApplyKnockback(knockback, _knockbackDuration));
            }
        }

        public void OnHit(int damage)
        {
            if (Targetable && !Invincible)
                Health -= damage;
        }

        public void OnDeath()
        {
            _gameManager.ShowGameOver();
            Debug.Log("Died!");
        }

        private bool _isTwinkling = false;

        private IEnumerator ApplyKnockback(Vector2 knockback, float duration)
        {
            Invincible = true;
            CanMove = false;
            _rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);

            _isTwinkling = true;
            StartCoroutine(HitTwinkle(1f));

            yield return new WaitForSeconds(duration);
            Invincible = false;
            CanMove = true;
        }

        private IEnumerator HitTwinkle(float timer)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (_originalColor == null) _originalColor = spriteRenderer.color;

            float elapsedTime = 0f;

            while (elapsedTime <= timer && _isTwinkling)
            {
                spriteRenderer.color = Color.Lerp(
                    _originalColor.Value,
                    Color.red,
                    Mathf.PingPong(Time.time * 5f, 1f)
                );
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (_isTwinkling)
            {
                spriteRenderer.color = _originalColor.Value;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider == _limitsTilemap)
            {
                Vector2 normal = collision.contacts[0].normal;

                Vector2 bounce = Vector2.Reflect(_rigidBody2D.velocity, normal);

                _rigidBody2D.MovePosition(_rigidBody2D.position + normal * 0.1f);

                _rigidBody2D.velocity = bounce * 1.5f;
            }
        }
    }
}