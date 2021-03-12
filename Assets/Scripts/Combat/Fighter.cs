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
        float timeBetweenAttacks = 1.5f;//�������ʱ��

        [SerializeField]
        Transform rightHandTransform = null;//����
        [SerializeField]
        Transform leftHandTransform = null;//����
        [SerializeField]
        WeaponConfig defaultWeapon = null;//Ĭ������

        private const float maxSpeedFraction = 1.0f;//����ٶȷ�����
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
                 GetComponent<Mover> ().MoveTo (target.transform.position, maxSpeedFraction);//�ƶ���ȥ
            }
            else
            {
                GetComponent<Mover>().Cancel();//ȡ���ƶ�
                AttackBehaviour();//����
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
        /// ������Ϊ
        /// </summary>
        private void AttackBehaviour()
        {
            //��ת����ʹ�õ�ǰ�������z��ָ��Ŀ�����target���ڵ�λ�á�
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks) {
                // This will trigger the hit event
                TriggerAttack ();
                timeSinceLastAttack = 0;
            }
        }
        /// <summary>
        ///  ��������
        /// </summary>
        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("StopAttack");//���ù���ֹͣ����
            GetComponent<Animator>().SetTrigger("Attack");//���ù�������
        }
        // Animation Event
        private void Hit()//Զ�̹���?
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
        /// �ڹ�����Χ��
        /// </summary>
        /// <returns>���</returns>
        private bool GetIsInRange (Transform targetTransform) {
            return Vector3.Distance (transform.position, targetTransform.position) < currentWeaponConfig.WeaponRange;
        }
        public void Attack (GameObject combatTarget) {
            GetComponent<ActionScheduler> ().StartAction (this);
            target = combatTarget.GetComponent<Health> ();
        }
        /// <summary>
        /// ȡ��
        /// </summary>
        public void Cancel () {
            StopAttack ();
            target = null;
            GetComponent<Mover> ().Cancel ();
        }
        /// <summary>
        /// ֹͣ��������
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