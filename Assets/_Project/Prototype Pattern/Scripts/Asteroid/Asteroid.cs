using PrototypePattern.Powerups;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Asteroids
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PowerupDropper))]
    public class Asteroid : MonoBehaviour, IPrototype<Asteroid>
    {
        [Header("Asteroid Data")]
        [SerializeField] private AsteroidData _asteroidData;

        [Header("References")]
        private TilemapCollider2D _limitsTilemap;
        private Rigidbody2D _rigidBody2D;
        private SpriteRenderer _spriteRenderer;
        private PowerupDropper _powerupDropper;

        [Header("Bounce Settings")]
        private bool _shouldBounce = false;
        private Vector2 _bounceNormal;

        public Asteroid Clone()
        {
            return Clone(transform.position);
        }

        public Asteroid Clone(Vector3 position)
        {
            Asteroid clone = Instantiate(this, position, Quaternion.identity, transform.parent);
            clone.gameObject.SetActive(true);
            return clone;
        }
        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _powerupDropper = GetComponent<PowerupDropper>();
        }

        private void Start()
        {

            Sprite sprite = _asteroidData.GetRandomSprite();
            _spriteRenderer.sprite = sprite;

            Vector2 direction = Random.insideUnitCircle.normalized;
            float speed = Random.Range(_asteroidData.MinSpeed, _asteroidData.MaxSpeed);
            _rigidBody2D.velocity = direction * speed;

            _rigidBody2D.angularVelocity = Random.Range(-_asteroidData.RotationSpeed, _asteroidData.RotationSpeed);
        }
        private void Update()
        {
            if (_shouldBounce)
            {
                BounceAsteroid();
            }
        }
        private void BounceAsteroid()
        {
            Vector2 bounce = Vector2.Reflect(_rigidBody2D.velocity, _bounceNormal);
            _rigidBody2D.MovePosition(_rigidBody2D.position + _bounceNormal * 0.5f);
            _rigidBody2D.velocity = bounce * 1.5f;
            _shouldBounce = false;
        }
        public void OnBulletHit()
        {
            GameObject asteroidFX = Instantiate(_asteroidData.HandleFX(), transform.position, Quaternion.identity);
            _powerupDropper.TryDropPowerup();
            Destroy(asteroidFX, 2f);
            Destroy(gameObject);
        }

        public void SetLimitTilemap(TilemapCollider2D tilemapCollider2D) => _limitsTilemap = tilemapCollider2D;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider == _limitsTilemap)
            {
                _shouldBounce = true;
                _bounceNormal = collision.contacts[0].normal;
            }
        }
    }
}
