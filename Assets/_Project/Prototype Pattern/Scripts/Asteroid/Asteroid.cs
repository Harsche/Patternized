using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Asteroids
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour, IPrototype<Asteroid>
    {
        [SerializeField] private AsteroidData _asteroidData;
        [SerializeField] private GameObject[] powerupPrefabs;
        private TilemapCollider2D _limitsTilemap;
        private Rigidbody2D rigidBody2D;
        private float? forcedDropChance = null;
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

        void Start()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite sprite = _asteroidData.GetRandomSprite();
            spriteRenderer.sprite = sprite;

            Vector2 direction = Random.insideUnitCircle.normalized;
            float speed = Random.Range(_asteroidData.MinSpeed, _asteroidData.MaxSpeed);
            rigidBody2D.velocity = direction * speed;

            rigidBody2D.angularVelocity = Random.Range(-_asteroidData.RotationSpeed, _asteroidData.RotationSpeed);
        }
        private void Update()
        {
            if (_shouldBounce)
            {
                Vector2 bounce = Vector2.Reflect(rigidBody2D.velocity, _bounceNormal);
                rigidBody2D.MovePosition(rigidBody2D.position + _bounceNormal * 0.5f);
                rigidBody2D.velocity = bounce * 1.5f;
                _shouldBounce = false;
            }
        }
        public void OnBulletHit()
        {
            OnDestroyAsteroid();
            GameObject asteroidFX = Instantiate(_asteroidData.HandleFX(), transform.position, Quaternion.identity);
            Destroy(asteroidFX, 2f);
            Destroy(gameObject);
        }

        private void OnDestroyAsteroid()
        {
            float chance = forcedDropChance ?? _asteroidData.DropBaseChance;

            if (Random.value < chance)
            {
                int index = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[index], transform.position, Quaternion.identity);
            }
        }

        public void OverrideDropChance(float chance)
        {
            forcedDropChance = Mathf.Clamp01(chance);
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
