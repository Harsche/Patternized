using PrototypePattern;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Player.Gun.Bullets
{
    /// <summary>
    /// Abstract base class for all bullet types.
    /// Implements common bullet behavior and properties.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BulletBase : MonoBehaviour, IPrototype<BulletBase>
    {
        [Header("Bullet Settings")]
        [SerializeField] protected int _bulletDamage = 10;
        [SerializeField] protected float _speed = 6f;
        [SerializeField] protected float _cooldown = 0.5f;
        [SerializeField] protected float _lifetime = 5f;
        [SerializeField] protected int _magazineCapacity = 6;
        
        protected Rigidbody2D _rigidBody2D;

        // Bullet properties
        public int BulletDamage => _bulletDamage;
        public float Speed => _speed;
        public float Cooldown => _cooldown;
        public float Lifetime => _lifetime;
        public int MagazineCapacity => _magazineCapacity;
        public Rigidbody2D Rigidbody2D => _rigidBody2D;

        protected virtual void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            Destroy(gameObject, _lifetime);
        }

        protected abstract void Move(Vector3 direction, Transform target = null);

        public virtual void Shoot(Vector3 spawnPosition, Vector3 direction, Transform parent, Transform target = null)
        {
            BulletBase clone = Clone(spawnPosition);
            clone.transform.SetParent(parent);
            clone.Move(direction, target);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider2D)
        {
            // Don't hit the player
            if (collider2D.TryGetComponent(out PlayerController player)) return;

            // Hit enemies
            if (collider2D.TryGetComponent(out EnemyController enemy))
            {
                enemy.OnHit(_bulletDamage);
                OnHitTarget(enemy);
            }
        }

        protected virtual void OnHitTarget(EnemyController target)
        {
            Destroy(gameObject);
        }

        #region IPrototype Implementation

        public virtual BulletBase Clone()
        {
            BulletBase clone = Instantiate(this);
            CopyBulletProperties(clone);
            return clone;
        }

        public virtual BulletBase Clone(Vector3 position)
        {
            BulletBase clone = Instantiate(this, position, Quaternion.identity);
            CopyBulletProperties(clone);
            return clone;
        }

        protected virtual void CopyBulletProperties(BulletBase target)
        {
            target._bulletDamage = _bulletDamage;
            target._speed = _speed;
            target._cooldown = _cooldown;
            target._lifetime = _lifetime;
            target._magazineCapacity = _magazineCapacity;
        }

        #endregion
    }
}