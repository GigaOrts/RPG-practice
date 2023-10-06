using RPG.Core;
using RPG.Movement;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RPG.Combat
{
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float _weaponRange = 2f;
        [SerializeField] private float _attackDelay = 1f;
        [SerializeField] private float _damage;

        private Health _target;
        private Animator _animator;
        private Mover _mover;

        private float _timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;

            if (_target == null || _target.IsDead)
                return;

            transform.LookAt(_target.transform, Vector3.up);
            
            if (IsInRange() == false)
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();
                AttackBehaviour();
            }
        }

        private void Hit()
        {
            if (_target != null)
                _target.TakeDamage(_damage);
        }

        private void AttackBehaviour()
        {
            if (_timeSinceLastAttack >= _attackDelay)
            {
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        public bool CanAttack(GameObject target)
        {
            return target.GetComponent<Health>().IsDead == false;
        }

        public void Attack(GameObject target)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            _mover.Cancel();
            _target = null;
        }

        private void TriggerAttack()
        {
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= _weaponRange;
        }
    }
}