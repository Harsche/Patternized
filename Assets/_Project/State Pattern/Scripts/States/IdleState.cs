using System;
using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class IdleState : IState{
        public IEnumerator Enter(Player player){
            player.Rigidbody.velocity = Vector2.zero;
            string targetAnimation;

            if (player.IsAiming){ targetAnimation = "Stand Shoot Forward"; }
            else{
                targetAnimation = player.LastState switch{
                    PlayerState.Idle => "Idle_1",
                    PlayerState.Running => "Run Stop",
                    PlayerState.Jumping => "Idle_1",
                    PlayerState.Crouching => "Idle_1",
                    PlayerState.MorphBall => "Idle_1",
                    PlayerState.Falling => "Fall_End",
                    PlayerState.WallGrab => "Idle_1",
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
            player.Animator.Play(targetAnimation);
            yield break;
        }

        public void Update(Player player){
            if (!player.CheckGround()){
                player.ChangeState(PlayerState.Falling);
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Space)){
                player.ChangeState(PlayerState.Jumping);
                return;
            }

            if (Input.GetAxisRaw("Vertical") < 0){
                player.ChangeState(PlayerState.Crouching);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Animator.Play("Stand Shoot Forward");
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

        public IEnumerator Exit(Player player){
            yield break;
        }
    }
}