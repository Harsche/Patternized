using Unity.Entities;
using UnityEngine;

namespace FlyweightPattern.ECS{
    [AddComponentMenu("Patterns/Flyweight/Player Authoring")]
    public class PlayerAuthoring : MonoBehaviour{
        [Min(1)] public float Speed;
        public int ScorePerSpeedPoint;
        public float MovementSmoothing;

        public class PlayerBaker : Baker<PlayerAuthoring>{
            public override void Bake(PlayerAuthoring authoring){
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Player{
                    Speed = authoring.Speed,
                    ScorePerSpeedPoint = authoring.ScorePerSpeedPoint,
                    MovementSmoothing = authoring.MovementSmoothing
                });
            }
        }
    }
    
    public struct Player : IComponentData{
        public float Speed;
        public int ScorePerSpeedPoint;
        public float MovementSmoothing;
    }
}