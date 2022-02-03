using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] Transform target;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float maxNavPathLength = 40f;

        NavMeshAgent navMeshAgent;
        Health health;

        private void Awake()
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path); //unasigned variable means, we need to assign it :) (give it a value) or create a new - like line before
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;  // path.status report whether the path reaches to the target, is partial, or is invalid           
            if (GetPathLenght(path) > maxNavPathLength) return false;

            return true;

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

        private float GetPathLenght(NavMeshPath path)
        {
            float total = 0;
            if (path.corners.Length < 2) return total; //we can't calculate if there are less than 2 corners, so return
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return total;
        }


        #region Saveabe Interface
        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState() //object can be anything like Vector3, dictionary, bool etc
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;   // we were using this statement when capturing only position, but now thanks to struct we dont need that casting here//new SerializableVector3(transform.position); // we need to return it like this - new c# script serializableVector3 because Unity is thorwin error, s specific case - we can return only basic types 
        }

        public void RestoreState(object state)
        {
            // now we use struct to capture position and rotation//SerializableVector3 position = (SerializableVector3)state;  //!!!!we need to tell c# that this object is a Serializable Vector3, and we do that by casting it, this method will throw an exception if the object is not a serializableVector, - in a case if we are not sure if object will be 100% the one we want we can use --object as TypethatweWant and in a case it is not this Type, we will get null in return // Vector3 is not marked as serializable,,, all basic types are marked as serializable int,bool,float,string , they are binary and can be saved into a file
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false; //to avoid bugs putting us over random position when teleporting
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true; //to avoid bugs
        }
        #endregion
    }

}