using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class JumpingPlayerState : IPlayerState{
        private int _physicsUpdatesSinceEnter;

        public IEnumerator Enter(Player player){
            _physicsUpdatesSinceEnter = 0;
            string targetAnimation = player.LastState == PlayerState.Idle ? "Jump_Idle_Begin" : "Jump_Run_Begin";
            player.Animator.Play(targetAnimation);
            player.Animator.SetBool(Player.IsAimingAnimationParameter, false);
            player.Animator.SetFloat(Player.AimingBlendAnimationParameter, 0f);
            yield break;
        }

        public void Update(Player player){ }

        public void FixedUpdate(Player player){
            _physicsUpdatesSinceEnter++;

            if (_physicsUpdatesSinceEnter <= 2){ return; }

            Vector2 velocity = player.Rigidbody.velocity;

            if (player.CheckGround()){
                PlayerState targetState = Mathf.Abs(velocity.x) > 0 ? PlayerState.Running : PlayerState.Idle;
                player.ChangeState(targetState);
                return;
            }
            
            if (player.CheckWallGrab()){
                player.ChangeState(PlayerState.WallGrab);
                return;
            }

            if (velocity.y <= 0){
                player.ChangeState(PlayerState.Falling);
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0){
                velocity.x = horizontal * player.Speed;
                player.Rigidbody.velocity = velocity;
                player.transform.localScale = new Vector3(horizontal, 1, 1);
            }
        }

        public IEnumerator Exit(Player player){
            yield break;
        }
    }
}