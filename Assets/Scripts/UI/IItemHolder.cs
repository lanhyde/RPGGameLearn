using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.UI.Inventories
{
    public interface IItemHolder
    {
        InventoryItem GetItem();
    }
}
