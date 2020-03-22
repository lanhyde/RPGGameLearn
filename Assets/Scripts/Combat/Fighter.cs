using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using RPG.Stats;
using RPG.Utils;
using UnityEngine;

namespace RPG.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider {
        [SerializeField]
        float timeBetweenAttacks = 1.5f;

        [SerializeField]
        Transform rightHandTransform = null;
        [SerializeField]
        Transform leftHandTransform = null;
        [SerializeField]
        WeaponConfig defaultWeapon = null;

        private const float maxSpeedFraction = 1.0f;
        private float timeSinceLastAttack = Mathf.Infinity;
        private Health target;
        private WeaponConfig currentWeaponConfig = null;
        private LazyValue<Weapon> currentWeapon;
        private void Awake () {
            currentWeaponConfig = defaultWeapon;
            currentWeapon = new LazyValue<Weapon> (SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon () {
            return AttachWeapon (defaultWeapon);
        }
        private void Start () {
            currentWeapon.ForceInit();
        }

        private void Update () {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;
            if (target.IsDead ()) return;

            if (!GetIsInRange ()) {
                GetComponent<Mover> ().MoveTo (target.transform.position, maxSpeedFraction);
            } else {
                GetComponent<Mover> ().Cancel ();
                AttackBehaviour ();
            }
        }

        public bool CanAttack (GameObject combatTarget) {
            if (!combatTarget) {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health> ();
            return targetToTest != null && !targetToTest.IsDead ();
        }

        private void AttackBehaviour () {
            transform.LookAt (target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This will trigger the hit event
                TriggerAttack ();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack () {
            GetComponent<Animator> ().ResetTrigger ("StopAttack");
            GetComponent<Animator> ().SetTrigger ("Attack");
        }

        // Animation Event
        private void Hit () {
            if (!target) return;
            float damage = GetComponent<BaseStats> ().GetStat (Stat.Damage);
            if(currentWeapon.value != null)
            {
                currentWeapon.value.OnHit();
            }
            if (currentWeaponConfig.HasProjectile ()) {
                currentWeaponConfig.LaunchProjectile (rightHandTransform, leftHandTransform, target, gameObject, GetComponent<BaseStats> ().GetStat (Stat.Damage));
            } else {
                target.TakeDamage (gameObject, GetComponent<BaseStats> ().GetStat (Stat.Damage));
            }
        }

        private void Shoot () {
            Hit ();
        }

        private bool GetIsInRange () {
            return Vector3.Distance (transform.position, target.transform.position) < currentWeaponConfig.WeaponRange;
        }
        public void Attack (GameObject combatTarget) {
            GetComponent<ActionScheduler> ().StartAction (this);
            target = combatTarget.GetComponent<Health> ();
        }

        public void Cancel () {
            StopAttack ();
            target = null;
            GetComponent<Mover> ().Cancel ();
        }

        private void StopAttack () {
            GetComponent<Animator> ().ResetTrigger ("Attack");
            GetComponent<Animator> ().SetTrigger ("StopAttack");
        }

        public void EquipWeapon (WeaponConfig weapon) {
            currentWeaponConfig = weapon;
            currentWeapon.value = AttachWeapon (weapon);
        }

        private Weapon AttachWeapon (WeaponConfig weapon) {
            Animator animator = GetComponent<Animator> ();
            return weapon.Spawn (rightHandTransform, leftHandTransform, animator);
        }

        public object CaptureState () {
            return currentWeaponConfig.name;
        }

        public void RestoreState (object state) {
            string weaponName = (string) state;
            WeaponConfig weapon = UnityEngine.Resources.Load<WeaponConfig> (weaponName);
            EquipWeapon (weapon);
        }

        public Health GetTarget () {
            return target;
        }

        public IEnumerable<float> GetAdditiveModifiers (Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaponConfig.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers (Stat stat) {
            if (stat == Stat.Damage) {
                yield return currentWeaponConfig.GetPercentageBonus ();
            }
        }
    }
}