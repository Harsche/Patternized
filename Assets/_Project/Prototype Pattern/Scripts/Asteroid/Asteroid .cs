using UnityEngine;

namespace PrototypePattern.Asteroid
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private AsteroidData _asteroidData;
        [HideInInspector] public int tamanhoIndex;

        private Rigidbody2D rigidBody2D;

        void Start()
        {
            rigidBody2D = GetComponent<Rigidbody2D>();

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Sprite[] pool = _asteroidData.GetSpriteArray(tamanhoIndex);

            spriteRenderer.sprite = pool[Random.Range(0, pool.Length)];

            Vector2 direction = Random.insideUnitCircle.normalized;
            float speed = Random.Range(_asteroidData.MinSpeed, _asteroidData.MaxSpeed);
            rigidBody2D.velocity = direction * speed;

            rigidBody2D.angularVelocity = Random.Range(-_asteroidData.RotationSpeed, _asteroidData.RotationSpeed);
        }

        public void OnDestroyAsteroid()
        {
            float chance = _asteroidData.DropBaseChance * (tamanhoIndex + 1);
            if (Random.value < chance)
            {
                Debug.Log("Drop de powerup!");
            }
        }
    }
}
