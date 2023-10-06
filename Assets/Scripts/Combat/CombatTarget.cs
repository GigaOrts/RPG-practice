using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour
    {
        private Health _health;

        public bool IsDead => _health.IsDead;

        private void Start()
        {
            _health = GetComponent<Health>();
        }
    }
}