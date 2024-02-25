using UnityEngine;

namespace StatePattern{
    public class Projectile : MonoBehaviour{
        [SerializeField] private float _speed = 2;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        
        public void SetDirection(Vector2 direction){
            _rigidbody2D.velocity = direction * _speed;
        }
    }
}