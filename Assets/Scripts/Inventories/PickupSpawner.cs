using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class PickupSpawner : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventoryItem item = null;
        [SerializeField] private int number = 1;

        private void Awake()
        {
            SpawnPickup();
        }

        public Pickup GetPickup()
        {
            return GetComponentInChildren<Pickup>();
        }

        public bool IsCollected()
        {
            return GetPickup() == null;
        }

        private void SpawnPickup()
        {
            var spawnedPickup = item.SpawnPickup(transform.position, number);
            spawnedPickup.transform.SetParent(transform);
        }

        private void DestroyPickup()
        {
            if (GetPickup())
            {
                Destroy(GetPickup().gameObject);
            }
        }

        object ISaveable.CaptureState()
        {
            return IsCollected();
        }

        void ISaveable.RestoreState(object state)
        {
            bool shouldBeCollected = (bool) state;
            if (shouldBeCollected && !IsCollected())
            {
                DestroyPickup();
            }

            if (!shouldBeCollected && IsCollected())
            {
                SpawnPickup();
            }
        }
    }
}

