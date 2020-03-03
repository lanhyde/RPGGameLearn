using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        float timeBetweenAttacks = 1.5f;

        [SerializeField]
        Transform handTransform = null;
        [SerializeField]
        Weapon weapon = null;
        private const float maxSpeedFraction = 1.0f;
        private float timeSinceLastAttack = Mathf.Infinity;
        Health target;

        private void Start()
        {
            SpawnWeapon();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if(!target) return;
            if(target.IsDead()) return;

            if(!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, maxSpeedFraction);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if(!combatTarget) 
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the hit event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");
            GetComponent<Animator>().SetTrigger("Attack");
        }

        // Animation Event
        private void Hit()
        {
            if(!target) return;
            target.TakeDamage(weapon.WeaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weapon.WeaponRange;
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }

        private void SpawnWeapon()
        {
            if(!weapon) return;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(handTransform, animator);
        }
    }
}