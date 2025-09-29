using UnityEngine;

namespace PrototypePattern.Powerups
{
    public class PowerupDropper : MonoBehaviour
    {
        [Header("Powerup Drop Settings")]
        [SerializeField] private GameObject[] powerupPrefabs;
        [SerializeField, Range(0f, 1f)] private float dropChance = 0.1f;
        private float? forcedDropChance = null;
        public void TryDropPowerup()
        {
            float chance = forcedDropChance ?? dropChance;
            if (powerupPrefabs.Length == 0) return;
            if (Random.value < chance)
            {
                int index = Random.Range(0, powerupPrefabs.Length);
                Instantiate(powerupPrefabs[index], transform.position, Quaternion.identity);
            }
        }
    }
}
