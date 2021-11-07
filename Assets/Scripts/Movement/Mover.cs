using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)  //interface 
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel() // we need this method to IAction interface to work, otherway it will be red underlined
        {
            navMeshAgent.isStopped = true;
        }

        //public void SetMovementSpeed(float speed)  //my solution to change enemy movement speed when chasing and patrolling
        //{
        //    navMeshAgent.speed = speed;
        //}


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity; //NavMesh is giving us the global values
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //converting into localVelocity point is that, the animator knows we are moving forward, no matter at what direction
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }

}