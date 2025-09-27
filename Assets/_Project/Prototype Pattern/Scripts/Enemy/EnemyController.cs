using UnityEngine;
using PrototypePattern.Player;
using PrototypePattern.Powerups;
using System;

namespace PrototypePattern.Enemy
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PowerupDropper))]
    public class EnemyController : MonoBehaviour, IDamageable, IPrototype<EnemyController>
    {
        [SerializeField] private MonsterStatsSO _stats;
        [SerializeField] private GameObject _enemyDestructionFX;

        // Runtime stats
        private float _health;
        private float _speed;
        private int _damage;
        private int _knockbackForce;

        private PlayerController _player;
        private PowerupDropper _powerupDropper;

        public bool Invincible { get; set; }
        public event Action OnDeathEvent;

        public enum StatType { Health, Speed, Damage }

        public float Health => _health;
        public bool Targetable => !Invincible;

        [SerializeField] private Sprite[] _sprites;

        private void Awake()
        {
            _powerupDropper = GetComponent<PowerupDropper>();
            ApplyStats(_stats);
        }

        private void Update()
        {
            MoveTowardsPlayer();
        }

        public void SetPlayer(PlayerController player)
        {
            _player = player;
        }

        private void MoveTowardsPlayer()
        {
            Vector3 direction = _player.transform.position - transform.position;
            transform.position += direction.normalized * _speed * Time.deltaTime;

            if (direction.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }

        public void OnHit(int damage, Vector2 knockback)
        {
            _health -= damage;
            if (_health <= 0) OnDeath();
            if (_player != null)
            {
                _player.GetComponent<Rigidbody2D>()?.AddForce(knockback);
            }
        }

        public void OnHit(int damage)
        {
            _health -= damage;
            if (_health <= 0) OnDeath();
        }

        private void OnEnable()
        {
            ApplyStats(_stats);
        }

        private void OnDisable()
        {
            OnDeathEvent = null;
        }

        public void OnDeath()
        {
            _powerupDropper.TryDropPowerup();
            OnDeathEvent?.Invoke();

            if (_enemyDestructionFX != null)
            {
                GameObject fx = Instantiate(_enemyDestructionFX, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
            }

            Destroy(gameObject);
        }

        public EnemyController Clone()
        {
            return Clone(transform.position);
        }

        public EnemyController Clone(Vector3 position)
        {
            EnemyController clone = Instantiate(this, position, Quaternion.identity);
            clone._stats = _stats;
            clone.ApplyStats(_stats);
            GetRandomSprite(clone);
            return clone;
        }
        private void GetRandomSprite(EnemyController enemy)
        {
            SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = enemy._sprites[UnityEngine.Random.Range(0, enemy._sprites.Length)];
        }

        private void ApplyStats(MonsterStatsSO stats)
        {
            if (stats == null) return;

            _health = stats.Health;
            _speed = stats.Speed;
            _damage = stats.Damage;
            _knockbackForce = stats.KnockbackForce;
        }

        public void ApplyModifier(StatType stat, float amount)
        {
            switch (stat)
            {
                case StatType.Health: _health += amount; break;
                case StatType.Speed: _speed += amount; break;
                case StatType.Damage: _damage += Mathf.RoundToInt(amount); break;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out PlayerController player))
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                Vector2 knockback = direction * _knockbackForce;
                player.OnHit(_damage, knockback);
            }
        }
    }
}
