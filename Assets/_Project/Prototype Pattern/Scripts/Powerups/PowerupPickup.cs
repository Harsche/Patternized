using UnityEngine;
using PrototypePattern.Player;
using PrototypePattern.Horde;
using System.Collections;
using PrototypePattern.Player.Gun;

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
            if (!other.TryGetComponent<PlayerController>(out var player)) return;

            HordeMessageUI messageUI = FindObjectOfType<HordeMessageUI>();
            float messageTimer = 1f;

            switch (powerupType)
            {
                case PowerupType.DualShot:
                    PlayerGun playerGun = player.GetComponentInChildren<PlayerGun>();
                    duration = 9999f;
                    playerGun.ActivateDualUntilEmpty();
                    messageUI?.ShowMessage("Dual Shot activated!", messageTimer);
                    break;

                case PowerupType.Speed:
                    if (player.TryGetComponent<PlayerMovement>(out var movement))
                    {
                        movement.StartSpeedBoost(speedMultiplier, duration);
                        messageUI?.ShowMessage($"Speed Boost activated! ({speedMultiplier}x)", messageTimer);
                    }
                    break;

                case PowerupType.Intangibility:
                    player.StartCoroutine(ApplyIntangibility(player));
                    messageUI?.ShowMessage("Invincibility activated!", messageTimer);
                    break;
            }

            Destroy(gameObject);
        }

        private IEnumerator ApplyIntangibility(PlayerController player)
        {
            player.Invincible = true;
            yield return new WaitForSeconds(duration);
            player.Invincible = false;
        }
    }
}
