using UnityEngine;

namespace PrototypePattern.Player.Gun.Bullets
{
    public class FastBullet : BulletBase
    {
        protected override void Awake()
        {
            base.Awake();
            _magazineCapacity = 200;
        }

        protected override void Move(Vector3 direction, Transform target = null)
        {
            _rigidBody2D.velocity = direction * Speed * 1.5f;
        }
    }
}
