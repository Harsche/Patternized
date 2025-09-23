using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Asteroid
{
    public class AsteroidManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _spaceTilemap;
        [SerializeField] private GameObject _asteroidPrefab;
        [SerializeField] private Transform _asteroidParent;
        public int asteroidCount = 20;

        void Start()
        {
            SpawnAsteroids();
        }

        void SpawnAsteroids()
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

                    GameObject asteroid = Instantiate(_asteroidPrefab, spawnPos, Quaternion.identity);
                    asteroid.transform.SetParent(_asteroidParent);

                    Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();
                    asteroidScript.tamanhoIndex = Random.Range(0, 10);

                    spawned++;
                }
            }
        }
    }
}