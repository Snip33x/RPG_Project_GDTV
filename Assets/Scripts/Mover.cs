using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        UpdateAnimator();
    }

    public void MoveTo(Vector3 destination)  //interface 
    {
        GetComponent<NavMeshAgent>().destination = destination;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity; //NavMesh is giving us the global values
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //converting into localVelocity point is that, the animator knows we are moving forward, no matter at what direction
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }
}
