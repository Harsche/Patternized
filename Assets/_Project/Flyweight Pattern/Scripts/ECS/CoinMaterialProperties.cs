using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace FlyweightPattern.ECS{
    [MaterialProperty("_Color")]
    public struct CoinColor : IComponentData
    {
        public float4 Value;
    }
    
    [MaterialProperty("_RotationSpeed")]
    public struct CoinRotation : IComponentData
    {
        public float Value;
    }
}