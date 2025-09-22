using System;
using System.Collections;
using System.Collections.Generic;
using PrototypePattern.Enemy;
using PrototypePattern.Player;
using UnityEngine;

namespace PrototypePattern.Horde
{
    [CreateAssetMenu(fileName = "New Horde", menuName = "Patternized/Prototype Pattern/Create Horde Level", order = 3)]
    public class HordeLevelSO : ScriptableObject
    {
        [field: SerializeField] public float HordeTimer { get; private set; }

        [field: SerializeField] public GameObject EnemyPrefab { get; private set; }

        [field: SerializeField] public MonsterStatsSO EnemyBaseStats { get; private set; }
    }
}