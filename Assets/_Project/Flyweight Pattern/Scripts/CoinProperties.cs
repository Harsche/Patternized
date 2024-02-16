using System;

namespace FlyweightPattern{

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