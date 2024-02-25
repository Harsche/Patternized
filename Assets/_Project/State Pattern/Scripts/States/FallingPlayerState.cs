using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class FallingPlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            if (player.LastState == PlayerState.Jumping){
                while (!player.CheckGround()){
                    AnimatorStateInfo stateInfo = player.Animator.GetCurrentAnimatorStateInfo(0);
                    if (stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime) >= 0.9f){ break; }
                    player.ApplyMoveInput(true);
                    yield return null;
                }
                yield return player.PlayAnimation("Jump_End");
            }
            else{ player.Animator.Play("Fall_Begin"); }
        }

        public void Update(Player player){
            float horizontal = Input.GetAxisRaw("Horizontal");

            if (player.CheckGround()){
                player.ChangeState(Mathf.Abs(horizontal) > 0 ? PlayerState.Running : PlayerState.Idle);
                return;
            }

            if (player.CheckWallGrab()){
                player.ChangeState(PlayerState.WallGrab);
                return;
            }

            Vector2 velocity = player.Rigidbody.velocity;
            if (Mathf.Abs(horizontal) > 0){
                velocity.x = horizontal * player.Speed;
                player.Rigidbody.velocity = velocity;
                player.transform.localScale = new Vector3(horizontal, 1, 1);
            }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            yield break;
        }
    }
}