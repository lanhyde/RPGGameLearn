using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.Dragging
{
    public interface IDragDestination<T> where T : class
    {
        /// <summary>
        /// How many of given items can be accepted.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        int MaxAcceptable(T item);
        /// <summary>
        /// Update the UI and any data to reflect adding the item to this destination
        /// </summary>
        /// <param name="item"></param>
        /// <param name="number"></param>
        void AddItems(T item, int number);
    }
}