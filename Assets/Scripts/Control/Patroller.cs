using GameDevTV.Utils;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Patroller : MonoBehaviour
    {
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float wayPointTolerance = 1f;
        [SerializeField] float wayPointDwellTime = 3f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Mover mover;

        LazyValue<Vector3> aiPosition;
        float timeSinceReachedWaypoint = Mathf.Infinity;
        int currentWayPointIndex = 0;

        // Start is called before the first frame update
        void Awake()
        {
            mover = GetComponent<Mover>();
            aiPosition = new LazyValue<Vector3>(GetAIPosition);
            aiPosition.ForceInit();
        }

        Vector3 GetAIPosition()
        {
            return transform.position;
        }

        internal void Reset()
        {
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(aiPosition.value);
            timeSinceReachedWaypoint = Mathf.Infinity;
            currentWayPointIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            PatrolBehavior();
            UpdateTimers();
        }

        void UpdateTimers()
        {
            timeSinceReachedWaypoint += Time.deltaTime;
        }

        public void PatrolBehavior()
        {
            Vector3 nextPosition = aiPosition.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceReachedWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeSinceReachedWaypoint > wayPointDwellTime)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        bool AtWaypoint()
        {
            float distanceToWavePointSqrMagnitude = (transform.position - GetCurrentWaypoint()).sqrMagnitude;
            return distanceToWavePointSqrMagnitude < wayPointTolerance * wayPointTolerance;
        }

        void CycleWaypoint()
        {
            currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);
        }

        Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWayPointIndex);
        }
    }
}
