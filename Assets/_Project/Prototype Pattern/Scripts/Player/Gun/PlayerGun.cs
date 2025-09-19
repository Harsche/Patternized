using System.Collections;
using PrototypePattern.Input;
using PrototypePattern.Player.Gun.Bullets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypePattern.Player.Gun
{
    public class PlayerGun : MonoBehaviour
    {
        [Header("Components")]
        private InputHandler _inputHandler;
        private PlayerController _player;

        [Header("Bullet Settings")]
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private Image _aim;

        [Header("Bullet Type")]
        [SerializeField] private BulletBase _currentBullet;
        [SerializeField] private NormalBullet _normalBullet;
        [SerializeField] private FastBullet _fastBullet;

        [Header("Ammo Settings")]
        private AmmoDisplay _ammoDisplay;
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private float _reloadTime = 2f;

        private int _currentAmmo;
        private int _currentMagazineSize;
        private bool _isReloading;
        private float _nextShootTime;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
            _inputHandler = GetComponentInParent<InputHandler>();

            _currentMagazineSize = _currentBullet.MagazineCapacity;
            _currentAmmo = _currentMagazineSize;

            _ammoDisplay = new AmmoDisplay(_currentMagazineSize, _ammoText, _currentAmmo);
        }

        private void Update()
        {
            Aim();
        }

        private void OnEnable()
        {
            _inputHandler.OnAttack += Shoot;
            _inputHandler.OnReload += StartReload;
        }

        private void OnDisable()
        {
            _inputHandler.OnAttack -= Shoot;
            _inputHandler.OnReload -= StartReload;
        }

        private void Aim()
        {
            Vector3 direction = GetMouseDirection();
            _aim.rectTransform.position = _inputHandler.MousePosition;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void Shoot()
        {
            if (_isReloading) return;                  // não dispara durante recarga
            if (_currentAmmo <= 0) return;             // sem munição
            if (Time.time < _nextShootTime) return;    // cooldown

            Vector3 spawnPosition = transform.position;
            Vector3 shootDirection = GetMouseDirection();

            _currentBullet.Shoot(spawnPosition, shootDirection, _bulletParent);

            _currentAmmo--;
            _nextShootTime = Time.time + _currentBullet.Cooldown;

            _ammoDisplay.UpdateAmmo(_currentAmmo);

            if (_currentAmmo <= 0)
                StartCoroutine(Reload());
        }

        private Vector3 GetMouseDirection()
        {
            Vector3 mousePosition = _inputHandler.GetMouseWorldPosition();
            return (mousePosition - _player.transform.position).normalized;
        }

        private void StartReload()
        {
            if (!_isReloading && _currentAmmo < _currentMagazineSize)
                StartCoroutine(Reload());
        }

        private IEnumerator Reload()
        {
            _isReloading = true;
            yield return new WaitForSeconds(_reloadTime);

            _currentAmmo = _currentMagazineSize;
            _isReloading = false;

            _ammoDisplay.UpdateAmmo(_currentAmmo);
        }

        // Trocar tipo de bala
        public void SwitchBulletType(BulletBase newBulletType)
        {
            _currentBullet = newBulletType;
            _currentMagazineSize = newBulletType.MagazineCapacity;

            _currentAmmo = _currentMagazineSize;
            _ammoDisplay = new AmmoDisplay(_currentMagazineSize, _ammoText, _currentAmmo);
        }
    }
}
