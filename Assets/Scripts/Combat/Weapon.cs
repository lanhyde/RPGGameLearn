using System.Collections;
using System.Collections.Generic;
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
        float weaponRange = 2f;
        [SerializeField]
        float weaponDamage = 4f;

        public float WeaponRange => weaponRange;
        public float WeaponDamage => weaponDamage;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if(!equippedPrefab) return;
            Instantiate(equippedPrefab, handTransform);
            if(!animatorOverride) return;
            animator.runtimeAnimatorController = animatorOverride;
        }
    }

}