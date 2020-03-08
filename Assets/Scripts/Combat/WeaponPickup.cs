using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField]
        private Weapon thisWeapon = null;

        [SerializeField] private float respawnTime = 5;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(thisWeapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform childTransform in transform)
            {
                childTransform.gameObject.SetActive(shouldShow);
            }
        }
    }

}