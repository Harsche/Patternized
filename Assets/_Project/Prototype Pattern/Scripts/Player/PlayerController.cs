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

        [Header("Knockback Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float _knockbackDuration = 0.1f;

        [Header("Status Settings")]
        [SerializeField] private bool _targetable = true;
        [SerializeField] private bool _invincible = false;

        [Header("Components")]
        private Collider2D _physicsCollider;
        private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;

        public float Health
        {
            get => _health;
            private set
            {
                _health = value;
                if (_health <= 0)
                {
                    Targetable = false;
                    OnDeath();
                }
            }
        }

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
                _physicsCollider.enabled = !Invincible;
            }
        }

        private void Awake()
        {
            _physicsCollider = GetComponent<Collider2D>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
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
            Debug.Log($"Took {damage} damage!");
            Health -= damage;
        }
        public void OnDeath()
        {
            Debug.Log("Died!");
        }
        private IEnumerator ApplyKnockback(Vector2 knockback, float duration)
        {
            Invincible = true;
            _rigidBody2D.AddForce(knockback, ForceMode2D.Impulse);
            StartCoroutine(HitTwinkle(1f));
            yield return new WaitForSeconds(duration);
            Invincible = false;
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