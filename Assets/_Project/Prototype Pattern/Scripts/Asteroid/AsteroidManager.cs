using UnityEngine;
using UnityEngine.Tilemaps;
using PrototypePattern.Asteroids;

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
            BoundsInt bounds = _spaceTilemap.cellBounds;

            int spawned = 0;
            int maxTries = asteroidCount * 10;
            int tries = 0;

            while (spawned < asteroidCount && tries < maxTries)
            {
                tries++;

                int x = Random.Range(bounds.xMin, bounds.xMax);
                int y = Random.Range(bounds.yMin, bounds.yMax);
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                if (_spaceTilemap.HasTile(cellPos))
                {
                    Vector3 spawnPos = _spaceTilemap.CellToWorld(cellPos) + _spaceTilemap.cellSize / 2;

                    int protoIdx = Random.Range(0, _prototypes.Length);
                    Asteroid asteroid = _prototypes[protoIdx].Clone(spawnPos);
                    asteroid.transform.SetParent(_asteroidParent);

                    asteroid.SetLimitTilemap(_limitTilemap);

                    spawned++;
                }
            }
        }
    }
}
