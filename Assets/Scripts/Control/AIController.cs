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
        [SerializeField] float aggroDistance = 5f;
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float susTime = 5f;
        [SerializeField] float aggroCoolDownTime = 5f;        
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter;
        Health health;
        GameObject player;
        Patroller patroller;

        LazyValue<Vector3> aiPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;

        private void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            patroller = GetComponent<Patroller>();
            player = GameObject.FindWithTag("Player");

            aiPosition = new LazyValue<Vector3>(GetAIPosition);
            aiPosition.ForceInit();
        }

        Vector3 GetAIPosition()
        {
            return transform.position;
        }

        internal void Reset()
        {
            patroller.Reset();
            NavMeshAgent navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(aiPosition.value);
            timeSinceLastSawPlayer = Mathf.Infinity;
            timeSinceAggravated = Mathf.Infinity;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            GameObject player = GameObject.FindWithTag("Player");
            if (IsAggrevated() && fighter.CanAttack(player))            {
                
                AttackBehavior(player);
            }
            else if (ShouldChase())
            {
                Debug.Log("chase");
                ChaseBehavior();
            }
            else if (timeSinceLastSawPlayer < susTime)
            {
                //SusBehavior();
            }
            else
            {
                patroller.PatrolBehavior();
            }

            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggravated = 0f;
        }

        void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }        
                
        void SusBehavior()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        void ChaseBehavior()
        {
            timeSinceLastSawPlayer = 0;
            //fighter.MoveToTarget();
        }

        void AttackBehavior(GameObject player)
        {            
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggravateNearbyEnemies();
        }

        void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null || ai == this) continue;
                ai.Aggrevate();
            }
        }

        bool IsAggrevated()
        {
            float DistanceToPlayerSqrMagnitude = (transform.position - player.transform.position).sqrMagnitude;

            return DistanceToPlayerSqrMagnitude < aggroDistance * aggroDistance || timeSinceAggravated < aggroCoolDownTime;
        }

        bool ShouldChase()
        {
            float DistanceToPlayerSqrMagnitude = (transform.position - player.transform.position).sqrMagnitude;

            return DistanceToPlayerSqrMagnitude < chaseDistance * chaseDistance || timeSinceAggravated < aggroCoolDownTime;
        }
                
        //Called within Unity IDE
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, aggroDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}

