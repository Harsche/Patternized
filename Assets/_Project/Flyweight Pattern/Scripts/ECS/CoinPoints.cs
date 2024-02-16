using Unity.Entities;

namespace FlyweightPattern.ECS{
    public struct CoinPoints : ISharedComponentData{
        public int Points;
    }
}