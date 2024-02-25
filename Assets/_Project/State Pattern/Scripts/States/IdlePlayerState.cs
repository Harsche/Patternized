using System;
using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class IdlePlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            player.Rigidbody.velocity = Vector2.zero;
            string targetAnimation = player.LastState switch{
                PlayerState.Idle => "Idle_1",
                PlayerState.Running => "Run Stop",
                PlayerState.Jumping => "Idle_1",
                PlayerState.Crouching => "Idle_1",
                PlayerState.MorphBall => "Idle_1",
                PlayerState.Falling => "Fall_End",
                _ => throw new ArgumentOutOfRangeException()
            };
            player.Animator.Play(targetAnimation);
            player.Animator.SetBool(Player.IsAimingAnimationParameter, false);
            player.Animator.SetFloat(Player.AimingBlendAnimationParameter, 0f);
            yield break;
        }

        public void Update(Player player){
            if (Input.GetKeyDown(KeyCode.Space)){
                player.ChangeState(PlayerState.Jumping);
                return;
            }

            if (Input.GetAxisRaw("Vertical") < 0){
                player.ChangeState(PlayerState.Crouching);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Shoot();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.R)){
                player.ChangeState(PlayerState.MorphBall);
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0){ player.ChangeState(PlayerState.Running); }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){yield break; }
    }
}