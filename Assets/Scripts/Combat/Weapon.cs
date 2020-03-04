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
        AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        GameObject equippedPrefab = null;
        [SerializeField]
        Projectile projectile = null;
        [SerializeField]
        float weaponRange = 2f;
        [SerializeField]
        float weaponDamage = 4f;
        [SerializeField]
        bool isRightHanded = true;

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if(!equippedPrefab) return;
            Transform handTransform = GetHandTransform(rightHand, leftHand);
            Instantiate(equippedPrefab, handTransform);
            if(!animatorOverride) return;
            animator.runtimeAnimatorController = animatorOverride;
        }

        public bool HasProjectile() => projectile != null;
        public Transform GetHandTransform(Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }
    }

}