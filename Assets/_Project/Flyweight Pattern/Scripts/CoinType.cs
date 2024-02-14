using System;

namespace FlyweightPattern{
    public abstract class CoinType{ }
    
    [Serializable]
    public enum CoinSize{
        Small,
        Normal,
        Big
    }

    [Serializable]
    public enum CoinRareness{
        Common,
        Uncommon,
        Rare
    }
}