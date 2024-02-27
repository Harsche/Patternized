using UnityEngine;

namespace StatePattern{
    public class Parallax : MonoBehaviour{
        [SerializeField] private Vector2 _scrollSpeed;
        private Transform _camera;
        private Transform _transform;

        private Vector2 _cameraLastPosition;

        private void Start(){
            if (Camera.main != null){
                _camera = Camera.main.transform;
                _cameraLastPosition = _camera.position;
                _transform = transform;
            }
        }

        private void LateUpdate(){
            if (_camera){
                Vector2 offset = (Vector2) _camera.transform.position - _cameraLastPosition;
                Vector2 position = _transform.position;
                position += offset * _scrollSpeed;
                _transform.position = position;
                _cameraLastPosition = _camera.position;
            }
        }
    }
}