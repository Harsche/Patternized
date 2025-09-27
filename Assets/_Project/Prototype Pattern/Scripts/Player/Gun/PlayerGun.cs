using System.Collections;
using PrototypePattern.Input;
using PrototypePattern.Player.Gun.Bullets;
using PrototypePattern.Player.Powerups;
using PrototypePattern.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypePattern.Player.Gun
{
    public class PlayerGun : MonoBehaviour
    {
        public enum ShotType { Single, Dual }
        [Header("Components")]
        private InputHandler _inputHandler;
        private PlayerController _player;

        [Header("Bullet Settings")]
        [SerializeField] private Transform _bulletParent;
        [SerializeField] private Image _aim;

        [Header("Bullet Type")]
        [SerializeField] private BulletBase _normalBullet;
        [SerializeField] private BulletBase _fastBullet;
        private BulletBase _currentBullet;

        [Header("Muzzles")]
        [SerializeField] private Transform _singleShotOrigin;
        [SerializeField] private Transform _dualShotContainer;
        [SerializeField] private ShotType _defaultShotType = ShotType.Single;

        [Header("Ammo Settings")]
        private AmmoDisplay _ammoDisplay;
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private float _reloadTime = 2f;
        [Header("Powerups")]
        [SerializeField] private float _dualShotSpread = 8f;

        private int _currentAmmo;
        private int _currentMagazineSize;
        private bool _isReloading;
        private float _nextShootTime;
        private bool _dualUntilEmpty = false;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
            _inputHandler = GetComponentInParent<InputHandler>();

            _currentBullet = _normalBullet;
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

            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
            _player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void Shoot()
        {
            if (_isReloading) return;
            if (_currentAmmo <= 0) return;
            if (Time.time < _nextShootTime) return;

            BulletBase previousBullet = _currentBullet;
            if (_dualUntilEmpty || _defaultShotType == ShotType.Dual)
            {
                _currentBullet = _fastBullet;
            }
            else
            {
                _currentBullet = _normalBullet;
            }

            if (previousBullet != _currentBullet)
            {
                _currentMagazineSize = _currentBullet.MagazineCapacity;
                if (_currentAmmo > _currentMagazineSize)
                    _currentAmmo = _currentMagazineSize;
                _ammoDisplay = new AmmoDisplay(_currentMagazineSize, _ammoText, _currentAmmo);
            }
            else
            {
                _currentMagazineSize = _currentBullet.MagazineCapacity;
                if (_currentAmmo > _currentMagazineSize)
                    _currentAmmo = _currentMagazineSize;
            }

            Vector3 spawnPosition = transform.position;
            Vector3 shootDirection = GetMouseDirection();

            if (_currentBullet == _normalBullet)
                FireSingle(spawnPosition, shootDirection);
            else
                FireDual(spawnPosition, shootDirection);

            _nextShootTime = Time.time + _currentBullet.Cooldown;
            _ammoDisplay.UpdateAmmo(_currentAmmo);

            if (_currentAmmo <= 0)
                StartCoroutine(Reload());
        }

        private void FireSingle(Vector3 spawnPosition, Vector3 shootDirection)
        {
            if (_currentAmmo <= 0) return;

            _currentBullet.Shoot(spawnPosition, shootDirection, _bulletParent);

            _currentAmmo--;
            _ammoDisplay.UpdateAmmo(_currentAmmo);
            if (_currentAmmo <= 0 && _dualUntilEmpty)
            {
                _dualUntilEmpty = false;
                PowerupUIManager.Instance.HidePowerupIcon();
            }
        }

        private void FireDual(Vector3 spawnPosition, Vector3 shootDirection)
        {
            if (_currentAmmo <= 0) return;

            if (_currentAmmo == 1)
            {
                FireSingle(spawnPosition, shootDirection);
                return;
            }

            if (_dualShotContainer != null && _dualShotContainer.childCount >= 2)
            {
                Transform muzzleLeft = _dualShotContainer.GetChild(0);
                Transform muzzleRight = _dualShotContainer.GetChild(1);

                Vector3 directionLeft = (muzzleLeft.up == Vector3.zero) ? shootDirection : muzzleLeft.up;
                Vector3 directionRight = (muzzleRight.up == Vector3.zero) ? shootDirection : muzzleRight.up;

                _currentBullet.Shoot(muzzleLeft.position, directionLeft.normalized, _bulletParent);
                _currentBullet.Shoot(muzzleRight.position, directionRight.normalized, _bulletParent);
            }

            _currentAmmo -= 2;
            _ammoDisplay.UpdateAmmo(_currentAmmo);
            if (_currentAmmo <= 0 && _dualUntilEmpty)
            {
                _dualUntilEmpty = false;
                if (PowerupUIManager.Instance != null)
                    PowerupUIManager.Instance.HidePowerupIcon();
            }
        }


        public void ActivateDualUntilEmpty()
        {
            _dualUntilEmpty = true;
            _currentBullet = _fastBullet;
            _currentMagazineSize = _currentBullet.MagazineCapacity;

            _currentAmmo = _currentMagazineSize;

            _ammoDisplay = new AmmoDisplay(_currentMagazineSize, _ammoText, _currentAmmo);
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

            _currentBullet = _normalBullet;
            _currentMagazineSize = _currentBullet.MagazineCapacity;
            _currentAmmo = _currentMagazineSize;
            _ammoDisplay = new AmmoDisplay(_currentMagazineSize, _ammoText, _currentAmmo);
            _isReloading = false;

            _ammoDisplay.UpdateAmmo(_currentAmmo);
        }

    }
}
