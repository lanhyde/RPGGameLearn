using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        public void SetItem(InventoryItem item)
        {
            var iconImage = GetComponent<Image>();
            if (item == null)
            {
                iconImage.enabled = false;
            }
            else
            {
                iconImage.enabled = true;
                iconImage.sprite = item.GetIcon();
            }
        }
    }

}
