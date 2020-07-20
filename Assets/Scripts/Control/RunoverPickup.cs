using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Control
{
    [RequireComponent(typeof(Pickup))]
    public class RunoverPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (other.gameObject == player)
            {
                GetComponent<Pickup>().PickupItem();
            }
        }
    }   

}
