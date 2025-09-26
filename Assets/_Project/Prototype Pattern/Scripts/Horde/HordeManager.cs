using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

namespace PrototypePattern.Horde
{
    [RequireComponent(typeof(HordeMessageUI))]
    public class HordeManager : MonoBehaviour
    {
        public static HordeManager Instance { get; private set; }

        [Header("Tilemap Bounds")]
        [SerializeField] private Tilemap _tilemap;

        [Header("Enemy Visuals")]
        [SerializeField] private Sprite[] _enemySprites;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private MonsterStatsSO _initialStats;
        [SerializeField] private Transform _hordeParent;

        [Header("Horde Settings")]
        [SerializeField] private float _hordeTimer = 25f;

        [Header("Player Reference")]
        [SerializeField] private PlayerController _player;

        [Header("UI")]
        [SerializeField] private TMP_Text _waveCounterText;

        [Header("Prototype & State")]
        private EnemyController _prototype;
        private int _currentWave = 0;
        private HordeMessageUI _hordeMessageUI;
        private int _enemiesSpawned;
        private int _enemiesRemainingInWave;
        private readonly List<EnemyController> _activeEnemies = new List<EnemyController>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            _hordeMessageUI = GetComponent<HordeMessageUI>();
        }

        private void Start()
        {
            _prototype = Instantiate(_enemyPrefab).GetComponent<EnemyController>();
            _prototype.gameObject.SetActive(false);
            _prototype.InitializeFromStats(_initialStats, _player);

            StartCoroutine(StartWaveCoroutine());
        }
        private void Update()
        {
            ClampEnemiesToTilemap();
        }

        private void ClampEnemiesToTilemap()
        {
            if (_tilemap == null) return;
            var bounds = _tilemap.cellBounds;
            Vector3 min = _tilemap.CellToWorld(bounds.min);
            Vector3 max = _tilemap.CellToWorld(bounds.max);
            foreach (var enemy in _activeEnemies)
            {
                if (enemy == null) continue;
                Vector3 pos = enemy.transform.position;
                pos.x = Mathf.Clamp(pos.x, min.x, max.x);
                pos.y = Mathf.Clamp(pos.y, min.y, max.y);
                enemy.transform.position = pos;
            }
        }
        private IEnumerator StartWaveCoroutine()
        {
            _enemiesRemainingInWave = 0;
            _waveCounterText.text = "00/00";

            _currentWave++;
            _hordeMessageUI.ShowMessage($"WAVE {_currentWave}", 3f);
            yield return new WaitForSeconds(3f);

            _enemiesSpawned = Mathf.RoundToInt(_hordeTimer);
            _enemiesRemainingInWave = _enemiesSpawned;
            UpdateWaveCounterText();

            StartCoroutine(StartHorde(_hordeTimer));
        }

        private IEnumerator StartHorde(float timer)
        {
            int elapsed = 0;
            while (elapsed < timer)
            {
                if (_enemiesRemainingInWave <= 0) break;

                Vector3 spawnPosition = GetRandomSpawnPosition();
                EnemyController enemy = _prototype.Clone(spawnPosition);
                enemy.InitializeFromStats(_initialStats, _player);
                enemy.gameObject.SetActive(true);
                enemy.transform.SetParent(_hordeParent);
                AssignRandomSprite(enemy);
                _activeEnemies.Add(enemy);
                enemy.OnDeathEvent += () => OnEnemyDeath(enemy);

                yield return new WaitForSeconds(1f);
                elapsed++;
            }

            yield return StartCoroutine(WaitForAllEnemiesDead());

            ApplyRandomModificationToPrototype();
        }

        private void OnEnemyDeath(EnemyController enemy)
        {
            _activeEnemies.Remove(enemy);
            _enemiesRemainingInWave--;
            UpdateWaveCounterText();
        }

        private void AssignRandomSprite(EnemyController enemy)
        {
            if (_enemySprites.Length > 0)
            {
                SpriteRenderer spriteRenderer = enemy.GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = _enemySprites[Random.Range(0, _enemySprites.Length)];
            }
        }
        private void ApplyRandomModificationToPrototype()
        {
            if (_prototype == null) return;

            var statTypes = System.Enum.GetValues(typeof(EnemyController.StatType));
            EnemyController.StatType chosen = (EnemyController.StatType)statTypes.GetValue(Random.Range(0, statTypes.Length));

            float modifier = 0f;
            string message = "";
            switch (chosen)
            {
                case EnemyController.StatType.Health:
                    modifier = Random.Range(1f, 5f);
                    message = $"WAVE {_currentWave}\nMonsters gain\n+{modifier:F1} Health!";
                    break;
                case EnemyController.StatType.Speed:
                    modifier = Random.Range(0.5f, 2f);
                    message = $"WAVE {_currentWave}\nMonsters gain\n+{modifier:F1} Speed!";
                    break;
                case EnemyController.StatType.Damage:
                    modifier = Random.Range(1f, 3f);
                    message = $"WAVE {_currentWave}\nMonsters gain\n+{modifier:F1} Damage!";
                    break;
            }

            _prototype.ApplyModifier(chosen, modifier);
            _hordeMessageUI.ShowMessage(message, 3f);
        }

        private IEnumerator WaitForAllEnemiesDead()
        {
            const float pollInterval = 0.5f;
            while (_enemiesRemainingInWave > 0)
            {
                yield return new WaitForSeconds(pollInterval);
            }
            _hordeMessageUI.ShowMessage("WAVE CLEARED!", 2f);
            yield return new WaitForSeconds(2f);
            StartCoroutine(StartWaveCoroutine());
        }

        private void UpdateWaveCounterText()
        {
            _waveCounterText.text = $"{_enemiesRemainingInWave}/{_enemiesSpawned}";
        }

        private Vector3 GetRandomSpawnPosition()
        {
            BoundsInt bounds = _tilemap.cellBounds;
            int maxTries = 100;
            for (int i = 0; i < maxTries; i++)
            {
                int x = Random.Range(bounds.xMin, bounds.xMax);
                int y = Random.Range(bounds.yMin, bounds.yMax);
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (_tilemap.HasTile(cellPos))
                {
                    Vector3 spawnPos = _tilemap.CellToWorld(cellPos) + _tilemap.cellSize / 2;
                    spawnPos.z = 0f;
                    return spawnPos;
                }
            }
            return _tilemap.CellToWorld(Vector3Int.RoundToInt(bounds.center)) + _tilemap.cellSize / 2;
        }
    }
}
