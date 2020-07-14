using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEditor;
using UnityEngine;

namespace RPG.Inventories
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [Tooltip("Allowed size")] [SerializeField]
        private int inventorySize = 16;

        private InventoryItem[] slots;

        public event Action inventoryUpdated;

        public static Inventory GetPlayerInventory()
        {
            var player = GameObject.FindWithTag("Player");
            return player.GetComponent<Inventory>();
        }
        /// <summary>
        /// Could this item fit anywhere in the inventory?
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasSpaceFor(InventoryItem item)
        {
            return FindSlot(item) >= 0;
        }

        public int GetSize()
        {
            return slots.Length;
        }
        /// <summary>
        /// Attempt to add the items to the first available slot.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool AddToFirstEmptySlot(InventoryItem item)
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }

            slots[i] = item;
            inventoryUpdated?.Invoke();
            return true;
        }

        public bool HasItem(InventoryItem item)
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                if (ReferenceEquals(slots[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        public InventoryItem GetItemInSlot(int slot)
        {
            return slots[slot];
        }

        public void RemoveFromSlot(int slot)
        {
            slots[slot] = null;
            inventoryUpdated?.Invoke();
        }

        public bool AddItemToSlot(int slot, InventoryItem item)
        {
            if (slots[slot] != null)
            {
                return AddToFirstEmptySlot(item);
            }

            slots[slot] = item;
            inventoryUpdated?.Invoke();
            return true;
        }

        void Awake()
        {
            slots = new InventoryItem[inventorySize];
            slots[0] = InventoryItem.GetFromID("dfd9dc40-2be5-4952-83ef-50f417e0f7a3");
            slots[1] = InventoryItem.GetFromID("b799d6e4-a137-4fe7-b08f-613a0eb1294d");
            slots[2] = InventoryItem.GetFromID("a85d8697-08aa-4410-819f-40310f9528ce");
        }
        /// <summary>
        /// Find a slot that can accomodate the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int FindSlot(InventoryItem item)
        {
            return FindEmptySlot();
        }

        private int FindEmptySlot()
        {
            for (int i = 0; i < slots.Length; ++i)
            {
                if (slots[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        object ISaveable.CaptureState()
        {
            var slotStrings = new string[inventorySize];
            for (int i = 0; i < inventorySize; ++i)
            {
                if (slots[i] != null)
                {
                    slotStrings[i] = slots[i].GetItemID();
                }
            }
            return slotStrings;
        }

        void ISaveable.RestoreState(object state)
        {
            var slotStrings = (string[]) state;
            for (int i = 0; i < inventorySize; ++i)
            {
                slots[i] = InventoryItem.GetFromID(slotStrings[i]);
            }
            inventoryUpdated?.Invoke();
        }
    }
}
