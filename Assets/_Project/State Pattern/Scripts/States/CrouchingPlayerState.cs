using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class CrouchingPlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            player.Animator.SetBool(Player.IsCrouchingAnimationParameter, true);
            player.Animator.SetBool(Player.IsAimingAnimationParameter, false);
            player.Animator.SetFloat(Player.AimingBlendAnimationParameter, 0f);
            yield break;
        }

        public void Update(Player player){
            if (Input.GetAxisRaw("Vertical") > 0){
                player.ChangeState(PlayerState.Idle);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Shoot();
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0){ player.transform.localScale = new Vector3(horizontal, 1, 1); }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            player.Animator.SetBool(Player.IsCrouchingAnimationParameter, false);
            yield break;
        }
    }
}