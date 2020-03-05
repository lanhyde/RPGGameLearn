using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon thisWeapon = null;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(thisWeapon);
                Destroy(gameObject);
            }
        }
    }

}