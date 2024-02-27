using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class MorphBallState : IState{
        public float DirectionOnExitMorphing{ get; set; }

        public IEnumerator Enter(Player player){
            DirectionOnExitMorphing = player.transform.localScale.x;
            player.IsAiming = false;
            player.Rigidbody.gravityScale = 0;
            yield return player.PlayAnimation("To_Morph_Ball");
            yield return player.WaitUntilCurrentAnimationFinishes();
            player.Rigidbody.gravityScale = player.InitialGravityScale;
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

            if (!player.CheckGround() && player.Rigidbody.velocity.y <= 0 && horizontal != 0){
                TrySlideMidAir(player, Vector2.right * horizontal);
            }

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
            player.Rigidbody.gravityScale = 0;
            player.Rigidbody.velocity = Vector2.zero;
            yield return player.PlayAnimation("From_Morph_Ball");
            yield return player.WaitUntilCurrentAnimationFinishes();
            player.Rigidbody.gravityScale = player.InitialGravityScale;
        }

        private bool CanExitMorph(Player player){
            BoxCollider2D box = player.ExitMorphColliderCheck;
            Collider2D[] colliders = Physics2D.OverlapBoxAll(
                (Vector2) player.transform.position + box.offset,
                box.size / 2,
                0f,
                LayerMask.GetMask("Ground")
            );
            return colliders.Length == 0;
        }

        private void TrySlideMidAir(Player player, Vector2 direction){
            LayerMask layerMask = LayerMask.GetMask("Ground");

            Vector2 origin1 = player.MorphSlideMidAirRaycast.position;
            Vector2 direction1 = direction;
            float distance1 = player.MorphSlideMidAirRaycastDistance;
            RaycastHit2D hit1 = Physics2D.Raycast(origin1, direction1, distance1, layerMask);
            if (hit1.collider != null){ return; }

            Vector2 origin2 = origin1 + direction1 * distance1;
            Vector2 direction2 = Vector2.down;
            float distance2 = 0.1f;
            RaycastHit2D hit2 = Physics2D.Raycast(origin2, direction2, distance2, layerMask);
            if (hit2.collider == null){ return; }

            player.Rigidbody.MovePosition((Vector2) player.transform.position + direction * 0.05f);
            Vector2 velocity = player.Rigidbody.velocity;
            velocity.y = 0;
            player.Rigidbody.velocity = velocity;
        }
    }
}