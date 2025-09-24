using System;
using System.Collections;
using PrototypePattern.Input;
using UnityEngine;

namespace PrototypePattern.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(InputHandler))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
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
        private Collider2D _physicsCollider;
        private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;

        // Event: passes normalized health (0..1)
        public event Action<float> OnHealthChanged;

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
            Debug.Log("Died!");
        }
        private IEnumerator ApplyKnockback(Vector2 knockback, float duration)
        {
            Invincible = true;
            CanMove = false;
            _rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);
            StartCoroutine(HitTwinkle(1f));
            yield return new WaitForSeconds(duration);
            Invincible = false;
            CanMove = true;
        }
        private IEnumerator HitTwinkle(float timer)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Color originalColor = spriteRenderer.color;
            float elapsedTime = 0f;

            while (elapsedTime <= timer)
            {
                spriteRenderer.color = Color.Lerp(originalColor, Color.red, Mathf.PingPong(Time.time * 5f, 1f));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            spriteRenderer.color = originalColor;
        }

    }
}