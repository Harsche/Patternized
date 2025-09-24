using PrototypePattern.Enemy;
using PrototypePattern.Asteroids;
using UnityEngine;

namespace PrototypePattern.Player.Gun.Bullets
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class BulletBase : MonoBehaviour
    {
        [Header("Bullet Settings")]
        [SerializeField] protected int _bulletDamage = 10;
        [SerializeField] protected float _speed = 6f;
        [SerializeField] protected float _cooldown = 0.5f;
        [SerializeField] protected float _lifetime = 5f;
        [SerializeField] protected int _magazineCapacity = 6;

        protected Rigidbody2D _rigidBody2D;

        public int BulletDamage => _bulletDamage;
        public float Speed => _speed;
        public float Cooldown => _cooldown;
        public float Lifetime => _lifetime;
        public int MagazineCapacity => _magazineCapacity;

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
            BulletBase clone = Instantiate(this, spawnPosition, Quaternion.identity);

            if (direction != Vector3.zero) clone.transform.up = direction;

            clone.transform.SetParent(parent);

            clone.Move(direction, target);
        }


        protected virtual void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent(out PlayerController _)) return;

            if (collider2D.TryGetComponent(out EnemyController enemy))
            {
                enemy.OnHit(_bulletDamage);
                OnHitTarget(enemy);
                Destroy(gameObject);
            }

            if (collider2D.TryGetComponent(out Asteroid asteroid))
            {
                asteroid.OnBulletHit();
                Destroy(gameObject);
            }
        }

        protected virtual void OnHitTarget(EnemyController target)
        {
            Destroy(gameObject);
        }
    }
}
