using UnityEngine;
using UnityEngine.Tilemaps;
using PrototypePattern.Utilities;

namespace PrototypePattern.Asteroids
{
    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _spaceTilemap;
        [SerializeField] private TilemapCollider2D _limitTilemap;
        [SerializeField] private GameObject[] _asteroidPrefabs;
        [SerializeField] private Transform _asteroidParent;
        public int asteroidCount = 20;

        private Asteroid[] _prototypes;

        private void Start()
        {
            _prototypes = new Asteroid[_asteroidPrefabs.Length];
            for (int i = 0; i < _asteroidPrefabs.Length; i++)
            {
                GameObject protoObj = Instantiate(_asteroidPrefabs[i], Vector3.zero, Quaternion.identity, _asteroidParent);
                protoObj.SetActive(false);
                _prototypes[i] = protoObj.GetComponent<Asteroid>();
            }
            SpawnAsteroids();
        }

        private void SpawnAsteroids()
        {
            int spawned = 0;
            int maxTries = asteroidCount * 10;
            while (spawned < asteroidCount)
            {
                Vector3 spawnPos = TilemapSpawnUtility.GetRandomSpawnPosition(_spaceTilemap, maxTries);
                int protoIdx = Random.Range(0, _prototypes.Length);
                Asteroid asteroid = _prototypes[protoIdx].Clone(spawnPos);
                asteroid.transform.SetParent(_asteroidParent);
                asteroid.SetLimitTilemap(_limitTilemap);
                spawned++;
            }
        }
    }
}
