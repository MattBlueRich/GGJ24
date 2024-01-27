using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTargetAndCustomer : MonoBehaviour
{
    private customerSpawningScript CSS;

    public GameObject customerPrefab;
    public GameObject customerSpawnPoint;

    bool needToSpawnTarget = true;
    public float timeToWaitBetweenCustomerSpawns = 5;
    float currentTime;

    
    void Start()
    {
        CSS = GetComponent<customerSpawningScript>();

        currentTime = timeToWaitBetweenCustomerSpawns;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;

        if(currentTime <= 0.0f)
        {
            if (needToSpawnTarget ) 
                spawnNewTarget();
            else
                spawnNewCustomer();

            currentTime = timeToWaitBetweenCustomerSpawns;
        }
    }

    void spawnNewTarget()
    {
        //need some way to randomize the appearance of the target
        //and save that data for the player to look for
        //should be decided ahead of time so that the target info doesnt appear as soon as the target spawn
        CSS.spawnNewCustomer(new customerAtributes(), customerPrefab, customerSpawnPoint);

    }

    void spawnNewCustomer()
    {
        //need some way to randomize the appearance of the customers


    }
}
