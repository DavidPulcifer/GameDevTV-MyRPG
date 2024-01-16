using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float susTime = 5f;
        [SerializeField] float aggroCoolDownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float wayPointDwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceReachedWaypoint = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
        int currentWayPointIndex = 0;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
            guardPosition.ForceInit();
        }

        Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        internal void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(guardPosition.value);
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceReachedWaypoint = Mathf.Infinity;
            timeSinceAggravated = Mathf.Infinity;
            currentWayPointIndex = 0;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            if (health.IsDead()) return;

            GameObject player = GameObject.FindWithTag("Player");
            if (IsAggrevated() && fighter.CanAttack(player))
            {                
                AttackBehavior(player);
            }
            else if (timeSinceLastSawPlayer < susTime)
            {
                SusBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggravated = 0f;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceReachedWaypoint += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition.value;

            if(patrolPath != null)
            {
                if(AtWaypoint())
                {
                    timeSinceReachedWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if(timeSinceReachedWaypoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }            
        }

        private bool AtWaypoint()
        {
            float distanceToWavePointSqrMagnitude = (transform.position - GetCurrentWaypoint()).sqrMagnitude;
            return distanceToWavePointSqrMagnitude < wayPointTolerance * wayPointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }

        private void SusBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior(GameObject player)
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggravateNEarbyEnemies();
        }

        private void AggravateNEarbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                ai.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            float DistanceToPlayerSqrMagnitude = (transform.position - player.transform.position).sqrMagnitude;

            return DistanceToPlayerSqrMagnitude < chaseDistance * chaseDistance || timeSinceAggravated < aggroCoolDownTime;
        }
                
        //Called within Unity IDE
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

