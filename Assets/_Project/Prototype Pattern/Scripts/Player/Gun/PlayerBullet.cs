using UnityEngine;

namespace PrototypePattern.Player.Gun
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerBullet : MonoBehaviour, IPrototype
    {
        [SerializeField] private int _bulletDamage = 1;

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
        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.TryGetComponent(out PlayerController player)) return;

            if (collider2D.TryGetComponent(out EnemyController enemy))
            {
                enemy.OnHit(_bulletDamage);
                Destroy(gameObject);
            }
        }
    }
}