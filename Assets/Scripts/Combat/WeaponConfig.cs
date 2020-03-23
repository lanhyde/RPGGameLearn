using System;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Combat {
    [CreateAssetMenu (fileName = "Weapon", menuName = "RPGProject/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject {
        [SerializeField]
        AnimatorOverrideController animatorOverride = null;
        [SerializeField]
        Weapon equippedPrefab = null;
        [SerializeField]
        Projectile projectile = null;
        [SerializeField]
        float weaponRange = 2f;
        [SerializeField]
        float weaponDamage = 4f;

        [SerializeField] private float percentageBonus = 0f;
        [SerializeField]
        bool isRightHanded = true;

        const string weaponName = "Weapon";
        const string destroyingWeaponName = "DESTROYING...";
        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;

        public Weapon Spawn (Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon (rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab) {
                Transform handTransform = GetHandTransform (rightHand, leftHand);
                weapon = Instantiate (equippedPrefab, handTransform);
                weapon.gameObject.name = weaponName;
            }
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride) {
                animator.runtimeAnimatorController = animatorOverride;
            } else if (overrideController) {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            return weapon;
        }

        public bool HasProjectile () => projectile != null;
        public Transform GetHandTransform (Transform rightHand, Transform leftHand) => isRightHanded ? rightHand : leftHand;

        public void LaunchProjectile (Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) {
            Projectile projectileInstance = Instantiate (projectile, GetHandTransform (rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget (target, instigator, calculatedDamage);
        }

        public float GetPercentageBonus () {
            return percentageBonus;
        }

        private void DestroyOldWeapon (Transform rightHand, Transform leftHand) {
            Transform oldWeapon = rightHand.Find (weaponName);
            if (!oldWeapon) {
                oldWeapon = leftHand.Find (weaponName);
            }
            if (!oldWeapon) return;
            oldWeapon.name = destroyingWeaponName;
            Destroy (oldWeapon.gameObject);
        }
    }

}