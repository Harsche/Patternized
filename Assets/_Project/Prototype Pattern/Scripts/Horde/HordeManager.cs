using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Horde
{
    public class HordeManager : MonoBehaviour
    {
        [SerializeField] private List<HordeLevelSO> _hordeLevels;

        private void Start()
        {
            float timer = _hordeLevels[0].HordeTimer;
            StartCoroutine(StartHorde(timer));
        }
        private IEnumerator StartHorde(float timer)
        {
            int count = 0;
            EnemyController enemyController = _hordeLevels[0].EnemyPrefab.GetComponent<EnemyController>();
            while (count < timer)
            {
                enemyController.Clone();
                yield return new WaitForSeconds(1f);
                count++;
            }
        }
    }
}