using System;
using UnityEngine;

namespace PrototypePattern.Asteroids
{
    [Serializable]
    public class AsteroidData
    {
        public enum AsteroidColor { None = 0, Brown, Grey }
        public enum AsteroidSize { Tiny = 0, Small, Medium, Big }
        [field: SerializeField] public AsteroidColor CurrentColor { get; private set; } = AsteroidColor.None;
        [field: SerializeField] public AsteroidSize CurrentSize { get; private set; } = AsteroidSize.Tiny;

        [field: SerializeField, Header("Sprites")] public Sprite[] BrownSprites { get; private set; }
        [field: SerializeField] public Sprite[] GreySprites { get; private set; }

        [Header("Effects")]
        [SerializeField] private GameObject _brownDestructionEffect;
        [SerializeField] private GameObject _grayDestructionEffect;

        [field: SerializeField, Header("Properties")] public float MinSpeed { get; private set; } = 0.5f;
        [field: SerializeField] public float MaxSpeed { get; private set; } = 2f;
        [field: SerializeField] public float RotationSpeed { get; private set; } = 30f;

        [field: SerializeField, Range(0f, 1f)] public float DropBaseChance { get; private set; } = 0.1f;

        public Sprite GetRandomSprite()
        {
            Sprite[] sprites;
            if (UnityEngine.Random.value < 0.5f)
            {
                sprites = BrownSprites;
                CurrentColor = AsteroidColor.Brown;
            }
            else
            {
                sprites = GreySprites;
                CurrentColor = AsteroidColor.Grey;
            }
            return sprites[UnityEngine.Random.Range(0, sprites.Length)];
        }
        public GameObject HandleFX()
        {
            GameObject effect = CurrentColor == AsteroidColor.Brown ? _brownDestructionEffect : _grayDestructionEffect;
            ChangeSize(effect);
            return effect;
        }
        public void ChangeSize(GameObject gameObject)
        {
            float scale = 1f;
            switch (CurrentSize)
            {
                case AsteroidSize.Tiny: scale = 0.25f; break;
                case AsteroidSize.Small: scale = 0.50f; break;
                case AsteroidSize.Medium: scale = 0.75f; break;
                case AsteroidSize.Big: scale = 1f; break;
            }

            gameObject.transform.localScale = Vector3.one * scale;
        }
    }
}