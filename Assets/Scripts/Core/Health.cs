using RPG.Saving;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(Animator))]
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoints = 100f;

        private Animator _animator;
        public bool IsDead { get; private set; }

        void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead)
                return;

            IsDead = true;
            _animator.SetTrigger("death");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
            {
                Die();
            }
        }
    }
}