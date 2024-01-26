using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

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
    public GameObject OrderFlag;
    public GameObject ParentGameObjectToSittingFlags;
    public GameObject ExitFlag;

    state currentState = state.NeedToOrder;
    state wantedState = state.NeedToOrder; 

    bool isAtMovementFlag = false;

    GameObject currentMovementFlag = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        think();    //the AI will think about what it is doing (choosing movement flag)
        act();     //the AI will then move act based on its choice
    }

    void think()
    {
        if(!isAtMovementFlag)
        {
            if(currentState==state.Waiting_For_Order)
            {
                //this will need a function that checks for a given order and check if order was correct
            }
            else if(currentState==state.Waiting_To_Finish_Eating)
            {
                //this will need a function that will wait a random amount of time then will move to the NeedToLeave state
            }
            else if(currentState==state.Waiting_For_Space)
            {
                GameObject nextMovementFlag;
                nextMovementFlag = findNextMovementFlag(wantedState);

                if (nextMovementFlag)   //a gameObject was found and returned
                {
                    currentMovementFlag = nextMovementFlag;
                    currentState = wantedState;
                }
            }
            else if(currentState == state.NeedToOrder)
            {
                updateFlagsAndStates();
            }
            else if(currentState == state.NeedToEat)
            {
                updateFlagsAndStates();
            }
            else if(currentState == state.NeedToLeave)
            {
                updateFlagsAndStates();
            }
        }
        else
        {
            if (!(currentState == state.Waiting_For_Order || currentState == state.Waiting_To_Finish_Eating))
            {
                //if it is not waiting for order or not waiting to finish eating

                if (currentMovementFlag.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
                {
                    //if it is being used by itself do nothing else switch state to waiting for space
                }
            }
        }
    }

    void act()
    {

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
                currentMovementFlag = OrderFlag;
            }
        }
        else if(currentState == state.NeedToEat)
        {
            foreach(Transform child in ParentGameObjectToSittingFlags.transform)    //loop of each of the sitting flags an checks if it is being used
            {
                if(!child.gameObject.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
                {
                    currentMovementFlag = child.gameObject;
                }
            }
        }
        else if(currentState == state.NeedToLeave)
        {
            if(!ExitFlag.GetComponent<movementFlag>().movementFlag_getIsBeingUsed())
            {
                currentMovementFlag = ExitFlag; 
            }
        }

        return returnFlag;
    }

    void updateFlagsAndStates()
    {
        //used to move clutter out of the Think()
        GameObject nextMovementFlag;
        nextMovementFlag = findNextMovementFlag(currentState);

        if (nextMovementFlag)   //next flag was found
            currentMovementFlag = nextMovementFlag;
        else
        {
            wantedState = currentState; //no free slots so go to waiting - and update wanted
            currentState = state.Waiting_For_Space;
        }
    }
}
