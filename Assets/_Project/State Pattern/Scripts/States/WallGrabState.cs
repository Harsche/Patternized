﻿using System.Collections;
using UnityEngine;

namespace StatePattern{
    public class WallGrabState : IPlayerState{
        private float _wallDirection;
        
        public IEnumerator Enter(Player player){
            Transform playerTransform = player.transform;
            Vector3 playerScale = playerTransform.localScale;
            
            _wallDirection = playerScale.x;
            player.Rigidbody.gravityScale = 0;
            player.Rigidbody.velocity = Vector2.zero;
            
            playerScale.x *= -1;
            playerTransform.localScale = playerScale;
            player.Animator.Play("Wall_Grab_Aim_Down");
            yield break;
        }

        public void Update(Player player){
            if (Input.GetAxisRaw("Vertical") < 0){ player.ChangeState(PlayerState.Falling); }
            
            float horizontal = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space)){
                player.Jump();
                
            }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            player.Rigidbody.gravityScale = player.InitialGravityScale;
            yield break;
        }
    }
}