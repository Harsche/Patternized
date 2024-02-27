using UnityEngine;

namespace StatePattern{
    public class Projectile : MonoBehaviour{
        [SerializeField] private float _speed = 2;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Animator _animator;
        [SerializeField] private CircleCollider2D _collider2D;

        private void OnTriggerEnter2D(Collider2D other){
            if (other.gameObject.CompareTag("Destroyable")){ Destroy(other.gameObject); }
            _rigidbody2D.velocity = Vector2.zero;
            _collider2D.enabled = false;
            _animator.SetTrigger(Animator.StringToHash("Explosion"));
        }

        public void Destroy(){
            Destroy(gameObject);
        }

        public void SetDirection(Vector2 direction){
            _rigidbody2D.velocity = direction * _speed;
        }
    }
}