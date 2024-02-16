using Unity.Mathematics;
using UnityEngine;

namespace FlyweightPattern.ECS{
    public struct CoinType{
        public float Scale{ get; set; }
        public CoinRotation RotationSpeed{ get; set; }
        public CoinPoints Points { get; set; }
        public CoinColor Color { get; set; }

        public CoinType(float scale, float rotationSpeed, int points, float4 color){
            Scale = scale;
            RotationSpeed = new CoinRotation{Value = rotationSpeed};
            Points = new CoinPoints{Points = points};
            Color = new CoinColor{Value = color};
        }
    }
}