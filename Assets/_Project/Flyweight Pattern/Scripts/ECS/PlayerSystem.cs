using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace FlyweightPattern.ECS{
    public partial class PlayerSystem : SystemBase{
        public static float3 PlayerPosition;

        private Camera _camera;
        private float3 _playerStartPosition;
        private Vector3 _velocity;

        protected override void OnCreate(){
            RequireForUpdate<Player>();
        }

        protected override void OnStartRunning(){
            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            _playerStartPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        }

        protected override void OnUpdate(){
            if (_camera == null){ _camera = Camera.main; }

            float3 moveDirection = float3.zero;

            if (_camera != null){
                Transform cameraTransform = _camera.transform;
                float3 cameraForward = cameraTransform.forward;
                float3 cameraRight = cameraTransform.right;
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward = math.normalizesafe(cameraForward);
                cameraRight = math.normalizesafe(cameraRight);
                moveDirection = cameraRight * Input.GetAxisRaw("Horizontal");
                moveDirection += cameraForward * Input.GetAxisRaw("Vertical");
            }

            Entity playerEntity = SystemAPI.GetSingletonEntity<Player>();
            var player = SystemAPI.GetSingleton<Player>();

            var velocity = SystemAPI.GetComponent<PhysicsVelocity>(playerEntity);
            int velocityPoints = World.GetOrCreateSystemManaged<GameSystem>().TotalScore / player.ScorePerSpeedPoint;
            Vector3 targetVelocity = math.normalizesafe(moveDirection) * (player.Speed + velocityPoints);
            velocity.Linear = Vector3.SmoothDamp(
                velocity.Linear,
                targetVelocity,
                ref _velocity,
                player.MovementSmoothing
            );

            EntityManager.SetComponentData(playerEntity, velocity);
            
            PlayerPosition = SystemAPI.GetComponent<LocalTransform>(playerEntity).Position;
        }

        public void ResetPlayerPosition(){
            Entity player = SystemAPI.GetSingletonEntity<Player>();
            EntityManager.SetComponentData(player, LocalTransform.FromPosition(_playerStartPosition));
        }
    }
}