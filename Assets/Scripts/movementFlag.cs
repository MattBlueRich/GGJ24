using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum FlagTypes   //hold the flagTypes - this can be expanded to include more flag types
{
    flag_spawn,
    flag_wait_at_counter,
    flag_sitting_at_table,
    flag_exit
}


public class movementFlag : MonoBehaviour
{
    //this will be used at certain locations to show where AI can move to

    public FlagTypes flagType;  //hold the type it is so other can know what it is
    bool currentBeingUsed = false;  //checks if it is being used - so nothing can be in the same space
    GameObject currentlyBeingUsedBy = null;
    
    public bool movementFlag_getIsBeingUsed()   //could make the bool public but doing it this way is safer
    {
        return currentBeingUsed;
    }

    public void movementFlag_setIsBeingUsed(bool updatedIsBeingUsed)
    {
        if(!updatedIsBeingUsed)
        {
            currentlyBeingUsedBy = null;
        }

        currentBeingUsed = updatedIsBeingUsed;
    }

    public GameObject movementFlag_getCurrentlyBeingUsedBy()
    {
        return currentlyBeingUsedBy;
    }

    public void movementFlag_setCurrentlyBeingUsedBy(GameObject newBeingUseBy)
    {
        currentlyBeingUsedBy=newBeingUseBy;
    }
}
