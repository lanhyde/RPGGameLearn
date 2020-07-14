using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotUI inventoryItemPrefab = null;
        private Inventory playerInventory;

        private void Awake()
        {
            playerInventory = Inventory.GetPlayerInventory();
            playerInventory.inventoryUpdated += Redraw;
        }

        private void Start()
        {
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < playerInventory.GetSize(); ++i)
            {
                var itemUI = Instantiate(inventoryItemPrefab, transform);
                itemUI.Setup(playerInventory, i);
            }
        }
    }

}
