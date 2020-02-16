using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    
    void Update()
    {
        UpdateAnimator();
    }

    public void MoveTo(Vector3 destination)
    {
        this.GetComponent<NavMeshAgent>().destination = destination;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = this.GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = this.transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;

        this.GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

}
