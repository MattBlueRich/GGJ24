using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

enum state
{
    NeedToOrder,
    Waiting_For_Order,
    NeedToEat,
    Waiting_To_Finish_Eating,
    NeedToLeave,
    Waiting_For_Space
}


public class basicCustomerScript : MonoBehaviour
{
    Rigidbody rb;

    public GameObject Player;

    public GameObject OrderFlag;
    public GameObject ParentGameObjectToSittingFlags;
    public GameObject ExitFlag;

    state currentState = state.NeedToOrder;
    state wantedState = state.NeedToOrder; 

    bool isAtMovementFlag = false;
    bool justFinishedAction = true;

    GameObject currentMovementFlag = null;

    //stats
    public float flagTollerance = 0.5f;

    public float movementSpeed = 5;

    public int secondsToWaitBeforeTableIsClear = 5;
    public int secondsToWaitBeforeOrderIsClear = 1;

    public int minTimeToTakeEating = 5;
    public int maxTimeToTakeEating = 15;
    int timeToTakeEating = 0;

    bool hasOrderBeenGiven = false;

    bool hasStartedEating = false;
    bool hasFinishedEating = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        timeToTakeEating = Random.Range(minTimeToTakeEating, maxTimeToTakeEating+1);

        Debug.Log(timeToTakeEating);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E)) 
        {
            //Debug testing

            if (currentState == state.Waiting_For_Order)
            {
                hasOrderBeenGiven = true;
            }
        }

        think();    //the AI will think about what it is doing (choosing movement flag)
        act();     //the AI will then move act based on its choice
    }

    void think()
    {
        if(!isAtMovementFlag)
        {   
            if(currentState==state.Waiting_For_Space)
            {
                GameObject nextMovementFlag = null;
                nextMovementFlag = findNextMovementFlag(wantedState);

                if (nextMovementFlag != null)   //a gameObject was found and returned
                {
                    currentMovementFlag = nextMovementFlag;
                    currentState = wantedState;
                }
            }
            else
            {
                if(justFinishedAction)
                {
                    //find next flag - also happens as soon as the customer starts
                    bool flagWasFound = false;
                    flagWasFound = updateFlagsAndStates();
                    if(flagWasFound) 
                    {
                        justFinishedAction = false;
                    };
                }
                else
                {
                    if (currentMovementFlag.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
                    {
                        //old flag was being used need to find a new one
                        updateFlagsAndStates();
                    }

                    if (checkIfAtFlag(gameObject.transform, currentMovementFlag.transform))
                    {
                        //if the customer is at the location of the movement flag
                        currentMovementFlag.GetComponent<movementFlag>().movementFlag_setIsBeingUsed(true);
                        currentMovementFlag.GetComponent<movementFlag>().movementFlag_setCurrentlyBeingUsedBy(gameObject);

                        isAtMovementFlag = true;

                        switch (currentState)
                        {
                            case state.NeedToOrder:
                                /////////////////////////////////////////////////////////////////////////////////look at player
                                currentState = state.Waiting_For_Order;
                                break;
                            case state.NeedToEat:
                                /////////////////////////////////////////////////////////////////////////////////look at food
                                currentState = state.Waiting_To_Finish_Eating;
                                break;
                            case state.NeedToLeave: //if the gameObject needs to leave and is at the leave flag it "leaves"
                                Debug.Log("gameobject has left");
                                currentMovementFlag.GetComponent<movementFlag>().movementFlag_setIsBeingUsed(false);
                                Destroy(gameObject);
                                break;
                            default:
                                break;
                        }

                    }
                }
            } 
        }
        else
        {
            if (!(currentState == state.Waiting_For_Order || currentState == state.Waiting_To_Finish_Eating))
            {
                //if it is not waiting for order or not waiting to finish eating, the it should be moving
                isAtMovementFlag = false;
                currentMovementFlag.GetComponent<movementFlag>().movementFlag_setIsBeingUsed(false);
                updateFlagsAndStates();
            }
            else if (currentState == state.Waiting_For_Order)
            {
                //this will need a function that checks for a given order and check if order was correct
                if(checkOrderIsCorrect())
                {
                    justFinishedAction = true;
                    currentState = state.NeedToEat;
                    isAtMovementFlag = false;
                    StartCoroutine(waitToClearFlag(secondsToWaitBeforeOrderIsClear, currentMovementFlag.GetComponent<movementFlag>()));
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////complain/ask for order again
                }
            }
            else if (currentState == state.Waiting_To_Finish_Eating)
            {
                //if customer hasnt started eating then they need to start
                if (!hasStartedEating)
                {
                    StartCoroutine(startEatingCountdown(timeToTakeEating));
                }

                //this will need a function that will wait a random amount of time then will move to the NeedToLeave state
                if(hasFinishedEating)
                {
                    //task finished so change state and mark flag as empty
                    justFinishedAction = true;
                    currentState = state.NeedToLeave;
                    isAtMovementFlag = false;
                    StartCoroutine(waitToClearFlag(secondsToWaitBeforeTableIsClear, currentMovementFlag.GetComponent<movementFlag>()));
                }
            }
        }
    }

    void act()
    {
        switch(currentState) 
        {
            case state.Waiting_For_Space:
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case state.Waiting_For_Order:
                gameObject.transform.LookAt(new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z));
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case state.Waiting_To_Finish_Eating:
                rb.velocity = new Vector3(0,0,0);
                break;
            default:
                //move the player towards its target 
                gameObject.transform.LookAt(new Vector3(currentMovementFlag.transform.position.x, gameObject.transform.position.y, currentMovementFlag.transform.position.z));
                rb.velocity = transform.forward * movementSpeed;
                break;
        }
    }

    GameObject findNextMovementFlag(state currentState)
    {
        //this will take the a state an based on that state will return the next empty flag gameObject
        //if no empty flag was found then it will return null
        GameObject returnFlag = null;

        if(currentState == state.NeedToOrder)
        {
            if(!OrderFlag.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
            {
                returnFlag = OrderFlag;
            }
        }
        else if(currentState == state.NeedToEat)
        {
            bool seatFound = false;
            foreach(Transform child in ParentGameObjectToSittingFlags.transform)    //loop of each of the sitting flags an checks if it is being used
            {
                //as this is a for loop they will always fill up the seats in oder

                if(!child.gameObject.GetComponent<movementFlag>().movementFlag_getIsBeingUsed() && !seatFound)
                {
                    seatFound = true;
                    returnFlag = child.gameObject;
                }
            }
        }
        else if(currentState == state.NeedToLeave)
        {
            if(!ExitFlag.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
            {
                returnFlag = ExitFlag; 
            }
        }

        return returnFlag;
    }

    bool updateFlagsAndStates()
    {
        bool found = false;
        //used to move clutter out of the Think()
        GameObject nextMovementFlag = null;
        nextMovementFlag = findNextMovementFlag(currentState);

        if (nextMovementFlag != null)   //next flag was found
        {
            currentMovementFlag = nextMovementFlag;
            found = true;
        }
        else
        {
            wantedState = currentState; //no free slots so go to waiting - and update wanted
            currentState = state.Waiting_For_Space;
            found = false;
        }

        return found;
    }

    bool checkIfAtFlag(Transform customerTransform, Transform flagTransform)
    {
        bool xCorrect = false;
        bool yCorrect = false;

        if (customerTransform.position.x >= flagTransform.position.x-flagTollerance && customerTransform.position.x <= flagTransform.position.y+flagTollerance)
        {
            xCorrect = true;
        }

        if(customerTransform.position.z >= flagTransform.position.z-flagTollerance && customerTransform.position.z <= flagTransform.position.z+flagTollerance)
        {
            yCorrect = true;
        }

        if (xCorrect && yCorrect)
            return true;
        else
            return false;
    }

    IEnumerator waitToClearFlag(float waitTime, movementFlag flagToClear)
    {
        //this will wait for the amout of time then will clear the pass in flag
        while(true)
        {
            yield return new WaitForSeconds(waitTime);
            flagToClear.movementFlag_setIsBeingUsed(false);
        }
    }

    IEnumerator startEatingCountdown(float waitTime) 
    {
        hasStartedEating = true;
        while(true)
        {
            yield return new WaitForSeconds(waitTime);
            hasFinishedEating = true;
        }
    }

    public void orderHasBeenGiven()
    {
        //pass in a struct of what the order contains

        //this will be called when the player gives an order
        hasOrderBeenGiven = true;
    }

    bool checkOrderIsCorrect()
    {
        if(!hasOrderBeenGiven)
            return false;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////check if order is correct
        /////////////////////////////////////////////////////////////////////////////////////////////////////////if it is return true
        /////////////////////////////////////////////////////////////////////////////////////////////////////////else return false
        return true;
    }
}
