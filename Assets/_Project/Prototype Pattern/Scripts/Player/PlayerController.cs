using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypePattern.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private float _health;
        [SerializeField] private bool _targetable = true;
        [SerializeField] private bool _invencible = false;
        [SerializeField] private Collider2D _physicsCollider;

        [SerializeField] private Rigidbody2D _rigidBody2D;
        private InputHandler _inputHandler;

        public float Health
        {
            get => _health;
            set
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
            set
            {
                _targetable = value;
            }
        }

        public bool Invencible
        {
            get => _invencible;
            set
            {
                _invencible = value;
                _physicsCollider.enabled = !Invencible; 
            }
        }

        private void Start()
        {
            _physicsCollider = GetComponent<Collider2D>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<InputHandler>();
        }

        public void OnDeath()
        {
            Debug.Log("Morreu!");
        }

        public void OnHit(int damage, Vector2 knockback)
        {
            if (Targetable && !Invencible)
            {
                Health -= damage;
                _inputHandler.ApplyKnockback(knockback, 0.1f);
            }
        }

        public void OnHit(int damage)
        {
            if (Targetable && !Invencible)
            {
                Debug.Log($"Tomou {damage} de dano!");
                Health -= damage;
            }
        }
    }
}