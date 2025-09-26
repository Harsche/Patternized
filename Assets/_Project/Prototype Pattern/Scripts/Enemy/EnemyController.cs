using System.Collections;
using System.Collections.Generic;
using PrototypePattern;
using PrototypePattern.Player;
using PrototypePattern.Enemy;
using UnityEngine;
using PrototypePattern.Horde;
using PrototypePattern.Powerups;

namespace PrototypePattern.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PowerupDropper))]
    public class EnemyController : MonoBehaviour, IDamageable, IPrototype<EnemyController>
    {
        public delegate void EnemyDeathHandler();
        private MonsterStatsSO _baseStats;

        private float _health;
        private int _hitDamage;
        private float _speed = 5f;
        private int _knockbackForce;

        [SerializeField] private GameObject _enemyDestructionFX;
        private PlayerController _player;
        private PowerupDropper _powerupDropper;
        public event EnemyDeathHandler OnDeathEvent;
        public enum StatType { Health, Speed, Damage }
        public bool Targetable { get; }
        public bool Invincible { get; set; }

        public float Health
        {
            get => _health;
            private set
            {
                _health = value;
                if (_health <= 0)
                {
                    _powerupDropper.TryDropPowerup();
                    OnDeath();
                }
            }
        }

        private void Awake()
        {
            InitializeFromStats(_baseStats);
            _powerupDropper = GetComponent<PowerupDropper>();
        }
        private void Update()
        {
            EnemyChasing();
        }

        public void InitializeFromStats(MonsterStatsSO stats, PlayerController player = null)
        {
            if (stats == null) return;
            _health = stats.Health;
            _speed = stats.Speed;
            _hitDamage = stats.Damage;
            _knockbackForce = stats.KnockbackForce;
            _player = player;
        }
        public EnemyController Clone()
        {
            EnemyController clone = Instantiate(this);
            clone.CopyRuntimeStatsFrom(this);
            clone._player = this._player;
            return clone;
        }
        public EnemyController Clone(Vector3 position)
        {
            EnemyController clone = Instantiate(this, position, Quaternion.identity);
            clone.CopyRuntimeStatsFrom(this);
            clone._player = this._player;
            return clone;
        }
        private void CopyRuntimeStatsFrom(EnemyController prototype)
        {
            _health = prototype._health;
            _speed = prototype._speed;
            _hitDamage = prototype._hitDamage;
            _knockbackForce = prototype._knockbackForce;
        }
        private void EnemyChasing()
        {
            Vector3 playerPosition = _player.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, _speed * Time.deltaTime);
            Vector3 direction = playerPosition - transform.position;
            if (direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
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
            OnDeathEvent?.Invoke();
            GameObject enemyFXPrefab = Instantiate(_enemyDestructionFX, transform.position, Quaternion.identity);
            Destroy(enemyFXPrefab, 2f);
            Destroy(gameObject);
        }
        public void ApplyModifier(StatType stat, float amount)
        {
            switch (stat)
            {
                case StatType.Health: _health += amount; break;
                case StatType.Speed: _speed += amount; break;
                case StatType.Damage: _hitDamage = Mathf.RoundToInt(_hitDamage + amount); break;
            }
        }
    }
}