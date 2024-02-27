using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class WallGrabState : IState{

        public IEnumerator Enter(Player player){
            Transform playerTransform = player.transform;
            Vector3 playerScale = playerTransform.localScale;
            
            player.Rigidbody.gravityScale = 0;
            player.Rigidbody.velocity = Vector2.zero;
            
            playerScale.x *= -1;
            playerTransform.localScale = playerScale;
            player.Animator.Play("Wall Grab");
            yield break;
        }

        public void Update(Player player){
            if (Input.GetAxisRaw("Vertical") < 0){ player.ChangeState(PlayerState.Falling); }
            
            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Shoot();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                player.Jump();
            }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            player.IsAiming = false;
            player.Rigidbody.gravityScale = player.InitialGravityScale;
            yield break;
        }
    }
}