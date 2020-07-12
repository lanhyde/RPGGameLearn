using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Dragging
{
    public interface IDragSource<T> where T : class
    {
        /// <summary>
        /// What item type currently resides in this source?
        /// </summary>
        /// <returns></returns>
        T GetItem();
        /// <summary>
        /// What is the quantity of items in this source?
        /// </summary>
        /// <returns></returns>
        int GetNumber();
        /// <summary>
        /// Remove a given number of items from the source.
        /// </summary>
        /// <param name="number"></param>
        void RemoveItems(int number);
    }
}