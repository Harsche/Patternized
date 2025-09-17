using System.Collections;
using System.Collections.Generic;
using PrototypePattern;
using PrototypePattern.Player;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    [SerializeField] private float _health;

    [SerializeField] private int _hitDamage;
    [SerializeField] private int _speed;
    [SerializeField] private int _knockbackForce;
    [SerializeField] private PlayerController _player;

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

    private void Update()
    {
        if (_player != null)
        {
            Vector2 playerPosition = _player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, _speed * Time.deltaTime);
        }
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

    public void OnHit(int damage, Vector2 knockback)
    {}

    public void OnHit(int damage)
    {
        Health -= damage;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
