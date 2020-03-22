using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat {
    public class WeaponPickup : MonoBehaviour, IRaycastable {
        [SerializeField]
        private WeaponConfig thisWeapon = null;
        [SerializeField] float healthToRestore = 0;
        [SerializeField] private float respawnTime = 5;

        private void OnTriggerEnter (Collider other) {
            if (other.tag == "Player") {
                Pickup (other.gameObject);
            }
        }

        private void Pickup (GameObject subject) {
            if (thisWeapon != null) {
                subject.GetComponent<Fighter> ().EquipWeapon (thisWeapon);
            }
            if(healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds (float seconds) {
            ShowPickup (false);
            yield return new WaitForSeconds (seconds);
            ShowPickup (true);
        }

        private void ShowPickup (bool shouldShow) {
            GetComponent<Collider> ().enabled = shouldShow;
            foreach (Transform childTransform in transform) {
                childTransform.gameObject.SetActive (shouldShow);
            }
        }

        public bool HandleRaycast (PlayerController callingController) {
            if (Input.GetMouseButtonDown (0)) {
                Pickup (callingController.gameObject);
            }
            return true;
        }

        public CursorType GetCursorType () {
            return CursorType.Pickup;
        }
    }

}