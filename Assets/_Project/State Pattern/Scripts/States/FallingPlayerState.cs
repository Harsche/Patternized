using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class FallingPlayerState : IPlayerState{
        public IEnumerator Enter(Player player){
            player.Animator.Play("Fall_Begin");
            yield break;
        }

        public void Update(Player player){
            float horizontal = Input.GetAxisRaw("Horizontal");
            Vector2 velocity = player.Rigidbody.velocity;
            if (Mathf.Abs(horizontal) > 0){
                velocity.x = horizontal * player.Speed;
                player.Rigidbody.velocity = velocity;
                player.transform.localScale = new Vector3(horizontal, 1, 1);
            }


            if (player.CheckGround()){
                player.ChangeState(Mathf.Abs(horizontal) > 0 ? PlayerState.Running : PlayerState.Idle);
            }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            yield break;
        }
    }
}