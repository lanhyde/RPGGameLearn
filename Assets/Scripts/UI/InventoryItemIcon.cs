using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    [RequireComponent(typeof(Image))]
    public class InventoryItemIcon : MonoBehaviour
    {
        [SerializeField] private GameObject textContainer = null;
        [SerializeField] private TextMeshProUGUI itemNumber = null;
        public void SetItem(InventoryItem item, int number)
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

            if (itemNumber)
            {
                if (number <= 1)
                {
                    textContainer.SetActive(false);
                }
                else
                {
                    textContainer.SetActive(true);
                    itemNumber.text = number.ToString();
                }
            }
        }
    }

}
