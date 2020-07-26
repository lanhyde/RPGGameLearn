using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.UI.Dragging;
using RPG.UI.Inventories;
using UnityEngine;

namespace RPG.UI
{
    public class InventorySlotUI : MonoBehaviour, IItemHolder, IDragContainer<InventoryItem>
    {
        [SerializeField] private InventoryItemIcon icon = null;

        private int index;
        private Inventory inventory;

        public void Setup(Inventory inventory, int index)
        {
            this.inventory = inventory;
            this.index = index;
            icon.SetItem(inventory.GetItemInSlot(index), inventory.GetNumberInSlot(index));
        }
        public int MaxAcceptable(InventoryItem item)
        {
            if (inventory.HasSpaceFor(item))
            {
                return int.MaxValue;
            }

            return 0;
        }

        public void AddItems(InventoryItem item, int number)
        {
            inventory.AddItemToSlot(index, item, number);
        }

        public InventoryItem GetItem()
        {
            return inventory.GetItemInSlot(index);
        }

        public int GetNumber()
        {
            return inventory.GetNumberInSlot(index);
        }

        public void RemoveItems(int number)
        {
            inventory.RemoveFromSlot(index, number);
        }
    }

}
