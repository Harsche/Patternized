using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using TMPro;
using PrototypePattern.Utilities;
using PrototypePattern.Managers;

namespace PrototypePattern.Horde
{
    [RequireComponent(typeof(HordeMessageUI))]
    public class HordeManager : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyPrototype;
        [SerializeField] private Transform _hordeParent;
        [SerializeField] private PlayerController _player;
        [SerializeField] private TMP_Text _waveCounterText;
        [SerializeField] private List<WaveConfig> _waves;
        [SerializeField] private Tilemap _spawnTilemap;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private EndGameManager _endGameManager;

        private HordeMessageUI _hordeMessageUI;

        private int _currentWave = 0;
        private int _killsInWave = 0;
        private List<EnemyController> _activeEnemies = new List<EnemyController>();

        private void Start()
        {
            _hordeMessageUI = GetComponent<HordeMessageUI>();
            StartCoroutine(StartWaveCoroutine());
        }

        private IEnumerator StartWaveCoroutine()
        {
            _currentWave++;
            if (_currentWave > _waves.Count)
            {
                _endGameManager.ShowVictory();
                yield break;
            }

            WaveConfig wave = _waves[_currentWave - 1];
            _killsInWave = wave.EnemyCount;
            _waveCounterText.text = $"{_killsInWave}/{wave.EnemyCount}";

            _hordeMessageUI.ShowMessage($"WAVE {_currentWave}", 2f);
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < wave.EnemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(wave.SpawnInterval);
            }

            while (_activeEnemies.Count > 0) yield return null;

            _hordeMessageUI.ShowMessage("WAVE CLEARED!", 2f);
            yield return new WaitForSeconds(2f);

            UpgradePrototype();
            StartCoroutine(StartWaveCoroutine());
        }

        private void SpawnEnemy()
        {
            Vector3 position = GetRandomSpawnPosition();
            EnemyController enemy = _enemyPrototype.Clone(position);
            enemy.SetPlayer(_player);
            enemy.transform.SetParent(_hordeParent);
            enemy.gameObject.SetActive(true);

            enemy.OnDeathEvent += () =>
            {
                _activeEnemies.Remove(enemy);
                _killsInWave--;
                _waveCounterText.text = $"{_killsInWave}/{_waves[_currentWave - 1].EnemyCount}";
            };

            _activeEnemies.Add(enemy);
        }

        private void UpgradePrototype()
        {
            float modifier = Random.Range(1f, 3f);
            _enemyPrototype.ApplyModifier(EnemyController.StatType.Health, modifier);
            _enemyPrototype.ApplyModifier(EnemyController.StatType.Speed, modifier);
            _enemyPrototype.ApplyModifier(EnemyController.StatType.Damage, modifier);
            _hordeMessageUI.ShowMessage($"Increased {modifier:F1} to all enemy stats!", 2f);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            return TilemapSpawnUtility.GetRandomSpawnPosition(_spawnTilemap, maxTries: 50, Vector3.zero, _mainCamera);
        }
    }
}
