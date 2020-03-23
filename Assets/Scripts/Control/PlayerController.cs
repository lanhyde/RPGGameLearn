using System;
using System.Collections;
using RPG.Combat;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.EventSystems;
namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }
        private Health health;
        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxNavmeshProjectionDistance = 1.0f;
        [SerializeField] private float raycastRadius = 1f;
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
            RaycastHit[] hits = RaycastAllSorted();
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));
            return hits;
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
            if (RaycastNavMesh(out Vector3 target))
            {
                if (!GetComponent<Mover>().CanMoveTo(target))
                {
                    return false;
                }
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, fullSpeedFraction);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = Vector3.zero;
            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            bool hasCastToNavmesh = NavMesh.SamplePosition(hit.point, out NavMeshHit meshHit,
                maxNavmeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavmesh) return false;
            
            target = meshHit.position;

            return true;
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