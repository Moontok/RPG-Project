using GameDevTV.Utils;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCoolDownTime = 5f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [Range(0,1)][SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        Fighter fighter = null;
        Health health = null;
        Mover mover = null;
        GameObject player = null;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggravated = Mathf.Infinity;
        int currentWaypointIndex = 0;

        LazyValue<Vector3> _guardPosition = null;
        public Vector3 guardPosition
        {
            get {return _guardPosition.value;}
            set {_guardPosition.value = value;}
        }

        private void Awake() 
        {
            fighter = this.GetComponent<Fighter>();
            health = this.GetComponent<Health>();
            mover = this.GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");

            _guardPosition = new LazyValue<Vector3>(GetGuardPosition);            
        }

        private Vector3 GetGuardPosition()
        {
            return this.transform.position;
        }

        private void Start() 
        {
            _guardPosition.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (IsAggravated() && fighter.CanAttack(player))
            {
                AttackBehavior();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehavior();
            }
            else
            {
                PatrolBehavior();
            }

            UpdateTimers();
        }

        public void Aggravate()
        {
            timeSinceAggravated = 0;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceAggravated += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwellTime)
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
            float distanToWaypoint = Vector3.Distance(this.transform.position, GetCurrentWaypoint());
            return distanToWaypoint < waypointTolerance;
        }

        private void SuspicionBehavior()
        {
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehavior()
        {
            timeSinceLastSawPlayer = 0f;
            fighter.Attack(player);

            AggravateNearbyEnemies();
        }

        private void AggravateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(this.transform.position, shoutDistance, Vector3.up, 0f);

            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                
                ai.Aggravate();
            }
        }

        private bool IsAggravated()
        {
            float distanceToPlayer = Vector3.Distance(this.transform.position, player.transform.position);
            return distanceToPlayer < chaseDistance || timeSinceAggravated < aggroCoolDownTime;
        }

        // Called by Unity in the Editor
        private void OnDrawGizmosSelected() 
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}
