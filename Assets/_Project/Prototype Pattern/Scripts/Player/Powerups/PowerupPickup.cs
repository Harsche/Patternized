using PrototypePattern.Enemy;
using UnityEngine;

namespace PrototypePattern.Player.Powerups
{
    public enum PowerupType { DualShot, Speed, Intangibility }

    public class PowerupPickup : MonoBehaviour
    {
        [Header("Powerup Settings")]
        public PowerupType powerupType = PowerupType.DualShot;
        public float duration = 6f;
        public float speedMultiplier = 1.5f;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<EnemyController>(out EnemyController enemy)) return;

            if (other.TryGetComponent<PlayerController>(out var player))
            {
                if (powerupType == PowerupType.DualShot)
                {
                    if (player.TryGetComponent<Gun.PlayerGun>(out var playerGun))
                    {
                        playerGun.ActivateDualUntilEmpty();
                        Destroy(gameObject);
                        return;
                    }
                }

                Destroy(gameObject);
            }
        }
    }
}
