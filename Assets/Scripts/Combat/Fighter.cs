using System;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        float timeBetweenAttacks = 1.5f;//攻击间隔时间

        [SerializeField]
        Transform rightHandTransform = null;//右手
        [SerializeField]
        Transform leftHandTransform = null;//左手
        [SerializeField]
        Weapon defaultWeapon = null;//默认武器
        private const float maxSpeedFraction = 1.0f;//最大速度分数？
        private float timeSinceLastAttack = Mathf.Infinity;//最后攻击计时
        private Health target;//目标
        private Weapon currentWeapon = null;//现在的武器

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (!target) return;//
            if (target.IsDead()) return;//以死亡

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, maxSpeedFraction);//移动过去
            }
            else
            {
                GetComponent<Mover>().Cancel();//取消移动
                AttackBehaviour();//攻击
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (!combatTarget)
            {
                return false;
            }
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }
        /// <summary>
        /// 攻击行为
        /// </summary>
        private void AttackBehaviour()
        {
            //旋转自身，使得当前对象的正z轴指向目标对象target所在的位置。
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > timeBetweenAttacks)
            {
                // This will trigger the hit event
                TriggerAttack();
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
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.WeaponDamage);
            }
        }

        private void Shoot()
        {
            Hit();
        }
        /// <summary>
        /// 在攻击范围内
        /// </summary>
        /// <returns>结果</returns>
        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.WeaponRange;
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }
        /// <summary>
        /// 取消
        /// </summary>
        public void Cancel()
        {
            StopAttack();
            target = null;
            GetComponent<Mover>().Cancel();
        }
        /// <summary>
        /// 停止攻击动画
        /// </summary>
        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("Attack");
            GetComponent<Animator>().SetTrigger("StopAttack");
        }
        /// <summary>
        /// 装备武器
        /// </summary>
        /// <param name="weapon">武器</param>
        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;//变更当前的武器
            Animator animator = GetComponent<Animator>();//获取动画师
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }
    }
}