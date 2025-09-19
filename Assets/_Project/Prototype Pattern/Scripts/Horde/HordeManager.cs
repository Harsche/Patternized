using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Horde
{
    public class HordeManager : MonoBehaviour
    {
        [SerializeField] private List<HordeLevelSO> _hordeLevels;
        [SerializeField] private Transform _hordeParent;

        private void Start()
        {
            float timer = _hordeLevels[0].HordeTimer;
            StartCoroutine(StartHorde(timer));
        }
        private IEnumerator StartHorde(float timer)
        {
            int count = 0;
            GameObject enemyPrefab = _hordeLevels[0].EnemyPrefab;
            EnemyController enemyController = enemyPrefab.GetComponent<EnemyController>();
            while (count < timer)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                
                EnemyController enemy = enemyController.Clone(spawnPosition);
                
                enemy.transform.SetParent(_hordeParent);
                
                yield return new WaitForSeconds(1f);
                count++;
            }
        }
        
        private Vector3 GetRandomSpawnPosition()
        {
            float radius = 10f;
            float angle = Random.Range(0f, 360f);
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            return new Vector3(x, y, 0f);
        }
    }
}