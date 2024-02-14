using UnityEngine;

namespace FlyweightPattern{
    public class Player : MonoBehaviour{
        [SerializeField, Min(1)] private float _speed = 5;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Game _game;

        public void Update(){
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = Input.GetAxisRaw("Horizontal");
            moveDirection.z = Input.GetAxisRaw("Vertical");

            _characterController.SimpleMove(moveDirection.normalized * _speed);
        }

        private void OnTriggerEnter(Collider other){
            if (!other.CompareTag("Collectable")){ return; }
            int index = other.GetComponent<FlyweightCoin>().GetCoinTypeIndex();
            _game.CollectCoin(index);
            Destroy(other.gameObject);
        }
    }
}