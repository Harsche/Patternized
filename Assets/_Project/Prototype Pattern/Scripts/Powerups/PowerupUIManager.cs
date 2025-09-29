using UnityEngine;
using UnityEngine.UI;

namespace PrototypePattern.Player.Powerups
{
    public class PowerupUIManager : MonoBehaviour
    {
        public static PowerupUIManager Instance { get; private set; }
        [SerializeField] private Image powerupIcon;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            HidePowerupIcon();
        }
        public void ShowPowerupIcon(Sprite icon)
        {
            powerupIcon.sprite = icon;
            powerupIcon.enabled = true;
        }

        public void HidePowerupIcon()
        {
            powerupIcon.enabled = false;
        }
    }
}