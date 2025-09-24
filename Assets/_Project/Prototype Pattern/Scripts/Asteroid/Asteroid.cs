using UnityEngine;

namespace PrototypePattern.Asteroids
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private AsteroidData _asteroidData;

        private Rigidbody2D rigidBody2D;

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
        public void OnBulletHit()
        {
            OnDestroyAsteroid();
            GameObject asteroidFX = Instantiate(_asteroidData.HandleFX(), transform.position, Quaternion.identity);
            Destroy(asteroidFX, 2f);
            Destroy(gameObject);
        }
        private void OnDestroyAsteroid()
        {
            if (_asteroidData == null)
                return;

            float chance = _asteroidData.DropBaseChance;
            if (Random.value < chance)
            {
                Debug.Log("Drop de powerup!");
            }
        }
    }
}
