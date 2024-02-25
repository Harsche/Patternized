using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class CrouchingPlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            player.Animator.SetBool(Player.IsCrouchingAnimationParameter, true);
            yield return player.PlayAnimation("Stand_To_Crouch");
            yield return player.WaitUntilCurrentAnimationFinishes();
        }

        public void Update(Player player){
            if (Input.GetAxisRaw("Vertical") > 0){
                player.ChangeState(PlayerState.Idle);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Animator.Play("Crouch_Shoot_Forward", 0, 0);
                player.Shoot();
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0){ player.transform.localScale = new Vector3(horizontal, 1, 1); }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            player.IsAiming = false;
            yield return player.PlayAnimation("Crouch_To_Stand");
            yield return player.WaitUntilCurrentAnimationFinishes();
        }
    }
}