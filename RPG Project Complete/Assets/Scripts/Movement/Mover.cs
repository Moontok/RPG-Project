using System.Collections.Generic;
using RPG.Core;
using RPG.Resources;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent = null;
        Health health = null;

        private void Start() 
        {
            navMeshAgent = this.GetComponent<NavMeshAgent>();
            health = this.GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            this.GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = this.transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;

            this.GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;  
            Vector3 position = ((SerializableVector3)data["position"]).ToVector();          
            this.GetComponent<NavMeshAgent>().Warp(position);
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
