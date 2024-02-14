using System;
using UnityEngine;

namespace FlyweightPattern{
    [Serializable]
    public class CoinTypeFlyweight : CoinType{
        public float Scale{ get; private set; }
        public float RotationSpeed{ get; private set; }
        public int Points{ get; private set; }
        public Color Color{ get; private set; }

        public CoinTypeFlyweight(CoinRareness rareness, CoinSize size){
            // Probably best done by exposing values to editor
            Scale = size switch{
                CoinSize.Small => 0.3f,
                CoinSize.Normal => 0.5f,
                CoinSize.Big => 1f,
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };

            RotationSpeed = size switch{
                CoinSize.Small => 360f,
                CoinSize.Normal => 180f,
                CoinSize.Big => 30f,
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };

            Color = rareness switch{
                CoinRareness.Common => Color.white,
                CoinRareness.Uncommon => Color.yellow,
                CoinRareness.Rare => Color.blue,
                _ => throw new ArgumentOutOfRangeException(nameof(rareness), rareness, null)
            };

            Points = rareness switch{
                CoinRareness.Common => size switch{
                    CoinSize.Small => 1,
                    CoinSize.Normal => 2,
                    CoinSize.Big => 3,
                    _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
                },
                CoinRareness.Uncommon => size switch{
                    CoinSize.Small => 3,
                    CoinSize.Normal => 5,
                    CoinSize.Big => 10,
                    _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
                },
                CoinRareness.Rare => size switch{
                    CoinSize.Small => 8,
                    CoinSize.Normal => 15,
                    CoinSize.Big => 25,
                    _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(rareness), rareness, null)
            };
        }
    }

    
}