using System.Collections;
using System.Collections.Generic;
using RPG.Core;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "RPGProject/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField]
        AnimatorOverrideController animatorOverride = null;//动画覆盖
        [SerializeField]
        GameObject equippedPrefab = null;//装备预制体
        [SerializeField]
        Projectile projectile = null;//投掷物
        [SerializeField]
        float weaponRange = 2f; //武器范围
        [SerializeField]
        float weaponDamage = 4f;//武器伤害
        [SerializeField]
        bool isRightHanded = true;//是右手

        const string weaponName = "Weapon";
        const string destroyingWeaponName = "DESTROYING...";
        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;
        /// <summary>
        /// 设定武器
        /// </summary>
        /// <param name="rightHand">右手</param>
        /// <param name="leftHand">左手</param>
        /// <param name="animator">动画师</param>
        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (equippedPrefab)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, handTransform);
                weapon.name = weaponName;
            }
            if (animatorOverride)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
        }

        public bool HasProjectile() => projectile != null;
        public Transform GetHandTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;
        /// <summary>
        /// 子弹？
        /// </summary>
        /// <param name="rightHand">右手</param>
        /// <param name="leftHand">左手</param>
        /// <param name="target">目标</param>
        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
        /// <summary>
        /// 销毁旧武器
        /// </summary>
        /// <param name="rightHand">右手</param>
        /// <param name="leftHand">左手</param>
        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            //获取要销毁的武器
            Transform oldWeapon = rightHand.Find(weaponName);
            #region 右手没找到就找左手 
            if (!oldWeapon)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if (!oldWeapon) return;
            #endregion

            //设置销毁名
            oldWeapon.name = destroyingWeaponName;
            //销毁旧武器
            Destroy(oldWeapon.gameObject);
        }
    }

}