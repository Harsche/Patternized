using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace FlyweightPattern.ECS{
    [AddComponentMenu("Patterns/Flyweight/Game Authoring")]
    public class GameAuthoring : MonoBehaviour{
        public GameData GameData;

        public class GameBaker : Baker<GameAuthoring>{
            public override void Bake(GameAuthoring authoring){
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity,
                    new Game{
                        MinigameTime = authoring.GameData.MinigameTime,
                        ArenaSize = authoring.GameData.ArenaSize,
                        TotalCoins = authoring.GameData.TotalCoins,
                        Origin = authoring.transform.position,
                        CoinPrefab = GetEntity(authoring.GameData.CoinPrefab, TransformUsageFlags.Renderable),
                        CoinScales = authoring.GameData.CoinScales,
                        CoinRotationSpeeds = authoring.GameData.CoinRotationSpeeds,
                        CommonCoinPoints = authoring.GameData.CommonCoinPoints,
                        UncommonCoinPoints = authoring.GameData.UncommonCoinPoints,
                        RareCoinPoints = authoring.GameData.RareCoinPoints,
                        CommonCoinsColor = ColorToFloat4(authoring.GameData.CommonCoinsColor),
                        UncommonCoinsColor = ColorToFloat4(authoring.GameData.UncommonCoinsColor),
                        RareCoinsColor = ColorToFloat4(authoring.GameData.RareCoinsColor),
                        RarityChance = authoring.GameData.RarityChance,
                        SizeChance = authoring.GameData.SizeChance
                    });
            }

            private static float4 ColorToFloat4(Color color){
                return new float4(color.r, color.g, color.b, color.a);
            }
        }
    }


    public struct Game : IComponentData{
        public int MinigameTime;
        public float2 ArenaSize;
        public float3 Origin;
        public int TotalCoins;
        public Entity CoinPrefab;
        public float3 CoinScales;
        public float3 CoinRotationSpeeds;
        public int3 CommonCoinPoints;
        public int3 UncommonCoinPoints;
        public int3 RareCoinPoints;
        public float4 CommonCoinsColor;
        public float4 UncommonCoinsColor;
        public float4 RareCoinsColor;
        public float3 RarityChance;
        public float3x3 SizeChance;
    }
}