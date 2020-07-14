using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = "RPGProject/Inventory/Item")]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver
    {
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one")]
        [SerializeField]
        private string itemID = null;

        [Tooltip("Item name to be displayed in UI")] [SerializeField]
        private string displayName = null;

        [Tooltip("Item description to be displayed in UI.")] [SerializeField, TextArea]
        private string description = null;

        [Tooltip("The UI icon to represent this item in the inventory")] [SerializeField]
        private Sprite icon = null;

        [Tooltip("If true, items of this type can be stacked in the same inventory slot")] [SerializeField]
        private bool stackable = false;

        private static Dictionary<string, InventoryItem> itemLookupCache;
        /// <summary>
        /// Get the inventory item instance from its UUID
        /// </summary>
        /// <param name="itemID">string UUID that persists between game instances</param>
        /// <returns>Inventory item instance corresponding to the ID</returns>
        public static InventoryItem GetFromID(string itemID)
        {
            if (itemLookupCache == null)
            {
                itemLookupCache = new Dictionary<string, InventoryItem>();
                var itemList = Resources.LoadAll<InventoryItem>("");
                foreach (var item in itemList)
                {
                    if (itemLookupCache.ContainsKey(item.itemID))
                    {
                        Debug.LogError($"Looks like there's duplicate Inventory ID for objects {itemLookupCache[item.itemID]} and {item}");
                        continue;
                    }

                    itemLookupCache[item.itemID] = item;
                }
            }

            if (itemID == null || !itemLookupCache.ContainsKey(itemID)) return null;
            return itemLookupCache[itemID];
        }

        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }

        public bool IsStackable()
        {
            return stackable;
        }

        public string GetDisplayName()
        {
            return displayName;
        }

        public string GetDescription()
        {
            return description;
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Nothing to do
        }
    }
}
