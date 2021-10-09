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

        NavMeshAgent navMeshAgent;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)  //interface 
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Cancel() // we need this method to IAction interface to work, otherway it will be red underlined
        {
            navMeshAgent.isStopped = true;
        }

         


        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity; //NavMesh is giving us the global values
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //converting into localVelocity point is that, the animator knows we are moving forward, no matter at what direction
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }

}