using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class basicPathFindingCustomer : MonoBehaviour
{
    NavMeshAgent customerAgent;

    Transform target = null;

    bool hasTarget = false;
    bool canMove = false;

    void Start()
    {
        customerAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove && hasTarget) 
        {
            customerAgent.destination = target.position;
        }
        else if (!canMove)
        {
            customerAgent.destination = transform.position;
        }

    }

    public void updateTarget(Transform newTarget)
    {
        target = newTarget;
        hasTarget = true;
    }

    public void updateCanMove(bool canMove) 
    {
        this.canMove = canMove;
    }
}
