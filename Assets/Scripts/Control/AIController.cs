using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Utils;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        float chaseDistance = 5f;
        [SerializeField]
        float suspicionTime = 3f;
        [SerializeField] private float aggroTime = 5f;
        [SerializeField]
        PatrolPath patrolPath;
        [SerializeField]
        float waypointTolerance = 0.5f;
        [SerializeField]
        float waypointDwellTime = 2f;
        [Range(0, 1), SerializeField]
        float patrolSpeedFraction = 0.2f;

        [SerializeField] private float shoutDistance = 5f;
        private Fighter fighter;
        private GameObject player;
        private Health health;
        private LazyValue<Vector3> guardPosition;
        private Mover mover;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeSinceArriveAtWaypoint = Mathf.Infinity;
        private float timeSinceAggrevated = Mathf.Infinity;
        private int currentWaypointIndex = 0;
        private void Awake()
        {
            fighter = GetComponent<Fighter>();   
            player = GameObject.FindWithTag("Player"); 
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            guardPosition = new LazyValue<Vector3>(() => transform.position);
        }

        private void Start()
        {
            guardPosition.ForceInit();
        }
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (IsAggrevated() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            // Suspicious Behavior
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                // Suspicion state
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }
            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArriveAtWaypoint += Time.deltaTime;
            timeSinceAggrevated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;
            if(patrolPath)
            {
                if(AtWaypoint())
                {
                    timeSinceArriveAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceArriveAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWayPoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWayPoint < waypointTolerance;
        }

        private void SuspicionBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (var hit in hits)
            {
                var ai = hit.collider.GetComponent<AIController>();
                if (ai)
                {
                    ai.Aggrevate();
                }
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance ||
                timeSinceAggrevated < aggroTime;
        }
        // Called by Unity Editor
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
