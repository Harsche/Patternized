using PrototypePattern.Input;
using Unity.Entities.UniversalDelegates;
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
        [SerializeField] private PlayerBullet _bulletPrefab;
        [SerializeField] private float _bulletSpeed = 25f;

        [SerializeField] private Image _aim;

        private void Awake()
        {
            _player = GetComponentInParent<PlayerController>();
            _inputHandler = GetComponentInParent<InputHandler>();
        }
        private void Update()
        {
            Aim();
        }
        private void OnEnable()
        {
            _inputHandler.OnAttack += Shoot;
        }
        private void OnDisable()
        {
            _inputHandler.OnAttack -= Shoot;
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
            Vector3 spawnPosition = gameObject.transform.position;
            GameObject bullet = _bulletPrefab.Clone(spawnPosition);
            bullet.transform.SetParent(_bulletParent);

            Vector3 shootDirection = GetMouseDirection();
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bulletRB.velocity = shootDirection * _bulletSpeed;
        }
        private Vector3 GetMouseDirection()
        {
            Vector3 mousePosition = _inputHandler.GetMouseWorldPosition();
            return (mousePosition - _player.transform.position).normalized;
        }
    }
}