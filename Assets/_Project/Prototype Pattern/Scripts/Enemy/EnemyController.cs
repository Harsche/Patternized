using System.Collections;
using System.Collections.Generic;
using PrototypePattern;
using PrototypePattern.Player;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable, IPrototype
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
    public GameObject Clone()
    {
        GameObject clone = Instantiate(gameObject);
        return clone;
    }
    public GameObject Clone(Vector3 position)
    {
        GameObject clone = Instantiate(gameObject, position, Quaternion.identity);
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
