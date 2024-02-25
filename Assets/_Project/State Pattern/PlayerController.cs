using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour{
        
        

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpHeight;

        private bool _isGrounded;

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private void Awake(){
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update(){
            // float horizontal = Input.GetAxisRaw("Horizontal");
            // Vector2 velocity = _rigidbody.velocity;
            // velocity.x = horizontal * _speed;
            // _animator.SetFloat(SpeedAnimationParameter, Mathf.Abs(horizontal));
            // if (Mathf.Abs(horizontal) > 0){ transform.localScale = new Vector3(horizontal, 1, 1); }
            //
            // if (Input.GetKeyDown(KeyCode.Space) && _isGrounded){
            //     velocity.y = _jumpHeight;
            //     _animator.SetBool(GroundedAnimationParameter, false);
            //     _animator.SetTrigger(JumpAnimationParameter);
            //     _isGrounded = false;
            // }
            // _rigidbody.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D other){
            // if (other.gameObject.CompareTag("Ground")){
            //     _isGrounded = true;
            //     _animator.SetBool(GroundedAnimationParameter, true);
            // }
        }
    }
}