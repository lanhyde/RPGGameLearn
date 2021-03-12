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
        float timeBetweenAttacks = 1.5f;//攻击间隔时间

        [SerializeField]
        Transform rightHandTransform = null;//右手
        [SerializeField]
        Transform leftHandTransform = null;//左手
        [SerializeField]
        WeaponConfig defaultWeapon = null;//默认武器

        private const float maxSpeedFraction = 1.0f;//最大速度分数？
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

            if (!GetIsInRange (target.transform)) {
                 GetComponent<Mover> ().MoveTo (target.transform.position, maxSpeedFraction);//移动过去
            }
            else
            {
                GetComponent<Mover>().Cancel();//取消移动
                AttackBehaviour();//攻击
            }
        }

        public bool CanAttack (GameObject combatTarget) {
            if (!combatTarget) {
                return false;
            }

            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) && 
                GetIsInRange(combatTarget.transform))

            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health> ();
            return targetToTest != null && !targetToTest.IsDead ();
        }
        /// <summary>
        /// 攻击行为
        /// </summary>
        private void AttackBehaviour()
        {
            //旋转自身，使得当前对象的正z轴指向目标对象target所在的位置。
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This will trigger the hit event
                TriggerAttack ();
                timeSinceLastAttack = 0;
            }
        }
        /// <summary>
        ///  触发攻击
        /// </summary>
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");//重置攻击停止动画
            GetComponent<Animator>().SetTrigger("Attack");//设置攻击动画
        }
        // Animation Event
        private void Hit()//远程攻击?
        {
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
        /// <summary>
        /// 在攻击范围内
        /// </summary>
        /// <returns>结果</returns>
        private bool GetIsInRange (Transform targetTransform) {
            return Vector3.Distance (transform.position, targetTransform.position) < currentWeaponConfig.WeaponRange;
        }
        public void Attack (GameObject combatTarget) {
            GetComponent<ActionScheduler> ().StartAction (this);
            target = combatTarget.GetComponent<Health> ();
        }
        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel () {
            StopAttack ();
            target = null;
            GetComponent<Mover> ().Cancel ();
        }
        /// <summary>
        /// 停止攻击动画
        /// </summary>
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