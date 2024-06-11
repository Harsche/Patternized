using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private float _speed = 3f;

        private void Update()
        {
            Vector3 input = Vector2.zero;

            bool isAttacking = _animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimationParameters.TagAttack);

            if (!isAttacking)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.z = Input.GetAxisRaw("Vertical");
            }


            _rigidbody.velocity = input.normalized * _speed;

            if (input.x != 0) { transform.localScale = new(Mathf.Sign(input.x), 1, 1); }

            UpdateAnimationParameters();
        }

        private void UpdateAnimationParameters()
        {
            _animator.SetFloat(AnimationParameters.Speed, _rigidbody.velocity.magnitude);

            if (Input.GetMouseButtonDown(0)) { _animator.SetTrigger(AnimationParameters.Attack); }
        }

        private static class AnimationParameters
        {
            public static int Speed = Animator.StringToHash("Speed");
            public static int Attack = Animator.StringToHash("Attack");
            public static string TagAttack = "Attack";
        }
    }

}

