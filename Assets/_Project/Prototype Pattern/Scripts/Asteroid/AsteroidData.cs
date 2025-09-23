using System;
using UnityEngine;

[Serializable]
public class AsteroidData
{
    [field: SerializeField] public Sprite[] Tiny { get; private set; }
    [field: SerializeField] public Sprite[] Small { get; private set; }
    [field: SerializeField] public Sprite[] Medium { get; private set; }
    [field: SerializeField] public Sprite[] Large { get; private set; }
    [field: SerializeField] public Sprite[] Huge { get; private set; }
    [field: SerializeField] public Sprite[] Giant { get; private set; }
    [field: SerializeField] public Sprite[] Massive { get; private set; }
    [field: SerializeField] public Sprite[] Titanic { get; private set; }
    [field: SerializeField] public Sprite[] Colossal { get; private set; }
    [field: SerializeField] public Sprite[] Mythic { get; private set; }

    [field: SerializeField] public float MinSpeed { get; private set; } = 0.5f;
    [field: SerializeField] public float MaxSpeed { get; private set; } = 2f;
    [field: SerializeField] public float RotationSpeed { get; private set; } = 30f;

    [field: SerializeField, Range(0f, 1f)] public float DropBaseChance { get; private set; } = 0.1f;

    public Sprite[] GetSpriteArray(int index)
    {
        switch (index)
        {
            case 0: return Tiny;
            case 1: return Small;
            case 2: return Medium;
            case 3: return Large;
            case 4: return Huge;
            case 5: return Giant;
            case 6: return Massive;
            case 7: return Titanic;
            case 8: return Colossal;
            case 9: return Mythic;
            default: return Tiny;
        }
    }
}