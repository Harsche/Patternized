using System;
using System.Collections;
using Unity.Physics;
using UnityEngine;

namespace StatePattern{
    public class RunningPlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            player.Animator.SetBool(Player.IsAimingAnimationParameter, false);
            player.Animator.SetFloat(Player.AimingBlendAnimationParameter, 0f);
            player.Animator.Play("Run Begin", 0, 0);
            yield break;
        }

        public void Update(Player player){
            if (Input.GetKeyDown(KeyCode.Space)){
                player.ChangeState(PlayerState.Jumping);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Shoot();
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            Vector2 velocity = player.Rigidbody.velocity;
            velocity.x = horizontal * player.Speed;
            player.Rigidbody.velocity = velocity;
            if (Mathf.Abs(horizontal) > 0){ player.transform.localScale = new Vector3(horizontal, 1, 1); }

            if (horizontal == 0){ player.ChangeState(PlayerState.Idle); }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            yield break;
        }
    }
}