using UnityEngine;

namespace PrototypePattern.Enemy
{
    [CreateAssetMenu(fileName = "New Monster", menuName = "Patternized/Prototype Pattern/Create Monster Stats", order = 1)]
    public class MonsterStatsSO : ScriptableObject
    {
        [field: SerializeField] private float _health = 10f;
        [field: SerializeField] private float _speed = 5f;
        [field: SerializeField] private int _damage = 1;
        [field: SerializeField] private int _knockbackForce = 200;

        public float Health => _health;
        public float Speed => _speed;
        public int Damage => _damage;
        public int KnockbackForce => _knockbackForce;
    }
}
