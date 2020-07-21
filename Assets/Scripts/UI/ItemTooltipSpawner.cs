using System.Collections;
using System.Collections.Generic;
using RPG.UI.Tooltips;
using UnityEngine;

namespace RPG.UI.Inventories
{
    [RequireComponent(typeof(IItemHolder))]
    public class ItemTooltipSpawner : TooltipSpawner
    {
        public override bool CanCreateTooltip()
        {
            var item = GetComponent<IItemHolder>().GetItem();
            return item != null;
        }

        public override void UpdateTooltip(GameObject tooltip)
        {
            var itemTooltip = tooltip.GetComponent<ItemTooltip>();
            if (!itemTooltip) return;
            var item = GetComponent<IItemHolder>().GetItem();
            itemTooltip.Setup(item);
        }
    }
}

