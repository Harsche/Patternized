using System.Collections;
using System.Collections.Generic;
using PrototypePattern;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Enemy
{
    public class EnemyController : MonoBehaviour, IDamageable, IPrototype<EnemyController>
    {
        private float _health;
        private int _hitDamage;
        private int _speed = 5;
        public int _knockbackForce;

        public PlayerController Player;

        public float Health
        {
            get => _health;
            private set
            {
                _health = value;
                if (_health <= 0)
                {
                    OnDeath();
                }
            }
        }

        public bool Targetable { get; }
        public bool Invincible { get; set; }
        private void Awake()
        {
            Player = FindObjectOfType<PlayerController>(); // Ignore this, just testing
        }
        private void Update()
        {
            EnemyChasing();
        }
        public EnemyController Clone()
        {
            EnemyController clone = Instantiate(this);
            return clone;
        }
        public EnemyController Clone(Vector3 position)
        {
            EnemyController clone = Instantiate(this, position, Quaternion.identity);
            return clone;
        }
        private void EnemyChasing()
        {
            if (Player != null)
            {
                Vector2 playerPosition = Player.transform.position;
                transform.position = Vector2.MoveTowards(transform.position, playerPosition, _speed * Time.deltaTime);
            }
        }
        public void OnHit(int damage, Vector2 knockback) { }
        public void OnHit(int damage)
        {
            Health -= damage;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Collider2D collider2D = collision.collider;
            if (collider2D.TryGetComponent(out PlayerController player))
            {
                Vector2 direction = (collider2D.transform.position - transform.position).normalized;
                Vector2 knockback = direction * _knockbackForce;

                player.OnHit(_hitDamage, knockback);
            }
        }

        public void OnDeath()
        {
            Destroy(gameObject);
        }
    }
}