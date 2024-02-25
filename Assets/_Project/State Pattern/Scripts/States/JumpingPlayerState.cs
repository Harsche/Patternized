using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class JumpingPlayerState : IPlayerState{
        private int _physicsUpdatesSinceEnter;

        public IEnumerator Enter(Player player){
            _physicsUpdatesSinceEnter = 0;
            player.IsAiming = false;
            string targetAnimation = player.LastState == PlayerState.Idle ? "Jump_Idle_Begin" : "Jump_Run_Begin";
            yield return player.PlayAnimation(targetAnimation);
            yield return new WaitWhile(player.CheckGround);
        }

        public void Update(Player player){
            if (player.CheckWallGrab()){
                player.ChangeState(PlayerState.WallGrab);
                return;
            }
        }

        public void FixedUpdate(Player player){
            _physicsUpdatesSinceEnter++;

            if (_physicsUpdatesSinceEnter <= 2){ return; }

            Vector2 velocity = player.Rigidbody.velocity;

            if (player.CheckGround()){
                PlayerState targetState = Mathf.Abs(velocity.x) > 0 ? PlayerState.Running : PlayerState.Idle;
                player.ChangeState(targetState);
                return;
            }

            if (velocity.y <= 0){
                player.ChangeState(PlayerState.Falling);
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