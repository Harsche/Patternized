using UnityEngine;

namespace PrototypePattern.Player.Gun.Bullets
{
    public class NormalBullet : BulletBase
    {
        protected override void Move(Vector3 direction, Transform target = null)
        {
            _rigidBody2D.velocity = direction * Speed;
        }
    }
}
