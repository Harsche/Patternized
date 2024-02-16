using FlyweightPattern.ECS;
using UnityEngine;

namespace FlyweightPattern{
    public class CinemachineTarget : MonoBehaviour{
        [SerializeField] private float _smoothing;

        private Vector3 _velocity;

        private void Update(){
            transform.position = Vector3.SmoothDamp(
                transform.position,
                PlayerSystem.PlayerPosition,
                ref _velocity,
                _smoothing
            );
        }
    }
}