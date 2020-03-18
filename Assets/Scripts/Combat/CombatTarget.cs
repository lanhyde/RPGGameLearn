using RPG.Control;
using RPG.Resources;
using UnityEngine;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            if(!callingController.GetComponent<Fighter>().CanAttack(gameObject))
            {
                return false;
            }
            if (Input.GetMouseButton(0))
            {
                callingController.GetComponent<Fighter>().Attack(gameObject);
            }
            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}