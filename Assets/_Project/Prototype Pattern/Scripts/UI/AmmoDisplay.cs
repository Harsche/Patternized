using TMPro;
using UnityEngine;

namespace PrototypePattern.UI
{
    public class AmmoDisplay
    {
        private TMP_Text _ammoText;

        private int _currentAmmo;
        private int _maxAmmo;

        public AmmoDisplay(int max, TMP_Text ammoText, int current = 0)
        {
            _maxAmmo = max;
            _currentAmmo = current;
            _ammoText = ammoText;
            UpdateDisplay();
        }

        public void UpdateAmmo(int current)
        {
            _currentAmmo = Mathf.Clamp(current, 0, _maxAmmo);
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            _ammoText.text = _currentAmmo == 0 ? "Reloading..." : $"{_currentAmmo}/{_maxAmmo}";
        }
    }
}