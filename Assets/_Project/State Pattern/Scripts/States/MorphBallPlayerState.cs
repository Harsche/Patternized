using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class MorphBallPlayerState : IPlayerState{
        public float DirectionOnExitMorphing{ get; set; }

        public IEnumerator Enter(Player player){
            DirectionOnExitMorphing = player.transform.localScale.x;
            player.Animator.SetFloat(Player.AimingBlendAnimationParameter, 0f);
            yield return player.PlayAnimation("To_Morph_Ball");
            yield return player.WaitUntilCurrentAnimationFinishes();
            player.Animator.Play("Morph_Ball_Roll");
        }

        public void Update(Player player){
            if (Input.GetKeyDown(KeyCode.R) && CanExitMorph(player)){
                player.ChangeState(PlayerState.Idle);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space) && player.CheckGround()){
                Vector2 velocity = player.Rigidbody.velocity;
                velocity.y = player.JumpHeight;
                player.Rigidbody.velocity = velocity;
            }
        }

        public void FixedUpdate(Player player){
            float horizontal = Input.GetAxisRaw("Horizontal");

            Vector2 velocity = player.Rigidbody.velocity;
            if (Mathf.Abs(velocity.x) > 0){ DirectionOnExitMorphing = Mathf.Sign(player.Rigidbody.velocity.x); }

            velocity.x = Mathf.MoveTowards(
                velocity.x,
                horizontal * player.Speed,
                player.MorphBallAcceleration * Time.fixedDeltaTime
            );
            player.Rigidbody.velocity = velocity;
        }

        public IEnumerator Exit(Player player){
            player.Animator.Play("From_Morph_Ball", 0, 0);
            yield return player.WaitUntilCurrentAnimationFinishes();
        }

        private bool CanExitMorph(Player player){
            BoxCollider2D box = player.ExitMorphColliderCheck;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(
                (Vector2)player.transform.position + box.offset,
                box.size / 2,
                0f,
                LayerMask.GetMask("Ground")
            );
            return colliders.Length == 0;
        }
    }
}