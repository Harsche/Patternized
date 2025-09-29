using UnityEngine;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Utilities
{
    public static class TilemapSpawnUtility
    {
        public static Vector3 GetRandomSpawnPosition(Tilemap tilemap, int maxTries = 100, Vector3 offset = default, Camera camera = null)
        {
            BoundsInt bounds = tilemap.cellBounds;
            for (int i = 0; i < maxTries; i++)
            {
                int x = Random.Range(bounds.xMin, bounds.xMax);
                int y = Random.Range(bounds.yMin, bounds.yMax);
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (tilemap.HasTile(cellPos))
                {
                    Vector3 spawnPos = tilemap.CellToWorld(cellPos) + tilemap.cellSize / 2 + offset;
                    spawnPos.z = 0f;
                    if (camera != null)
                    {
                        Vector3 viewportPoint = camera.WorldToViewportPoint(spawnPos);
                        bool insideCamera = viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                                            viewportPoint.y >= 0 && viewportPoint.y <= 1 &&
                                            viewportPoint.z > 0;
                        if (insideCamera) continue;
                    }
                    return spawnPos;
                }
            }
            return tilemap.CellToWorld(Vector3Int.RoundToInt(bounds.center)) + tilemap.cellSize / 2 + offset;
        }
    }
}
