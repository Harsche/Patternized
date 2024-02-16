using Unity.Mathematics;
using UnityEngine;

namespace FlyweightPattern{
    [CreateAssetMenu(fileName = "Game Data", menuName = "Patternized/Flyweight Pattern/Game Data", order = 0)]
    public class GameData : ScriptableObject{
        public int MinigameTime;
        public Vector2 ArenaSize;
        public int TotalCoins;
        public GameObject CoinPrefab;
        public float3 CoinScales;
        public float3 CoinRotationSpeeds;
        public int3 CommonCoinPoints;
        public int3 UncommonCoinPoints;
        public int3 RareCoinPoints;
        public Color CommonCoinsColor;
        public Color UncommonCoinsColor;
        public Color RareCoinsColor;
        public float3 RarityChance;
        public float3x3 SizeChance;
    }
}