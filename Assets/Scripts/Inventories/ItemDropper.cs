﻿using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class ItemDropper : MonoBehaviour, ISaveable
    {
        private List<Pickup> droppedItems = new List<Pickup>();

        public void DropItem(InventoryItem item)
        {
            SpawnPickup(item, GetDropLocation());
        }

        protected virtual Vector3 GetDropLocation()
        {
            return transform.position;
        }

        public void SpawnPickup(InventoryItem item, Vector3 spawnLocation)
        {
            var pickup = item.SpawnPickup(spawnLocation);
            droppedItems.Add(pickup);
        }

        [System.Serializable]
        private struct DropRecord
        {
            public string itemID;
            public SerializableVector3 position;
        }

        object ISaveable.CaptureState()
        {
            RemoveDestroyedDrops();
            var droppedItemList = new DropRecord[droppedItems.Count];
            for (int i = 0; i < droppedItemList.Length; ++i)
            {
                droppedItemList[i].itemID = droppedItems[i].GetItem().GetItemID();
                droppedItemList[i].position = new SerializableVector3(droppedItems[i].transform.position);
            }

            return droppedItemList;
        }

        void ISaveable.RestoreState(object state)
        {
            var droppedItemList = (DropRecord[]) state;
            foreach (var item in droppedItemList)
            {
                var pickupItem = InventoryItem.GetFromID(item.itemID);
                Vector3 position = item.position.ToVector();
                SpawnPickup(pickupItem, position);
            }
        }

        private void RemoveDestroyedDrops()
        {
            var newList = new List<Pickup>();
            foreach (var item in droppedItems)
            {
                if (item != null)
                {
                    newList.Add(item);
                }
            }

            droppedItems = newList;
        }
    }

}
