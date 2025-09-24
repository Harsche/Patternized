using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Horde
{
    [RequireComponent(typeof(HordeMessageUI))]
    public class HordeManager : MonoBehaviour
    {
        [SerializeField] private List<HordeLevelSO> _hordeLevels;
        [SerializeField] private Transform _hordeParent;

        private EnemyController _prototype;
        [SerializeField] private PlayerController _player;

        private int _currentWave = 0;

        private HordeMessageUI _hordeMessageUI;

        /// <summary>Initializes the HordeManager and starts the first wave coroutine.</summary>
        private void Start()
        {
            _hordeMessageUI = GetComponent<HordeMessageUI>();

            StartCoroutine(StartWaveCoroutine(_currentWave));
        }

        private IEnumerator StartWaveCoroutine(int waveIndex)
        {
            if (waveIndex >= _hordeLevels.Count)
            {
                _hordeMessageUI.ShowMessage("All waves completed!");
                _hordeMessageUI.ChangeTextAlignment(TMPro.TextAlignmentOptions.Center);
                yield break;
            }

            // pré-contagem antes da wave — usar apenas ShowMessage por segundo
            int preCountdown = 5;
            for (int timer = preCountdown; timer > 0; timer--)
            {
                _hordeMessageUI.ShowMessage($"{timer}", 0.9f);
                yield return new WaitForSeconds(1f);
            }

            HordeLevelSO level = _hordeLevels[waveIndex];
            GameObject enemyPrefab = level.EnemyPrefab;

            if (_prototype != null)
                Destroy(_prototype.gameObject);

            _prototype = Instantiate(enemyPrefab).GetComponent<EnemyController>();
            _prototype.gameObject.SetActive(false);

            _prototype.InitializeFromStats(level.EnemyBaseStats, _player);

            StartCoroutine(StartHorde(level.HordeTimer));
        }

        /// <summary>Starts the horde for a set duration, spawning enemies once per second.</summary>
        private IEnumerator StartHorde(float timer)
        {
            StartCoroutine(HordeRemainingMessage(timer));

            int elapsed = 0;
            while (elapsed < timer)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();

                EnemyController enemy = _prototype.Clone(spawnPosition);
                enemy.gameObject.SetActive(true);
                enemy.transform.SetParent(_hordeParent);

                yield return new WaitForSeconds(1f);
                elapsed++;
            }

            // finished spawning for this wave — now wait until all spawned enemies are dead
            yield return StartCoroutine(WaitForAllEnemiesDead());

            // apply a random modification to the prototype for next wave
            ApplyRandomModificationToPrototype();

            _currentWave++;
            StartCoroutine(StartWaveCoroutine(_currentWave));
        }

        /// <summary>Shows messages about remaining horde time and when the wave completes.</summary>
        private IEnumerator HordeRemainingMessage(float timer)
        {
            bool halfShown = false;
            int halfRemaining = Mathf.CeilToInt(timer / 2f);

            for (int elapsed = 0; elapsed < timer; elapsed++)
            {
                int remaining = Mathf.CeilToInt(timer - elapsed);

                if (!halfShown && remaining == halfRemaining)
                {
                    _hordeMessageUI.ShowMessage($"{remaining} SECONDS LEFT!", 1.2f);
                    halfShown = true;
                }

                yield return new WaitForSeconds(1f);
            }

            _hordeMessageUI.ShowMessage("WAVE COMPLETED!", 3f);
        }
        /// <summary>Applies a random stat modification to the enemy prototype.</summary>
        private void ApplyRandomModificationToPrototype()
        {
            if (_prototype == null) return;

            var statTypes = System.Enum.GetValues(typeof(EnemyController.StatType));
            EnemyController.StatType chosen = (EnemyController.StatType)statTypes.GetValue(Random.Range(0, statTypes.Length));

            float modifier = 0f;
            switch (chosen)
            {
                case EnemyController.StatType.Health: modifier = Random.Range(1f, 5f); break;
                case EnemyController.StatType.Speed: modifier = Random.Range(0.5f, 2f); break;
                case EnemyController.StatType.Damage: modifier = Random.Range(1f, 3f); break;
            }

            _prototype.ApplyModifier(chosen, modifier);
        }

        private IEnumerator WaitForAllEnemiesDead()
        {
            const float pollInterval = 0.5f;

            while (true)
            {
                bool anyAlive = false;

                if (_hordeParent != null && _hordeParent.childCount > 0)
                {
                    for (int i = 0; i < _hordeParent.childCount; i++)
                    {
                        Transform child = _hordeParent.GetChild(i);
                        if (child == null) continue;

                        EnemyController enemyController = child.GetComponent<EnemyController>();
                        if (enemyController != null && child.gameObject.activeInHierarchy)
                        {
                            anyAlive = true;
                            break;
                        }
                    }
                }

                if (!anyAlive)
                {
                    _hordeMessageUI.ShowMessage("WAVE CLEARED!", 2f);
                    yield break;
                }

                yield return new WaitForSeconds(pollInterval);
            }
        }

        /// <summary>Calculates and returns a random spawn position outside the screen relative to the player.</summary>
        private Vector3 GetRandomSpawnPosition()
        {
            // radial distance from the player where we initially try to spawn
            float radius = 10f;
            Vector3 playerPos = _player.transform.position;

            // choose a random angle around the player and convert to XY coordinates
            float angle = Random.Range(0f, 360f);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            Vector3 spawnPos = new Vector3(x, y, 0f) + playerPos;

            // check if the calculated position is inside the camera's viewport
            Camera cameraMain = Camera.main;
            Vector3 viewportPoint = cameraMain.WorldToViewportPoint(spawnPos);

            // if it's inside the screen, reposition to a random edge outside the viewport
            if (viewportPoint.x > 0 && viewportPoint.x < 1 && viewportPoint.y > 0 && viewportPoint.y < 1)
            {
                int side = Random.Range(0, 4);
                switch (side)
                {
                    case 0:
                        // left
                        spawnPos = cameraMain.ViewportToWorldPoint(new Vector3(-0.1f, Random.value, cameraMain.nearClipPlane));
                        break;
                    case 1:
                        // right
                        spawnPos = cameraMain.ViewportToWorldPoint(new Vector3(1.1f, Random.value, cameraMain.nearClipPlane));
                        break;
                    case 2:
                        // bottom
                        spawnPos = cameraMain.ViewportToWorldPoint(new Vector3(Random.value, -0.1f, cameraMain.nearClipPlane));
                        break;
                    case 3:
                        // top
                        spawnPos = cameraMain.ViewportToWorldPoint(new Vector3(Random.value, 1.1f, cameraMain.nearClipPlane));
                        break;
                }
                // keep the same Z plane as the player for 2D/top-down consistency
                spawnPos.z = playerPos.z;
            }
            return spawnPos;
        }
    }
}