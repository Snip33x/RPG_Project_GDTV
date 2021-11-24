using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
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

        #region Saveabe Interface
        public object CaptureState() //object can be anything like Vector3, dictionary, bool etc
        {
            return new SerializableVector3(transform.position); // we need to return it like this - new c# script serializableVector3 because Unity is thorwin error, s specific case 
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;  //!!!!we need to tell c# that this object is a Serializable Vector3, and we do that by casting it, this method will throw an exception if the object is not a serializableVector, - in a case if we are not sure if object will be 100% the one we want we can use --object as TypethatweWant and in a case it is not this Type, we will get null in return
            GetComponent<NavMeshAgent>().enabled = false; //to avoid bugs
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true; //to avoid bugs
        }
        #endregion
    }

}