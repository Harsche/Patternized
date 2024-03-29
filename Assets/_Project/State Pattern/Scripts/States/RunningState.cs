﻿using System;
using System.Collections;
using Unity.Physics;
using UnityEngine;

namespace StatePattern{
    public class RunningState : IState{
        public IEnumerator Enter(Player player){
            player.IsAiming = false;
            player.Animator.Play("Run Begin", 0, 0);
            yield break;
        }

        public void Update(Player player){
            if (!player.CheckGround()){
                player.StartCoroutine(FallExitThreshold(player));
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space)){
                player.ChangeState(PlayerState.Jumping);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftShift)){
                player.Shoot();
                return;
            }

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(horizontal) > 0){
                Vector2 velocity = player.Rigidbody.velocity;
                velocity.x = horizontal * player.Speed;
                player.Rigidbody.velocity = velocity;
                player.transform.localScale = new Vector3(horizontal, 1, 1);
            }
            else{ player.ChangeState(PlayerState.Idle); }
        }

        public void FixedUpdate(Player player){ }

        public IEnumerator Exit(Player player){
            yield break;
        }

        private IEnumerator FallExitThreshold(Player player){
            for (float f = 0; f < 0.1f; f += Time.deltaTime){
                if (player.CheckGround()){ yield break; }
                yield return null;
            }
            player.ChangeState(PlayerState.Falling);
        }
    }
}