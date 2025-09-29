using UnityEngine;

namespace PrototypePattern.Horde
{
    [CreateAssetMenu(fileName = "New WaveConfig", menuName = "Patternized/Prototype Pattern/Create WaveConfig", order = 1)]
    public class WaveConfig : ScriptableObject
    {
        [Header("Wave Settings")]
        [SerializeField] private int _enemyCount = 5;
        [SerializeField] private float _spawnInterval = 10f;

        public int EnemyCount => _enemyCount;
        public float SpawnInterval => _spawnInterval;

    }
}
