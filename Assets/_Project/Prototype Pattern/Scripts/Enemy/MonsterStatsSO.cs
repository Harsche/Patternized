using UnityEngine;

namespace PrototypePattern.Enemy
{
    [CreateAssetMenu(fileName = "New Monster", menuName = "Patternized/Prototype Pattern/Create Monster Stats", order = 1)]
    public class MonsterStatsSO : ScriptableObject
    {
        [field: SerializeField] public float Health { get; private set; } = 10f;
        [field: SerializeField] public float Speed { get; private set; } = 5f;
        [field: SerializeField] public int Damage { get; private set; } = 1;
        [field: SerializeField] public int KnockbackForce { get; private set; } = 200;
    }
}
