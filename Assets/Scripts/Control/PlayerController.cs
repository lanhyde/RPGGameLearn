using System;
using RPG.Combat;
using RPG.Movement;
using RPG.Resources;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        enum CursorType 
        {
            None, Movement, Combat, UI
        }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        private Health health;
        [SerializeField] CursorMapping[] cursorMappings = null;
        private const float fullSpeedFraction = 1.0f;
        private void Awake()
        {
            health = GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            if(InteractWithUI()) return;
            if(health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if(InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(CursorType.Combat);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool InteractWithUI()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            if (Physics.Raycast(GetMouseRay(), out RaycastHit hit))
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, fullSpeedFraction);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType cursorType)
        {
            var mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            var targetCursor = cursorMappings.SingleOrDefault(c => c.type == type);
            return targetCursor;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

}